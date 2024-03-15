using System;
using System.Collections.Generic;

namespace Lisseth.Models
{
    public partial class Telefono
    {
        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? Compania { get; set; }
        public string? Tipo { get; set; }

        public virtual Cliente? IdClienteNavigation { get; set; }
    }
}
