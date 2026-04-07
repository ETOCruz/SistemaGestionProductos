using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Orders
{
    public class MissingProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int RequestedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
