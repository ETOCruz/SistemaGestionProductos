using System;
using System.Collections.Generic;

namespace Domain
{
    public class WarehouseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public WarehouseEntity(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        // Para EF Core
        private WarehouseEntity() { }
    }
}
