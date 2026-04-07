using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public Guid SellerId { get; set; }
        public List<CreateOrderDetailDto> Items { get; set; } = new List<CreateOrderDetailDto>();
    }
}
