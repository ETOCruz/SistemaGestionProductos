using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Producs
{
    public class CreateProductDto
    {
        public string Barcode { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public string Name { get; set; } = string.Empty;
        public int ProductQuantity { get; set; } = 0;
        public string ProductDescription { get; set; } = string.Empty;

    }
}
