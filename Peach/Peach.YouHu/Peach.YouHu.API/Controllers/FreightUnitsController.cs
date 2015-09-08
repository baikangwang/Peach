namespace Peah.YouHu.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Peah.YouHu.API.Component;
    using Peah.YouHu.API.Models;

    [Authorize]
    [RoutePrefix("api")]
    public class FreightUnitsController : BaseController
    {
        // GET: api/FreightUnits/List
        [Route("Driver/FreightUnits/List")]
        [ResponseType(typeof(IList<FreightUnitViewModel>))]
        [HttpGet]
        public async Task<IHttpActionResult> List()
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IList<FreightUnitViewModel> view = await this.AppDb.FreightUnits.Include(u => u.Driver)
                .Where(u => u.Driver.Id == this.LogonId)
                .Select(u => new FreightUnitViewModel(){Id = u.Id,Rank = u.Driver.Rank,Driver = u.Driver.FullName,Location = u.Location,State = u.State})
                .ToListAsync();

            view = view.Select(model =>
            {
                // address + "|" +value.city + "|" + point.lng + "," + point.lat;
                string dest = model.Location;
                var segaments1 = dest.Split(new[] { "|", "," }, StringSplitOptions.None);
                if (segaments1.Length == 1)
                {
                    return model;
                }
                else
                {
                    model.LongLocation = segaments1[0];
                    model.ShortLocation = segaments1[1];

                    //double lon1, lat1;
                    //lon1 = Convert.ToDouble(segaments1[2]);
                    //lat1 = Convert.ToDouble(segaments1[3]);
                    // model.Destance = (decimal)Math.Round(Utility.GetLongDistance(lon1, lat1, lon2, lat2) / 1000, 2, MidpointRounding.ToEven);
                }
                return model;

            }).ToList();

            return this.Ok(view);
        }

        // GET: api/freightUnits/find
        [Route("Owner/FreightUnits/Find")]
        [ResponseType(typeof(IList<FreightUnitViewModel>))]
        [HttpGet]
        public async Task<IHttpActionResult> Find(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.AppDb.Orders.FindAsync(id);

            IList<FreightUnitViewModel> view = await this.AppDb.FreightUnits
                .Include(u=>u.Driver)
                //todo: implement later
                //.Where(u=>FreightUnitFinder.Default.Match(u.Length,u.Height,order.Size,u.Weight,order.Weight,u.Location,order.Source))
                .Where(u => /*(order.Source == u.Location) &&*/ (order.Weight <= u.Weight) && (order.Size <= (u.Length * u.Height)))
                .Select(u => new FreightUnitViewModel()
                             {
                                 Id = u.Id, Rank = u.Driver.Rank, Driver = u.Driver.FullName, Location = u.Location,
                                 LongLocation = u.Location,
                                 ShortLocation = u.Location,
                                 Destance = 0.0m
                             })
                .OrderByDescending(v=>v.Rank)
                .ToListAsync();


            string source = order.Source;
            var segaments2 = source.Split(new[] { "|", "," }, StringSplitOptions.None);
            double lon2, lat2;
            lon2 = Convert.ToDouble(segaments2[2]);
            lat2 = Convert.ToDouble(segaments2[3]);

            view = view.Where(model=>
            {
                string dest = model.Location;
                var segaments1 = dest.Split(new[] { "|", "," }, StringSplitOptions.None);
                // filter freight unit is staying in the same city as the order
                return Regex.IsMatch(segaments1[1], segaments2[1], RegexOptions.Compiled | RegexOptions.IgnoreCase);

            }).Select(model =>
            {
                model.Cost = FreightCostCalculator.Default.Calculate(model.Location, order.Destination, order.Size, order.Weight);

                // address + "|" +value.city + "|" + point.lng + "," + point.lat;
                string dest = model.Location;
                var segaments1 = dest.Split(new[] { "|", "," }, StringSplitOptions.None);
                if (segaments1.Length == 1 || segaments2.Length == 1)
                {
                    return model;
                }
                else
                {
                    model.LongLocation = segaments1[0];
                    model.ShortLocation = segaments1[1];

                    double lon1, lat1;
                    lon1 = Convert.ToDouble(segaments1[2]);
                    lat1 = Convert.ToDouble(segaments1[3]);
                    model.Destance = (decimal)Math.Round(Utility.GetLongDistance(lon1, lat1, lon2, lat2) / 1000, 2, MidpointRounding.ToEven);
                }
                return model;

            }).ToList();

            return this.Ok(view);
        }

        // POST: api/freightUnits/Publish
        [Route("Driver/FreightUnits/Publish")]
        public async Task<IHttpActionResult> Publish(PublishFreightUnitBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            FreightUnit unit = await this.AppDb.FreightUnits.FindAsync(model.Id);

            unit.Location = model.Location;
            unit.State = FreightUnitState.Ready;
            unit.ModifiedDate = DateTime.Now;
            unit.ModifiedBy = this.LogonId;

            this.AppDb.Entry(unit).State = EntityState.Modified;

            try
            {
               int added= await this.AppDb.SaveChangesAsync();

                if (added <= 0)
                    return this.BadRequest("Fail to publish the freight unit");
            }
            catch (Exception ex)
            {
                return this.BadRequest("Fail to publish the freight unit. "+ ex.Message);
            }

            return this.Ok();
        }

        // POST: api/freightUnits/Publish
        [Route("Driver/FreightUnits/Register")]
        public async Task<IHttpActionResult> Register(RegisterFreightUnitBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            //int driverId = model.DriverId;
            AppUser driver = this.Logon;

            FreightUnit fu = new FreightUnit();
            fu.Location = model.Location;
            fu.ModifiedBy = this.LogonId; //driverId;
            fu.ModifiedDate=DateTime.Now;
            fu.State=FreightUnitState.None;
            fu.Height = model.Height;
            fu.Length = model.Length;
            fu.Licence = model.Licence;
            fu.Type = model.Type;
            fu.Weight = model.Weight;

            fu.Driver = driver;

            try
            {
                this.AppDb.Entry(fu).State = EntityState.Added;
                int added = await this.AppDb.SaveChangesAsync();

                if (added <= 0)
                    return this.BadRequest("Fail to register freight unit");
            }
            catch (Exception ex)
            {
                return this.BadRequest("Fail to register freight unit. " + ex.Message);
            }

            return this.Ok();
        }
    }
}