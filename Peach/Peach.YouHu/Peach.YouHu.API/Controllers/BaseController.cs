namespace Peah.YouHu.API.Controllers
{
    using System.Web.Http;

    using Peah.YouHu.API.Models;

    public class BaseController : ApiController
    {
        #region DbContext
        private OwnerDbContext _odb;

        private DriverDbContext _ddb;

        protected virtual OwnerDbContext OwnerDb
        {
            get
            {
                return this._odb ?? (this._odb = new OwnerDbContext());
            }
        }

        protected virtual DriverDbContext DriverDb
        {
            get
            {
                return this._ddb ?? (this._ddb = new DriverDbContext());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._ddb != null)
                    this._ddb.Dispose();
                if (this._odb != null)
                    this._odb.Dispose();
            }
            base.Dispose(disposing);
        }

        protected virtual User Logon
        {
            get
            {
                return this.User as User;
            }
        }

        #endregion
    }
}
