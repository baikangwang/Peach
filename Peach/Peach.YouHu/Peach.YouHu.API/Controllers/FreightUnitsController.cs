using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Peah.YouHu.API.Models;

namespace Peah.YouHu.API.Controllers
{
    using System.Threading;

    using Peah.YouHu.API.Models.Enum;

    [RoutePrefix("api/FreightUnits")]
    public class FreightUnitsController : ApiController
    {
        private OwnerDbContext db = new OwnerDbContext();

        // GET: api/FreightUnits/List
        [Route("List")]
        [ResponseType(typeof(IList<FreightUnitViewModel>))]
        public async Task<IHttpActionResult> List(string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IList<FreightUnitViewModel> view = await this.db.FreightUnits.Include(u => u.Driver)
                .Where(u => u.Driver.Id == id)
                .Select(u => new FreightUnitViewModel(u.Id, u.Driver.Rank, u.Driver.Name, u.Location))
                .ToListAsync();

            return this.Ok(view);
        }

        // GET: api/freightUnits/find
        [Route("Find")]
        [ResponseType(typeof(IList<FreightUnitViewModel>))]
        public async Task<IHttpActionResult> Find(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await db.Orders.FindAsync(id);

            IList<FreightUnitViewModel> view = await db.FreightUnits
                .Include(u=>u.Driver)
                .Where(u=>FreightUnitFinder.Default.Match(u.Height,u.Height,order.Size,u.Weight,order.Weight,u.Location,order.Source))
                .Select(u=>new FreightUnitViewModel(u.Id,u.Driver.Rank,u.Driver.Name,u.Location))
                .OrderByDescending(v=>v.Rank)
                .ToListAsync();

            view = view.Select(v=>
            {
                v.Cost = FreightCostCalculator.Default.Calculate(v.Location, order.Destination, order.Size, order.Weight);
                return v;
            }).ToList();

            return this.Ok(view);
        }

        // POST: api/freightUnits/Publish
        [Route("Publish")]
        public async Task<IHttpActionResult> Publish(PublishFreightUnitBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            FreightUnit unit = await this.db.FreightUnits.FindAsync(model.Id);

            unit.Location = model.Location;
            unit.State=FreightUnitState.Ready;
            unit.ModifiedDate=DateTime.Now;
            unit.ModifiedBy = model.ModifiedBy;

            this.db.Entry(unit).State=EntityState.Modified;

            try
            {
               await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return this.BadRequest("Fail to publish the freight unit");
            }

            return this.Ok();
        }

        // POST: api/freightUnits/Publish
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterFreightUnitBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            int driverId = model.DriverId;
            Driver driver = await this.db.Drivers.FindAsync(driverId);

            FreightUnit fu=new FreightUnit();
            fu.Location = model.Location;
            fu.ModifiedBy = driverId;
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
                this.db.Entry(fu).State = EntityState.Added;
                await this.db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return this.BadRequest("Fail to register freight unit");
            }

            return this.Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}