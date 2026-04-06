using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Domain
{
    public class BodegaEntity
    {
        public Guid BodegaId { get; set; }
        public string Name { get; private set; } = string.Empty;
        public Guid ProductId { get; set; }

        public ProductEntity Product { get; set; }

    }
}
