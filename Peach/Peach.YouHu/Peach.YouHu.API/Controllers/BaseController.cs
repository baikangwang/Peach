namespace Peah.YouHu.API.Controllers
{
    using System.Web.Http;

    using Microsoft.AspNet.Identity;

    using Peah.YouHu.API.Models;

    public class BaseController : ApiController
    {
        #region DbContext
        private AppDbContext _odb;

        protected virtual AppDbContext AppDb
        {
            get
            {
                return this._odb ?? (this._odb = new AppDbContext());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._odb != null)
                    this._odb.Dispose();
            }
            base.Dispose(disposing);
        }

        private AppUser _logon;
        protected virtual AppUser Logon
        {
            get
            {
                return this._logon??(this._logon= this.AppDb.Users.Find(this.User.Identity.GetUserId()));
            }
        }

        private string _logonId;
        protected virtual string LogonId
        {
            get
            {
                return this._logonId ?? (this._logonId = this.User.Identity.GetUserId());
            }
        }

        #endregion
    }
}
