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
            order.ModifiedBy = this.Logon.Id;
            this.AppDb.Orders.Add(order);
            this.AppDb.Entry(order).State = EntityState.Added;

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

        #region Owner/List
        // GET: api/Orders/Owner/Orders
        [Route("Owner/Orders/List")]
        [ResponseType(typeof(IList<OwnerOrderViewModel>))]
        public async Task<IHttpActionResult> OwnerOrders(string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IList<OwnerOrderViewModel> view = await this.AppDb.Orders
                .Include(o=>o.Owner)
                .Include(o=>o.FreightUnit)
                .Include(o=>o.FreightUnit.Driver)
                .Where(o => o.State != OrderState.Consigned && o.Owner.Id==id)
                .Select(o=>new OwnerOrderViewModel(o.Id,o.FreightUnit.Driver.FullName,o.Destination,o.Description,o.PublishedDate,o.State))
                .ToListAsync();
            return this.Ok(view);
        }
        #endregion

        #region Driver/List
        [Route("Driver/Orders/List")]
        [ResponseType(typeof(IList<DriverOrderViewModel>))]
        public async Task<IHttpActionResult> DriverOrders(string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IList<DriverOrderViewModel> view = await this.AppDb.Orders
                .Include(o => o.Owner)
                .Include(o => o.FreightUnit)
                .Include(o => o.FreightUnit.Driver)
                .Where(o => o.State != OrderState.Consigned && o.FreightUnit.Driver.Id == id)
                .Select(o => new DriverOrderViewModel(o.Id, o.Owner.FullName,o.Source, o.Destination, o.Description, o.PublishedDate, o.State))
                .ToListAsync();
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
            order.ModifiedBy = this.Logon.Id;
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
        [ResponseType(typeof(bool))]
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
            order.ModifiedBy = this.Logon.Id;
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
                order.ModifiedBy = this.Logon.Id;
                order.Paid = model.Paid;
                this.AppDb.Entry(order).State = EntityState.Modified;
                await this.AppDb.SaveChangesAsync();
            }
            else
                return this.BadRequest("Fail to pay, try a later");

            return this.Ok(true);
        }
        #endregion

        #region Consign
        // POST: api/Orders/Owner/Consign
        [Route("Owner/Orders/Consign")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> Consign(ConsignBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.AppDb.Orders.FindAsync(model.OrderId);
            
            string paymentCode = order.Owner.PaymentCode;
            if (paymentCode != model.PaymentCode)
                return this.BadRequest("Invalid Payment Code");

            order.State = OrderState.Consigned;
            order.ModifiedDate = DateTime.Now;
            order.ModifiedBy = this.Logon.Id;

            FreightUnit fu = order.FreightUnit;
            fu.State = FreightUnitState.Ready;
            fu.ModifiedDate = DateTime.Now;
            fu.ModifiedBy = this.Logon.Id;

            DriverExt ext = this.AppDb.DriverExts.FirstOrDefault(de => de.Driver.Id == order.FreightUnit.Driver.Id);
            decimal paid = order.Paid ?? 0;
            ext.TotalIncome += paid;
            ext.CurrentIncome += paid;
            ext.ModifiedBy = this.Logon.Id;
            ext.ModifiedDate = DateTime.Now;

            using (DbContextTransaction trans=this.AppDb.Database.BeginTransaction())
            {
                try
                {
                    this.AppDb.Entry(order).State = EntityState.Modified;
                    this.AppDb.Entry(ext).State = EntityState.Modified;
                    this.AppDb.Entry(fu).State = EntityState.Modified;

                    await this.AppDb.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to consign the order, try a later");
                }
            }

            return this.Ok(true);
        }
        #endregion

        #region ConfirmDeal

        [Route("Driver/Orders/ConfirmDeal")]
        public async Task<IHttpActionResult> ConfirmDeal(ConfirmDealBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            int acceptedId = model.AcceptedId;
            IList<int> rejectedIds = model.RejectedIds;

            Order accepted = await this.AppDb.Orders.FindAsync(acceptedId);

            IList<Order> rejecteds = await Task.Run(() => rejectedIds.Select(r => this.AppDb.Orders.Find(r)).ToList());

            accepted.State=OrderState.Dealt;
            accepted.ModifiedDate=DateTime.Now;
            accepted.ModifiedBy = this.Logon.Id;
            this.AppDb.Entry(accepted).State = EntityState.Modified;

            foreach (Order rej in rejecteds)
            {
                rej.State = OrderState.Rejected;
                rej.ModifiedDate = DateTime.Now;
                rej.ModifiedBy = this.Logon.Id;
                this.AppDb.Entry(rej).State = EntityState.Modified;
            }

            using (DbContextTransaction trans = this.AppDb.Database.BeginTransaction())
            {
                try
                {
                    await this.AppDb.SaveChangesAsync();
                   trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to confirm the deal");
                }
            }

            return this.Ok();
        }

        #endregion

        #region UpdateOrderState

        [Route("Driver/Orders/UpdateOrderState")]
        public async Task<IHttpActionResult> UpdateOrderState(UpdateOrderStateBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            int orderId = model.OrderId;
            OrderState oState = model.State;
            string location = model.Location;

            if (oState != OrderState.Paid && oState != OrderState.InProgress)
                return this.Ok();
            else
            {
                Order order = await this.AppDb.Orders.FindAsync(orderId);
                FreightUnit fu = order.FreightUnit;

                if (oState == OrderState.Paid)
                    order.State = OrderState.InProgress;
                else if (oState == OrderState.InProgress)
                    order.State = OrderState.Arrived;

                fu.Location = location;
                fu.ModifiedDate=DateTime.Now;
                fu.ModifiedBy = this.Logon.Id;

                order.ModifiedBy = this.Logon.Id;
                order.ModifiedDate = DateTime.Now;

                using (DbContextTransaction trans=this.AppDb.Database.BeginTransaction())
                {
                    try
                    {
                        this.AppDb.Entry(order).State = EntityState.Modified;
                        this.AppDb.Entry(fu).State = EntityState.Modified;
                        await this.AppDb.SaveChangesAsync();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return this.BadRequest("Fail to Change order state");
                    }
                }

            }

            return this.Ok();
        }

        #endregion
    }
}