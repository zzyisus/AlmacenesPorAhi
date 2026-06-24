using System;
using System.Collections.Generic;
using System.Text;

namespace AlmacenesPorAhi.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Categoria { get; set; }
        public int StockMinimo { get; set; } = 5;

        // Computed helper
        public bool StockBajo => Stock <= StockMinimo;
        public string StockEstado => Stock == 0 ? "Sin stock" : StockBajo ? "Stock bajo" : "En stock";
        public string StockColor => Stock == 0 ? "#DC2626" : StockBajo ? "#D97706" : "#16A34A";
        public string StockBgColor => Stock == 0 ? "#FEF2F2" : StockBajo ? "#FFFBEB" : "#F0FDF4";
    }
}
