using System;
using System.Collections.Generic;

namespace Application.DTOs.Orders
{
    public class ValidateStockRequestDto
    {
        public List<ValidateStockItemDto> Items { get; set; } = new List<ValidateStockItemDto>();
    }

    public class ValidateStockItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class ValidateStockResponseDto
    {
        public bool IsAvailable { get; set; }
        public List<ValidateStockDetailDto> Details { get; set; } = new List<ValidateStockDetailDto>();
    }

    public class ValidateStockDetailDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int RequestedQuantity { get; set; }
        public int AvailableStock { get; set; }
        public bool IsShortage { get; set; }
    }
}
