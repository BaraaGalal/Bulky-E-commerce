﻿using E_Commerce.Domain.Models;

namespace E_Commerce.Domain.ViewModels
{
    public class ShoppingCartVM
    {

        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public double OrderHeader { get; set; }
    }
}
