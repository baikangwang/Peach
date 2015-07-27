using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peach.YouHu.API.Test.Controller
{
    using System.Web.Http.Results;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Peah.YouHu.API.Controllers;
    using Peah.YouHu.API.Models;

    [TestClass]
    public class OrdersControllerTest
    {
        [TestMethod]
        [TestCategory("Owner.Order")]
        public void PublishTest()
        {
            using (OrdersController controller = new UTOwnerOrdersController())
            {
                var model = new PublishBindingModel()
                            {
                                Description =
                                    string.Format("a cup of water bought at {0:yyyy-MM-dd hh:mm:ss}", DateTime.Now),
                                Destination = "Tianjin",
                                Size = 1.0m,
                                Source = "Beijing",
                                Weight = 2.0m
                            };
                var result = controller.Publish(model).Result;

                Assert.IsTrue(result is OkResult);

                var view = controller.OwnerOrders().Result as OkNegotiatedContentResult<IList<OwnerOrderViewModel>>;

                Assert.IsNotNull(view);

                Assert.IsNotNull(view.Content);

                var list = view.Content;
                Assert.IsTrue(list.Any(l => l.Description == model.Description));

            }
        }

        [TestMethod]
        [TestCategory("Owner.Order")]
        public void ListTest()
        {
            using (OrdersController controller = new UTOwnerOrdersController())
            {
                var result = controller.OwnerOrders().Result as OkNegotiatedContentResult<IList<OwnerOrderViewModel>>;

                Assert.IsNotNull(result);

                Assert.IsNotNull(result.Content);

                Assert.IsTrue(result.Content.Count>0);
            }
        }

        [TestMethod]
        [TestCategory("Owner.Order")]
        public void MakeDealTest()
        {
            using (OrdersController controller = new UTOwnerOrdersController())
            {
                var model = new MakeDealBindingModel()
                            {
                                FreightUnitId = 2,
                                OrderId = 2
                            };

                var result = controller.MakeDeal(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Owner.Order")]
        public void PayTest()
        {
            using (OrdersController controller = new UTOwnerOrdersController())
            {
                var model = new PayBdingModel()
                {
                    FreightCost = 6000,
                    OrderId = 2,
                    Paid = 6000,
                    PaymentCode = "123456"
                };

                var result = controller.Pay(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Owner.Order")]
        public void ConsignTest()
        {
            using (OrdersController controller = new UTOwnerOrdersController())
            {
                var model = new ConsignBindingModel()
                {
                    OrderId = 2,
                    PaymentCode = "123456"
                };

                var result = controller.Consign(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }
    }
}
