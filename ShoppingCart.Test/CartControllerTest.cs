using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using NUnit.Framework;

using ShoppingCart.Controllers;
using ShoppingCart.Models;
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
        public void Setup() {
            cardMock = new Mock<ICard>();
            cartServiceMock = new Mock<ICartService>();
            paymentServiceMock = new Mock<IPaymentService>();
            shipmentServiceMock = new Mock<IShipmentService>();
            addressInfoMock = new Mock<IAddressInfo>();

            var cartItemMock = new Mock<CartItem>();
            cartItemMock.Setup(item => item.Price).Returns(10);

            items = new List<CartItem> {
                cartItemMock.Object
            };

            var itemPrice = cartItemMock.Object.Price;
            cartServiceMock.Setup(c => c.Items()).Returns(items.AsEnumerable());
            controller = new CartController(cartServiceMock.Object, paymentServiceMock.Object, shipmentServiceMock.Object);
        }

        [Test]
        public void CheckOutTest() {
            paymentServiceMock.Setup(p => 
                p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(true);

            // act
            string result = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

            // assert
            shipmentServiceMock.Verify(s => 
                s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Once());
             
            Assert.AreEqual("charged", result);
        }

        [Test]
        public void ShouldReturnNotCharged() {
            paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(false);

            // act
            var result = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

            // assert
            shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Never());
            Assert.AreEqual("not charged", result);
        }
    }
}