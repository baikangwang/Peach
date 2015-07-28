using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peach.YouHu.API.Test.ContollerTest
{
    using System.Web.Http.Results;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Peach.YouHu.API.Test.Controller;

    using Peah.YouHu.API.Models;
    [TestClass]
    public class EvaluationControllerTest
    {
        [TestMethod]
        [TestCategory("Step 9 - Owner")]
        public void OwnerEvaluateTest()
        {
            using (UTOEvaluationsController controller = new UTOEvaluationsController())
            {
                var model = new EvaluateBindingModel()
                            {
                                Comments = "好就一个字",
                                OrderId = 3,//2,
                                Rank = 9
                            };

                var result = controller.OwnerEvaluate(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Step 9 - Driver")]
        public void DriverEvaluateTest()
        {
            using (UTDEvaluationsController controller = new UTDEvaluationsController())
            {
                var model = new EvaluateBindingModel()
                {
                    Comments = "好人一生平安",
                    OrderId = 3,//2,
                    Rank = 9
                };

                var result = controller.DriverEvaluate(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }
    }
}
