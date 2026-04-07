namespace Application.DTOs.Orders
{
    public class OrderDetailResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantityOrdered { get; set; }
        public int QuantityScanned { get; set; }
    }
}