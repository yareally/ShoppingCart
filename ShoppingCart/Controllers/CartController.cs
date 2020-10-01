using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Services;

namespace ShoppingCart.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CartController {
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;
        private readonly IShipmentService _shipmentService;

        public CartController(ICartService cartService, IPaymentService paymentService, IShipmentService shipmentService) {
            _cartService = cartService;
            _paymentService = paymentService;
            _shipmentService = shipmentService;
        }

        [HttpPost]
        public string CheckOut(ICard card, IAddressInfo addressInfo) {
            bool result = _paymentService.Charge(_cartService.Total(), card);
            
            if (result) {
                _shipmentService.Ship(addressInfo, _cartService.Items());
                return "charged";
            }
            return "not charged";
        }
    }
}