using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Orders
{
    public class AuthorizationResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Folio { get; set; }
        public List<MissingProductDto> MissingProducts { get; set; } = new List<MissingProductDto>();
    }
}
