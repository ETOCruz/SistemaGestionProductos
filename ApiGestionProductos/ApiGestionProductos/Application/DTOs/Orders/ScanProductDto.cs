using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Orders
{
    public class ScanProductDto
    {
        public Guid UserId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }
}
