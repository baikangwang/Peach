using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peach.YouHu.API.Test.Controller
{
    using Peah.YouHu.API.Controllers;
    using Peah.YouHu.API.Models;

    public class UTOEvaluationsController:EvaluationsController
    {
        private AppUser _logUser;
        public override AppUser Logon
        {
            get
            {
                return this._logUser ?? (this._logUser = this.AppDb.Users.Find(this.LogonId));
            }
        }

        protected override string LogonId
        {
            get
            {
                return "5280253e-3a6b-4869-9d22-ff9a3120ac94";
            }
        }
    }

    public class UTDEvaluationsController : UTOEvaluationsController
    {
        protected override string LogonId
        {
            get
            {
                return "f36e0586-a8b9-4f15-a500-b61ea6c3b3e8";
            }
        }
    }
}
