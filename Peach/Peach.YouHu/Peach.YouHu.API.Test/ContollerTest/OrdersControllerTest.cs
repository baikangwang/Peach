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
        [TestCategory("Step 2 - Owner")]
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
        [TestCategory("Step 2 - Owner")]
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
        [TestCategory("Step 4 - Owner")]
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
        [TestCategory("Step 6 - Owner")]
        public void PayTest()
        {
            using (OrdersController controller = new UTOwnerOrdersController())
            {
                var model = new PayBdingModel()
                {
                    FreightCost = 6000,
                    OrderId = 3,//2,
                    Paid = 6000,
                    PaymentCode = "123456"
                };

                var result = controller.Pay(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Step 8 - Owner")]
        public void ConsignTest()
        {
            using (OrdersController controller = new UTOwnerOrdersController())
            {
                var model = new ConsignBindingModel()
                {
                    OrderId = 3,//2,
                    PaymentCode = "123456"
                };

                var result = controller.Consign(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Step 5 - Driver")]
        public void ConfirmDealTest()
        {
            using (UTDriverOrderController controller=new UTDriverOrderController())
            {
                var model = new ConfirmDealBindingModel()
                          {
                              AcceptedId = 3,
                              FreightCost = 6000
                          };
                var result= controller.ConfirmDeal(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Step 5 - Driver")]
        public void DriverOrdersTest()
        {
            using (UTDriverOrderController controller = new UTDriverOrderController())
            {
                var result = controller.DriverOrders().Result as OkNegotiatedContentResult<IList<DriverOrderViewModel>>;

                Assert.IsNotNull(result);

                var view = result.Content;

                Assert.IsNotNull(view);

                Assert.IsTrue(view.Count>0);

                foreach (var model in view)
                {
                    Console.WriteLine(string.Format("desc: {7}, from {0} to {1}, state: {2}, published at {3:yyyy-MM-dd}, s-w: {4}-{5}, owner: {6}",
                        model.Source,model.Destination,model.State,model.PublishedDate,model.Size,model.Weight,model.OwnerName,model.Description));
                }
            }
        }

        [TestMethod]
        [TestCategory("Step 7 - Driver")]
        public void UpdateStateToInprogressTest()
        {
            using (UTDriverOrderController controller = new UTDriverOrderController())
            {
                var model=new UpdateOrderStateBindingModel()
                          {
                              Location = "Jixian",
                              OrderId = 3
                          };

                var result = controller.UpdateOrderState(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }

        [TestMethod]
        [TestCategory("Step 7 - Driver")]
        public void UpdateStateToArrivedTest()
        {
            using (UTDriverOrderController controller = new UTDriverOrderController())
            {
                var model = new UpdateOrderStateBindingModel()
                {
                    Location = "Tianjin",
                    OrderId = 3
                };

                var result = controller.UpdateOrderState(model).Result;

                Assert.IsTrue(result is OkResult);
            }
        }
    }
}
