namespace Peah.YouHu.API.Controllers
{
    using System.Web.Http;

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

        protected virtual AppUser Logon
        {
            get
            {
                return this.User as AppUser;
            }
        }

        #endregion
    }
}
