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

            Order order = await this.AppDb.Orders.FindAsync(model.OrderId);

            AppUser driver = order.FreightUnit.Driver;

            driver.Rank += model.Rank;

            Evaluation evaluation = new Evaluation();
            evaluation.Order = order;
            evaluation.Comments = model.Comments;
            evaluation.From = EvaluationFrom.Owner;
            evaluation.ModifiedBy = this.LogonId;
            evaluation.ModifiedDate = DateTime.Now;
            evaluation.Rank = model.Rank;

            order.OwnerEvaluation = evaluation;

            using (DbContextTransaction trans = this.AppDb.Database.BeginTransaction())
            {
                try
                {
                    this.AppDb.Entry(order).State = EntityState.Modified;
                    this.AppDb.Entry(driver).State=EntityState.Modified;
                    this.AppDb.Entry(evaluation).State = EntityState.Added;

                    int effected= await this.AppDb.SaveChangesAsync();

                    if(effected>0)
                    {
                        trans.Commit();
                        return this.Ok();
                    }
                    else
                    {
                        trans.Rollback();
                        return this.BadRequest("Fail to post comments");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to post comments. " + ex.Message);
                }
            }
        }

        [Route("Driver/Evaluations/Evaluate")]
        public async Task<IHttpActionResult> DriverEvaluate(EvaluateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Order order = await this.AppDb.Orders.FindAsync(model.OrderId);

            AppUser owner = order.Owner;
            
            owner.Rank += model.Rank;

            Evaluation evaluation = new Evaluation();
            evaluation.Order = order;
            evaluation.Comments = model.Comments;
            evaluation.From = EvaluationFrom.Driver;
            evaluation.ModifiedBy = this.LogonId;
            evaluation.ModifiedDate = DateTime.Now;
            evaluation.Rank = model.Rank;

            order.DriverEvaluation = evaluation;

            using (DbContextTransaction trans = this.AppDb.Database.BeginTransaction())
            {
                try
                {
                    this.AppDb.Entry(order).State = EntityState.Modified;

                    this.AppDb.Entry(evaluation).State = EntityState.Added;

                    int effected = await this.AppDb.SaveChangesAsync();

                     if (effected > 0)
                     {
                         trans.Commit();
                         return this.Ok();
                     }
                     else
                     {
                         trans.Rollback();
                         return this.BadRequest("Fail to post comments");
                     }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return this.BadRequest("Fail to post comments. " + ex.Message);
                }
            }
        }
    }
}