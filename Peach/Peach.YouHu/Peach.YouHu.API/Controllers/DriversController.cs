namespace Peah.YouHu.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;

    using Peah.YouHu.API.Component;
    using Peah.YouHu.API.Models;
    using Peah.YouHu.API.Providers;
    using Peah.YouHu.API.Results;

    [Authorize]
    [RoutePrefix("api")]
    public class DriversController : BaseController
    {
        [Route("Accounts/Driver/WithDraw")]
        public async Task<IHttpActionResult> WithDraw(WithDrawBindingModel model)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            decimal amount = model.Amount;
            string paymentCode = model.PaymentCode;

            AppUser driver = this.Logon;

            if (paymentCode != driver.PaymentCode)
                return this.BadRequest("Invalid Payment Code");

            DriverExt ext = this.AppDb.DriverExts.FirstOrDefault(de => de.Driver.Id == driver.Id);

            if (ext.CurrentIncome < amount)
                return this.BadRequest("the amout withdrawe was larger then current income amount");

           bool withdrawed= await PaymentPlatment.Default.WithDraw(amount, driver.FullName);
            if (withdrawed)
            {
                ext.CurrentIncome -= amount;
                ext.ModifiedDate = DateTime.Now;
                ext.ModifiedBy = driver.Id;

                try
                {
                    this.AppDb.Entry(ext).State = EntityState.Modified;
                    await this.AppDb.SaveChangesAsync();
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
    }
}