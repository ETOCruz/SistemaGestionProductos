using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Barcode { get; private set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ProductQuantity { get; private set; } = 0;
        public string ProductDescription { get; private set; } = string.Empty;
        public decimal Price { get; private set; } = decimal.Zero;
        public string ProductFullDescription => $"{Name} - {ProductDescription}";

        public ProductEntity(string barcode, decimal price, string name, int productQuantity, string productDescription)
        {
            ValidateBarcode(barcode);
            ValidatePrice(price);
            ValidateProductName(name);
            ValidateProductDescription(productDescription);

            Id = Guid.NewGuid();
            Barcode = barcode.Trim().ToUpper();
            Price = price;
            Name = name.Trim();
            ProductQuantity = productQuantity;
            ProductDescription = productDescription;
        }

        public void UpdateProduct(string barcode, decimal price, string name, int productQuantity, string productDescription)
        {
            ValidateBarcode(barcode);
            ValidatePrice(price);
            ValidateProductName(name);
            ValidateProductDescription(productDescription);

            Barcode = barcode.Trim().ToUpper();
            Price = price;
            Name = name.Trim();
            ProductQuantity = productQuantity;
            ProductDescription = productDescription;
        }

        private void ValidateBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                throw new ArgumentNullException("El código no puede estar vacío.", nameof(barcode));
            }

            string barcodePattern = @"^\d{5,10}$";

            if (!Regex.IsMatch(barcode, barcodePattern))
                throw new ArgumentException("El formato del Barcode es inválido", nameof(barcode));

        }

        private void ValidateProductName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("El nombre no puede estar vacio", nameof(productName));

            if (productName.Trim().Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(productName));

            if (productName.Trim().Length > 50)
                throw new ArgumentException("El nombre no puede exceder 50 caracteres", nameof(productName));
        }

        private void ValidateProductDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("El nombre no puede estar vacio", nameof(description));

            if (description.Trim().Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(description));

            if (description.Trim().Length > 50)
                throw new ArgumentException("El nombre no puede exceder 50 caracteres", nameof(description));
        }

        private void ValidatePrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo", nameof(price));
        }

    }
}
