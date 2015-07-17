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
    using Peah.YouHu.API.Component;

    public class DriversController : ApiController
    {
        private OwnerDbContext db = new OwnerDbContext();

        // GET: api/Drivers
        public IQueryable<Driver> GetDrivers()
        {
            return db.Drivers;
        }

        // GET: api/Drivers/5
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> GetDriver(string id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }

        // PUT: api/Drivers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDriver(string id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driver.Id)
            {
                return BadRequest();
            }

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        // POST: api/Drivers
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> PostDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = driver.Id }, driver);
        }

        // DELETE: api/Drivers/5
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> DeleteDriver(int id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            await db.SaveChangesAsync();

            return Ok(driver);
        }

        [Route("WithDraw")]
        public async Task<IHttpActionResult> WithDraw(WithDrawBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            int driverId = model.DriverId;
            int modifiedBy = model.DriverId;
            decimal amount = model.Amount;
            string paymentCode = model.PaymentCode;

            Driver driver = await this.db.Drivers.FindAsync(driverId);

            if (paymentCode != driver.PaymentCode)
                return this.BadRequest("Invalid Payment Code");

            if (driver.CurrentIncome < amount)
                return this.BadRequest("the amout withdrawe was larger then current income amount");

           bool withdrawed= await PaymentPlatment.Default.WithDraw(amount, driver.Name);
            if (withdrawed)
            {
                driver.CurrentIncome -= amount;
                driver.ModifiedDate=DateTime.Now;
                driver.ModifiedBy = modifiedBy;

                try
                {
                    this.db.Entry(driver).State=EntityState.Modified;
                    await this.db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return this.BadRequest("fail to update income amount");
                }
            }
            else
            {
                return this.BadRequest("Fail to transfer amount to the driver");
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

        private bool DriverExists(string id)
        {
            return db.Drivers.Count(e => e.Id == id) > 0;
        }
    }
}