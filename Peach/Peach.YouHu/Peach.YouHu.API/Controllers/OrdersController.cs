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
    using System.Collections;
    using System.Threading;

    using Peah.YouHu.API.Component;

    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return this._db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Order order = await this._db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            this._db.Entry(order).State = EntityState.Modified;

            try
            {
                await this._db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this._db.Orders.Add(order);
            await this._db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = await this._db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            this._db.Orders.Remove(order);
            await this._db.SaveChangesAsync();

            return this.Ok(order);
        }

        // POST: api/Orders/Publish
        [Route("Owner/Publish")]
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
            this._db.Orders.Add(order);
            this._db.Entry(order).State = EntityState.Added;

            try
            {
                int added = await this._db.SaveChangesAsync();

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

        // GET: api/Orders/Owner/Orders
        [Route("Owner/Orders")]
        [ResponseType(typeof(IList<OwnerOrderViewModel>))]
        public async Task<IHttpActionResult> OwnerOrders(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IList<OwnerOrderViewModel> view = await this._db.Orders
                .Include(o=>o.Owner)
                .Include(o=>o.FreightUnit)
                .Include(o=>o.FreightUnit.Driver)
                .Where(o => o.State != OrderState.Consigned && o.Owner.Id==id)
                .Select(o=>new OwnerOrderViewModel(o.Id,o.FreightUnit.Driver.Name,o.Destination,o.Description,o.PublishedDate,o.State))
                .ToListAsync();
            return this.Ok(view);
        }

        [Route("Driver/Orders")]
        [ResponseType(typeof(IList<DriverOrderViewModel>))]
        public async Task<IHttpActionResult> DriverOrders(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IList<DriverOrderViewModel> view = await this._db.Orders
                .Include(o => o.Owner)
                .Include(o => o.FreightUnit)
                .Include(o => o.FreightUnit.Driver)
                .Where(o => o.State != OrderState.Consigned && o.FreightUnit.Driver.Id == id)
                .Select(o => new DriverOrderViewModel(o.Id, o.Owner.Name,o.Source, o.Destination, o.Description, o.PublishedDate, o.State))
                .ToListAsync();
            return this.Ok(view);
        }

        // POST: api/Orders/Owner/MakeDeal
        [Route("Owner/MakeDeal")]
        public async Task<IHttpActionResult> MakeDeal(MakeDealBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            
            Order order = await this._db.Orders.FindAsync(model.OrderId);
            FreightUnit fu = await this._db.FreightUnits.FindAsync(model.FreightUnitId);
            order.FreightUnit = fu;
            order.ModifiedBy = model.ModifiedBy;
            order.ModifiedDate=DateTime.Now;
            order.State = OrderState.Dealing;
            this._db.Entry(order).State = EntityState.Modified;
            
            try
            {
                int added = await this._db.SaveChangesAsync();

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

        // POST: api/Orders/Owner/Pay
        [Route("Pay")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> Pay(PayBdingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this._db.Orders.FindAsync(model.OrderId);
            string paymentCode = order.Owner.PaymentCode;
            if (paymentCode != model.PaymentCode)
                return this.BadRequest("Invalid Payment Code");

            order.State = OrderState.Paying;
            order.ModifiedDate=DateTime.Now;
            order.ModifiedBy = model.ModifiedBy;
            this._db.Entry(order).State = EntityState.Modified;

            try
            {
                await this._db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return this.BadRequest("Fail to update order state to Paying");
            }

            bool paid = await PaymentPlatment.Default.Pay(model.Paid);

            if (paid)
            {
                order.State = OrderState.Paid;
                order.ModifiedDate = DateTime.Now;
                order.ModifiedBy = model.ModifiedBy;
                order.Paid = model.Paid;
                this._db.Entry(order).State = EntityState.Modified;
                await this._db.SaveChangesAsync();
            }
            else
                return this.BadRequest("Fail to pay, try a later");

            return this.Ok(true);
        }

        // POST: api/Orders/Owner/Consign
        [Route("Consign")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> Consign(ConsignBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this._db.Orders.FindAsync(model.OrderId);
            
            string paymentCode = order.Owner.PaymentCode;
            if (paymentCode != model.PaymentCode)
                return this.BadRequest("Invalid Payment Code");

            order.State = OrderState.Consigned;
            order.ModifiedDate = DateTime.Now;
            order.ModifiedBy = model.ModifiedBy;

            Driver driver = order.FreightUnit.Driver;
            decimal paid = order.Paid ?? 0;
            driver.TotalIncome += paid;
            driver.CurrentIncome += paid;
            driver.ModifiedBy = 0;
            driver.ModifiedDate = DateTime.Now;

            using (DbContextTransaction trans=this._db.Database.BeginTransaction())
            {
                try
                {
                    this._db.Entry(order).State = EntityState.Modified;
                    this._db.Entry(driver).State = EntityState.Modified;

                    await this._db.SaveChangesAsync();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return this._db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}