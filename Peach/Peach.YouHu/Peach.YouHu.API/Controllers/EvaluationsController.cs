namespace Peah.YouHu.API.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Peah.YouHu.API.Models;
    using Peah.YouHu.API.Models.Enum;

    [Authorize]
    [RoutePrefix("api")]
    public class EvaluationsController : BaseController
    {
        [Route("Owner/Evaluations/Evaluate")]
        public async Task<IHttpActionResult> OwnerEvaluate(EvaluateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.OwnerDb.Orders.FindAsync(model.OrderId);

            Driver driver = await this.OwnerDb.Drivers.FindAsync(model.Id);

            driver.Rank += model.Rank;

            Evaluation evaluation = new Evaluation();
            evaluation.Order = order;
            evaluation.Comments = model.Comments;
            evaluation.From = EvaluationFrom.Owner;
            evaluation.ModifiedBy = this.Logon.Id;
            evaluation.ModifiedDate = DateTime.Now;
            evaluation.Rank = model.Rank;

            order.OwnerEvaluation = evaluation;

            using (DbContextTransaction trans = this.OwnerDb.Database.BeginTransaction())
            {
                try
                {
                    this.OwnerDb.Entry(order).State = EntityState.Modified;

                    this.OwnerDb.Entry(evaluation).State = EntityState.Added;

                    await this.OwnerDb.SaveChangesAsync();

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

        [Route("Driver/Evaluations/Evaluate")]
        public async Task<IHttpActionResult> DriverEvaluate(EvaluateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.DriverDb.Orders.FindAsync(model.OrderId);

            Owner owner = await this.DriverDb.Owners.FindAsync(model.Id);
            
            owner.Rank += model.Rank;

            Evaluation evaluation = new Evaluation();
            evaluation.Order = order;
            evaluation.Comments = model.Comments;
            evaluation.From = EvaluationFrom.Owner;
            evaluation.ModifiedBy = this.Logon.Id;
            evaluation.ModifiedDate = DateTime.Now;
            evaluation.Rank = model.Rank;

            order.DriverEvaluation = evaluation;

            using (DbContextTransaction trans = this.DriverDb.Database.BeginTransaction())
            {
                try
                {
                    this.DriverDb.Entry(order).State = EntityState.Modified;

                    this.DriverDb.Entry(evaluation).State = EntityState.Added;

                    await this.DriverDb.SaveChangesAsync();

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
    }
}