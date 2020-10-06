using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Services;

namespace ShoppingCart.Models
{
    public class Card : ICard
    {

        public Card(string cardNumber, string name, DateTime validTo) {
            CardNumber = cardNumber;
            Name = name;
            ValidTo = validTo;
        }

        /// <inheritdoc />
        public string CardNumber { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public DateTime ValidTo { get; set; }
    }
}
