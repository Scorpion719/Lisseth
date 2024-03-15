using System;
using System.Collections.Generic;

namespace Lisseth.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Telefono = new List<Telefono>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Correo { get; set; }

        public virtual IList<Telefono> Telefono { get; set; }

    }
}
