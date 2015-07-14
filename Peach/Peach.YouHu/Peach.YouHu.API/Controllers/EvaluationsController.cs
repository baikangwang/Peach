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

    [RoutePrefix("api/Evaluations")]
    public class EvaluationsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Evaluations
        public IQueryable<Evaluation> GetEvaluations()
        {
            return db.Evaluations;
        }

        // GET: api/Evaluations/5
        [ResponseType(typeof(Evaluation))]
        public async Task<IHttpActionResult> GetEvaluation(int id)
        {
            Evaluation evaluation = await db.Evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return Ok(evaluation);
        }

        // PUT: api/Evaluations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvaluation(int id, Evaluation evaluation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != evaluation.Id)
            {
                return BadRequest();
            }

            db.Entry(evaluation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluationExists(id))
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

        // POST: api/Evaluations
        [ResponseType(typeof(Evaluation))]
        public async Task<IHttpActionResult> PostEvaluation(Evaluation evaluation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Evaluations.Add(evaluation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = evaluation.Id }, evaluation);
        }

        // DELETE: api/Evaluations/5
        [ResponseType(typeof(Evaluation))]
        public async Task<IHttpActionResult> DeleteEvaluation(int id)
        {
            Evaluation evaluation = await db.Evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }

            db.Evaluations.Remove(evaluation);
            await db.SaveChangesAsync();

            return Ok(evaluation);
        }

        [Route("Owner/Evaluate")]
        public async Task<IHttpActionResult> OwnerEvaluate(EvaluateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.db.Orders.FindAsync(model.OrderId);

            Driver driver = await this.db.Drivers.FindAsync(model.Id);

            driver.Rank += model.Rank;

            Evaluation evaluation = new Evaluation();
            evaluation.Order = order;
            evaluation.Comments = model.Comments;
            evaluation.From = EvaluationFrom.Owner;
            evaluation.ModifiedBy = 0;
            evaluation.ModifiedDate = DateTime.Now;
            evaluation.Rank = model.Rank;

            order.OwnerEvaluation = evaluation;

            using (DbContextTransaction trans= this.db.Database.BeginTransaction())
            {
                try
                {
                    this.db.Entry(order).State = EntityState.Modified;

                    this.db.Entry(evaluation).State = EntityState.Added;
                    
                    await this.db.SaveChangesAsync();

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to post comments");
                }
            }

            return this.Ok();
        }

        [Route("Driver/Evaluate")]
        public async Task<IHttpActionResult> DriverEvaluate(EvaluateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.db.Orders.FindAsync(model.OrderId);

            Owner owner = await this.db.Owners.FindAsync(model.Id);

            owner.Rank += model.Rank;

            Evaluation evaluation = new Evaluation();
            evaluation.Order = order;
            evaluation.Comments = model.Comments;
            evaluation.From = EvaluationFrom.Owner;
            evaluation.ModifiedBy = 0;
            evaluation.ModifiedDate = DateTime.Now;
            evaluation.Rank = model.Rank;

            order.DriverEvaluation = evaluation;

            using (DbContextTransaction trans = this.db.Database.BeginTransaction())
            {
                try
                {
                    this.db.Entry(order).State = EntityState.Modified;

                    this.db.Entry(evaluation).State = EntityState.Added;

                    await this.db.SaveChangesAsync();

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to post comments");
                }
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

        private bool EvaluationExists(int id)
        {
            return db.Evaluations.Count(e => e.Id == id) > 0;
        }
    }
}