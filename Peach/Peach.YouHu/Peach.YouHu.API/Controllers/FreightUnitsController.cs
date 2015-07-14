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
    [RoutePrefix("api/FreightUnits")]
    public class FreightUnitsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FreightUnits
        public IQueryable<FreightUnit> GetFreightUnits()
        {
            return db.FreightUnits;
        }

        // GET: api/FreightUnits/5
        [ResponseType(typeof(FreightUnit))]
        public async Task<IHttpActionResult> GetFreightUnit(int id)
        {
            FreightUnit freightUnit = await db.FreightUnits.FindAsync(id);
            if (freightUnit == null)
            {
                return NotFound();
            }

            return Ok(freightUnit);
        }

        // PUT: api/FreightUnits/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFreightUnit(int id, FreightUnit freightUnit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != freightUnit.Id)
            {
                return BadRequest();
            }

            db.Entry(freightUnit).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FreightUnitExists(id))
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

        // POST: api/FreightUnits
        [ResponseType(typeof(FreightUnit))]
        public async Task<IHttpActionResult> PostFreightUnit(FreightUnit freightUnit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FreightUnits.Add(freightUnit);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = freightUnit.Id }, freightUnit);
        }

        // DELETE: api/FreightUnits/5
        [ResponseType(typeof(FreightUnit))]
        public async Task<IHttpActionResult> DeleteFreightUnit(int id)
        {
            FreightUnit freightUnit = await db.FreightUnits.FindAsync(id);
            if (freightUnit == null)
            {
                return NotFound();
            }

            db.FreightUnits.Remove(freightUnit);
            await db.SaveChangesAsync();

            return Ok(freightUnit);
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
                .Select(u=>new FreightUnitViewModel(u.Id,u.Driver.Rank??0,u.Driver.Name,u.Location))
                .OrderByDescending(v=>v.Rank)
                .ToListAsync();

            view = view.Select(v=>
            {
                v.Cost = FreightCostCalculator.Default.Calculate(v.Location, order.Destination, order.Size, order.Weight);
                return v;
            }).ToList();

            return this.Ok(view);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FreightUnitExists(int id)
        {
            return db.FreightUnits.Count(e => e.Id == id) > 0;
        }
    }
}