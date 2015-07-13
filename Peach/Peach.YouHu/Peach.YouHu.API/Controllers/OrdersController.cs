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

            return Ok(order);
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
        public async Task<IList<OwnerOrderViewModel>> List()
        {
            IList<OwnerOrderViewModel> view = await this._db.Orders.Where(o => o.State != OrderState.Consigned)
                .Select(o=>new OwnerOrderViewModel(o.Id,o.FreightUnit.Driver.Name,o.Destination,o.Description,o.PublishedDate,o.State))
                .ToListAsync();

            return view;
        }

        // POST: api/Orders/Owner/MakeDeal
        [Route("Owner/MakeDeal")]
        public async Task<IHttpActionResult> MakeDeal(MakeDealBindingModel model)
        {
            Order order = await this._db.Orders.FindAsync(model.OrderId);
            FreightUnit fu = await this._db.FreightUnits.FindAsync(model.FreightUnitId);
            order.FreightUnit = fu;
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