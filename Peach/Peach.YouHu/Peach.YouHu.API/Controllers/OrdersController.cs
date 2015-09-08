namespace Peah.YouHu.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Peah.YouHu.API.Component;
    using Peah.YouHu.API.Models;

    [Authorize]
    [RoutePrefix("api")]
    public class OrdersController : BaseController
    {
        #region Publish
        // POST: api/Orders/Publish
        [Route("Owner/Orders/Publish")]
        public async Task<IHttpActionResult> Publish(PublishBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = new Order();
            order.Description = model.Description;
            order.Destination = model.Destination;
            order.Source = model.Source;
            order.Size = model.Size;
            order.Weight = model.Weight;
            order.State = OrderState.Ready;
            order.PublishedDate = DateTime.Now;
            order.ModifiedDate=DateTime.Now;
            order.ModifiedBy = this.LogonId;
            order.Owner = this.Logon;
            order.ConsignCode = ConsignCode.Keygen.Generate();
            this.AppDb.Orders.Add(order);

            try
            {
                this.AppDb.Entry(order).State = EntityState.Added;
                int added = await this.AppDb.SaveChangesAsync();

                if (added > 0)
                {
                    return this.Ok();
                }
                else
                {
                    return this.BadRequest();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.BadRequest();
            }
        }
        #endregion

        #region Owner/List
        // GET: api/Orders/Owner/Orders
        [Route("Owner/Orders/List")]
        [ResponseType(typeof(IList<OwnerOrderViewModel>))]
        [HttpGet]
        public async Task<IHttpActionResult> OwnerOrders()
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            string id = this.LogonId;

            IList<OwnerOrderViewModel> view = await this.AppDb.Orders
                .Include(o => o.Owner)
                .Include(o => o.FreightUnit)
                .Include(o => o.FreightUnit.Driver)
                .Where(o => /*o.State != OrderState.Consigned &&*/ o.Owner.Id == id)
                .Select(o => new OwnerOrderViewModel()
                {
                    Id = o.Id,
                    Driver = o.FreightUnit.Driver.FullName,
                    Destination = o.Destination,
                    Description = o.Description,
                    PublishedDate = o.PublishedDate,
                    State = o.State,
                    FreightCost = o.FreightCost ?? (0.0m),
                    Source = o.Source,
                    ConsignCode = o.ConsignCode,
                    ShortDestination = o.Destination,
                    LongDestination = o.Destination,
                    ShortSource = o.Source,
                    LongSource = o.Source,
                    Destance = 0.0m
                })
                .ToListAsync();

            view = view.Select(model =>
            {
                // address + "|" +value.city + "|" + point.lng + "," + point.lat;
                string dest = model.Destination;
                string source = model.Source;
                var segaments1 = dest.Split(new[] { "|", "," }, StringSplitOptions.None);
                var segaments2 = source.Split(new[] { "|", "," }, StringSplitOptions.None);
                if (segaments1.Length == 1 || segaments2.Length == 1)
                {
                    return model;
                }
                else
                {
                    model.LongDestination = segaments1[0];
                    model.ShortDestination = segaments1[1];

                    model.LongSource = segaments2[0];
                    model.ShortSource = segaments2[1];
                    double lon1, lat1, lon2, lat2;
                    lon1 = Convert.ToDouble(segaments1[2]);
                    lat1 = Convert.ToDouble(segaments1[3]);
                    lon2 = Convert.ToDouble(segaments2[2]);
                    lat2 = Convert.ToDouble(segaments2[3]);
                    model.Destance = (decimal)Math.Round(Utility.GetLongDistance(lon1, lat1, lon2, lat2) / 1000, 2, MidpointRounding.ToEven);
                }
                return model;
            }).ToList();

            return this.Ok(view);
        }
        #endregion

        #region Driver/List
        [Route("Driver/Orders/List")]
        [ResponseType(typeof(IList<DriverOrderViewModel>))]
        [HttpGet]
        public async Task<IHttpActionResult> DriverOrders()
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            string id = this.LogonId;

            IList<DriverOrderViewModel> view = await this.AppDb.Orders
                .Include(o => o.Owner)
                .Include(o => o.FreightUnit)
                .Include(o => o.FreightUnit.Driver)
                .Where(o => /*o.State != OrderState.Consigned &&*/ o.FreightUnit.Driver.Id == id)
                .Select(o => new DriverOrderViewModel()
                             {
                                 Id = o.Id, OwnerName = o.Owner.FullName, 
                                 Source = o.Source, Destination = o.Destination, 
                                 Description = o.Description, PublishedDate = o.PublishedDate, 
                                 State = o.State, FreightCost = o.FreightCost,Weight=o.Weight,Size=o.Size,
                                 ShortSource = o.Source,
                                 LongSource = o.Source,
                                 ShortDestination = o.Destination,
                                 LongDestination = o.Destination,
                                 Destance = 0.0m
                             })
                .ToListAsync();

            view = view.Select(
                model =>
                {
                    if (model.FreightCost == null || model.FreightCost == 0.0m) model.FreightCost = FreightCostCalculator.Default.Calculate(model.Source, model.Destination, model.Weight, model.Size);

                    // address + "|" +value.city + "|" + point.lng + "," + point.lat;
                    string dest = model.Destination;
                    string source = model.Source;
                    var segaments1 = dest.Split(new[] { "|", "," }, StringSplitOptions.None);
                    var segaments2 = source.Split(new[] { "|", "," }, StringSplitOptions.None);
                    if (segaments1.Length == 1 || segaments2.Length == 1)
                    {
                        return model;
                    }
                    else
                    {
                        model.LongDestination = segaments1[0];
                        model.ShortDestination = segaments1[1];

                        model.LongSource = segaments2[0];
                        model.ShortSource = segaments2[1];

                        double lon1, lat1, lon2, lat2;
                        lon1 = Convert.ToDouble(segaments1[2]);
                        lat1 = Convert.ToDouble(segaments1[3]);
                        lon2 = Convert.ToDouble(segaments2[2]);
                        lat2 = Convert.ToDouble(segaments2[3]);
                        model.Destance = (decimal)Math.Round(Utility.GetLongDistance(lon1, lat1, lon2, lat2) / 1000, 2, MidpointRounding.ToEven);
                    }
                    return model;
                })
                .ToList();

            return this.Ok(view);
        }
        #endregion

        #region MakeDeal
        // POST: api/Orders/Owner/MakeDeal
        [Route("Owner/Orders/MakeDeal")]
        public async Task<IHttpActionResult> MakeDeal(MakeDealBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            
            Order order = await this.AppDb.Orders.FindAsync(model.OrderId);
            FreightUnit fu = await this.AppDb.FreightUnits.FindAsync(model.FreightUnitId);
            order.FreightUnit = fu;
            order.ModifiedBy = this.LogonId;
            order.ModifiedDate=DateTime.Now;
            order.State = OrderState.Dealing;
            this.AppDb.Entry(order).State = EntityState.Modified;
            
            try
            {
                int added = await this.AppDb.SaveChangesAsync();

                if (added > 0)
                {
                    return this.Ok();
                }
                else
                {
                    return this.BadRequest();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.BadRequest();
            }
        }
        #endregion

        #region Pay
        // POST: api/Orders/Owner/Pay
        [Route("Owner/Orders/Pay")]
        public async Task<IHttpActionResult> Pay(PayBdingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.AppDb.Orders.FindAsync(model.OrderId);
            string paymentCode = order.Owner.PaymentCode;
            if (paymentCode != model.PaymentCode)
                return this.BadRequest("Invalid Payment Code");

            order.State = OrderState.Paying;
            order.ModifiedDate=DateTime.Now;
            order.ModifiedBy = this.LogonId;
            this.AppDb.Entry(order).State = EntityState.Modified;

            AppUser owner = order.Owner;

            try
            {
                await this.AppDb.SaveChangesAsync();
            }
            catch (Exception)
            {
                return this.BadRequest("Fail to update order state to Paying");
            }

            bool paid = await PaymentPlatment.Default.Pay(model.Paid,owner.FullName);

            if (paid)
            {
                order.State = OrderState.Paid;
                order.ModifiedDate = DateTime.Now;
                order.ModifiedBy = this.LogonId;
                order.Paid = model.Paid;
                // order.FreightCost = model.FreightCost;
                this.AppDb.Entry(order).State = EntityState.Modified;
                await this.AppDb.SaveChangesAsync();
            }
            else
                return this.BadRequest("Fail to pay, try a later");

            return this.Ok();
        }
        #endregion

        #region Consign
        // POST: api/Orders/Owner/Consign
        [Route("Owner/Orders/Consign")]
        public async Task<IHttpActionResult> Consign(OwnerConsignBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.AppDb.Orders.FindAsync(model.OrderId);
            
            string code = order.Owner.PaymentCode;
            if (code != model.PaymentCode)
                return this.BadRequest("Invalid Consign Code");

            order.State = OrderState.Consigned;
            order.ModifiedDate = DateTime.Now;
            order.ModifiedBy = this.LogonId;

            FreightUnit fu = order.FreightUnit;
            fu.State = FreightUnitState.Ready;
            fu.ModifiedDate = DateTime.Now;
            fu.ModifiedBy = this.LogonId;

            DriverExt ext = this.AppDb.DriverExts.FirstOrDefault(de => de.Driver.Id == order.FreightUnit.Driver.Id);
            bool isNew = false;
            decimal paid = order.Paid ?? 0;

            if (ext == null)
            {
                ext = new DriverExt()
                      {
                          CurrentIncome = 0,
                          TotalIncome = 0,
                          Driver = order.FreightUnit.Driver
                      };
                isNew = true;
            }
            
            ext.TotalIncome += paid;
            ext.CurrentIncome += paid;
            ext.ModifiedBy = this.LogonId;
            ext.ModifiedDate = DateTime.Now;

            using (DbContextTransaction trans=this.AppDb.Database.BeginTransaction())
            {
                try
                {
                    this.AppDb.Entry(order).State = EntityState.Modified;
                    this.AppDb.Entry(ext).State = isNew ? EntityState.Added : EntityState.Modified;
                    this.AppDb.Entry(fu).State = EntityState.Modified;

                    int effected = await this.AppDb.SaveChangesAsync();

                    if (effected > 0)
                    {
                        trans.Commit();
                        return this.Ok();
                    }
                    else
                    {
                        trans.Rollback();
                        return this.BadRequest("Fail to consign the order, try a later");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to consign the order, try a later. " + ex.Message);
                }
            }
        }

        // POST: api/Orders/Owner/Consign
        [Route("Driver/Orders/Consign")]
        public async Task<IHttpActionResult> Consign(DriverConsignBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.AppDb.Orders.FindAsync(model.OrderId);

            int code = order.ConsignCode;
            if (code != model.ConsignCode)
                return this.BadRequest("Invalid Consign Code");

            order.State = OrderState.Consigned;
            order.ModifiedDate = DateTime.Now;
            order.ModifiedBy = this.LogonId;

            FreightUnit fu = order.FreightUnit;
            fu.State = FreightUnitState.Ready;
            fu.ModifiedDate = DateTime.Now;
            fu.ModifiedBy = this.LogonId;

            DriverExt ext = this.AppDb.DriverExts.FirstOrDefault(de => de.Driver.Id == order.FreightUnit.Driver.Id);
            bool isNew = false;
            decimal paid = order.Paid ?? 0;

            if (ext == null)
            {
                ext = new DriverExt()
                {
                    CurrentIncome = 0,
                    TotalIncome = 0,
                    Driver = order.FreightUnit.Driver
                };
                isNew = true;
            }

            ext.TotalIncome += paid;
            ext.CurrentIncome += paid;
            ext.ModifiedBy = this.LogonId;
            ext.ModifiedDate = DateTime.Now;

            using (DbContextTransaction trans = this.AppDb.Database.BeginTransaction())
            {
                try
                {
                    this.AppDb.Entry(order).State = EntityState.Modified;
                    this.AppDb.Entry(ext).State = isNew ? EntityState.Added : EntityState.Modified;
                    this.AppDb.Entry(fu).State = EntityState.Modified;

                    int effected = await this.AppDb.SaveChangesAsync();

                    if (effected > 0)
                    {
                        trans.Commit();
                        return this.Ok();
                    }
                    else
                    {
                        trans.Rollback();
                        return this.BadRequest("Fail to consign the order, try a later");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to consign the order, try a later. " + ex.Message);
                }
            }
        }
        #endregion

        #region ConfirmDeal

        [Route("Driver/Orders/ConfirmDeal")]
        public async Task<IHttpActionResult> ConfirmDeal(ConfirmDealBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            int acceptedId = model.AcceptedId;
            //IList<int> rejectedIds = model.RejectedIds;

            // Order accepted = await this.AppDb.Orders.FindAsync(acceptedId);

            IList<Order> dealings = await this.AppDb.Orders.Where(o => o.State == OrderState.Dealing && o.FreightUnit.Driver.Id == this.LogonId).ToListAsync();

            using (DbContextTransaction trans = this.AppDb.Database.BeginTransaction())
            {
                try
                {
                    foreach (var order in dealings)
                    {
                        if (order.Id == acceptedId)
                        {
                            order.State = OrderState.Dealt;
                            order.FreightCost = model.FreightCost;
                        }
                        else
                        {
                            order.State = OrderState.Rejected;
                            var current = order.FreightUnit;
                            order.FreightUnit = null;
                            this.AppDb.Entry(current).State = EntityState.Modified;

                        }

                        order.ModifiedDate = DateTime.Now;
                        order.ModifiedBy = this.LogonId;
                        this.AppDb.Entry(order).State = EntityState.Modified;
                    }
                   int effected= await this.AppDb.SaveChangesAsync();
                   if (effected > 0)
                   {
                       trans.Commit();
                       return this.Ok();
                   }
                   else
                   {
                       trans.Rollback();
                       return this.BadRequest("Fail to confirm the deal.");
                   }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to confirm the deal. " + ex.Message);
                }
            }
        }

        #endregion

        #region UpdateOrderState

        [Route("Driver/Orders/UpdateOrderState")]
        public async Task<IHttpActionResult> UpdateOrderState(UpdateOrderStateBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            int orderId = model.OrderId;
            Order order = await this.AppDb.Orders.FindAsync(orderId);
            OrderState oState = order.State;
            string location = model.Location;

            if (oState != OrderState.Paid && oState != OrderState.InProgress)
                return this.Ok();
            else
            {
                FreightUnit fu = order.FreightUnit;

                if (oState == OrderState.Paid)
                    order.State = OrderState.InProgress;
                else if (oState == OrderState.InProgress)
                    order.State = OrderState.Arrived;

                fu.Location = location;
                fu.ModifiedDate=DateTime.Now;
                fu.ModifiedBy = this.LogonId;

                order.ModifiedBy = this.LogonId;
                order.ModifiedDate = DateTime.Now;

                using (DbContextTransaction trans=this.AppDb.Database.BeginTransaction())
                {
                    try
                    {
                        this.AppDb.Entry(order).State = EntityState.Modified;
                        this.AppDb.Entry(fu).State = EntityState.Modified;
                        int effected = await this.AppDb.SaveChangesAsync();
                        
                        if (effected>0)
                        {
                            trans.Commit();
                            return this.Ok();
                        }
                        else
                        {
                            trans.Rollback();
                            return this.BadRequest("Fail to Change order state");
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return this.BadRequest("Fail to Change order state. "+ex.Message);
                    }
                }

            }
        }

        #endregion
    }
}