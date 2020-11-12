
namespace ServiciosWeb.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Producto
    {
        public int id { get; set; }
        public string SKU { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string cantidad { get; set; }
        [Required]
        public decimal precio { get; set; }
        public string descripcion { get; set; }
        public byte[] imagen { get; set; }
    }
}
