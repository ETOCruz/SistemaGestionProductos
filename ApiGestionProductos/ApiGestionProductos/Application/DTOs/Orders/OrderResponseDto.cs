using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Orders
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public string? Folio { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderDetailResponseDto> Details { get; set; } = new List<OrderDetailResponseDto>();
    }
}
