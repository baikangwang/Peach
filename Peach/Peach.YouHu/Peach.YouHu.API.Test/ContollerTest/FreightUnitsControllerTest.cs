using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Peach.YouHu.API.Test.Controller
{
    using System.Collections.Generic;
    using System.Web.Http.Results;

    using Peah.YouHu.API.Models;
    using Peah.YouHu.API.Models.Enum;

    [TestClass]
    public class FreightUnitsControllerTest
    {
        [TestMethod]
        [TestCategory("Step 1 - Driver")]
        public void PublishTest()
        {
            using (UTDFreightUnitController controller=new UTDFreightUnitController())
            {
                var model = new PublishFreightUnitBindingModel()
                            {
                                Id = 2,
                                Location = "Beijing"
                            };

                var result = controller.Publish(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Step 1 - Driver")]
        public void RegisterTest()
        {
            using (UTDFreightUnitController controller=new UTDFreightUnitController())
            {
                var model = new RegisterFreightUnitBindingModel()
                            {
                                Height = 3.0m,
                                Length = 4.0m,
                                Licence = string.Format("津{0:ffffff}",DateTime.Now),
                                Location = "Tianjin",
                                Type = FreightUnitType.Truck,
                                Weight = 20.0m
                            };
                
                var result= controller.Register(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Step 3 - Owner")]
        public void FindTest()
        {
            using (UTDFreightUnitController controller=new UTDFreightUnitController())
            {
                int id = 3;
                var result = controller.Find(id).Result as OkNegotiatedContentResult<IList<FreightUnitViewModel>>;
                Assert.IsNotNull(result);

                var view = result.Content;

                Assert.IsNotNull(view);

                Assert.IsTrue(view.Count > 0);
            }
        }
    }
}
