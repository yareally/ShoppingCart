using System.Collections.Generic;
using System.Linq;

using Moq;

using NUnit.Framework;

using ShoppingCart.Controllers;
using ShoppingCart.Services;

namespace ShoppingCart.Test {
    public class CartControllerTest {
        private CartController controller;
        private Mock<IPaymentService> paymentServiceMock;
        private Mock<ICartService> cartServiceMock;

        private Mock<IShipmentService> shipmentServiceMock;
        private Mock<ICard> cardMock;
        private Mock<IAddressInfo> addressInfoMock;
        private List<CartItem> items;

        [SetUp]
        public void Setup() { }

        [Test]
        public void CheckOutTest() {
            // arrange
            paymentServiceMock.Setup(p => 
                p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(true);

            // act
            string result = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

            // assert
            shipmentServiceMock.Verify(s => 
                s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Once());

            Assert.AreEqual("charged", result);
        }
    }
}