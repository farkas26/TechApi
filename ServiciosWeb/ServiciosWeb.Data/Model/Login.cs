
namespace ServiciosWeb.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Login
    {
        public int id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string name { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo debe ser numerico")]
        public string number { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> birthdate { get; set; }
        [EmailAddress]
        [Required]
        public string email { get; set; }
        public string token { get; set; }
    }
}
