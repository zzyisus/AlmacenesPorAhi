using System.Security.Cryptography;
using System.Text;
using AlmacenesPorAhi.Models;

namespace AlmacenesPorAhi.Services
{
    public class AuthService
    {
        private static AuthService _instance;
        public static AuthService Instance => _instance ??= new AuthService();

        private readonly List<Usuario> _users = new();
        public Usuario UsuarioActual { get; private set; }
        public bool EstaAutenticado => UsuarioActual != null;

        private AuthService()
        {
            _users.Add(new Usuario
            {
                UsuarioNombre = "admin",
                Password = HashPassword("1234"),
                NombreCompleto = "Administrador",
                Rol = "Admin"
            });
            _users.Add(new Usuario
            {
                UsuarioNombre = "empleado",
                Password = HashPassword("1234"),
                NombreCompleto = "Juan Pérez",
                Rol = "Empleado"
            });
        }

        public bool Login(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                return false;

            var hashed = HashPassword(password);
            var user = _users.FirstOrDefault(u =>
                u.UsuarioNombre.Equals(usuario, StringComparison.OrdinalIgnoreCase)
                && u.Password == hashed);

            if (user != null)
            {
                UsuarioActual = user;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            UsuarioActual = null;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
