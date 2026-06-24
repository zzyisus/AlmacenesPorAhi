using System;
using System.Collections.Generic;
using System.Text;

namespace AlmacenesPorAhi.Models
{
    public class Usuario
    {
        public string UsuarioNombre { get; set; }
        public string Password { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; } = "Empleado"; // Admin | Empleado
    }
}
