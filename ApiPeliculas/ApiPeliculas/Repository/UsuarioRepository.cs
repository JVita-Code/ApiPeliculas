using ApiPeliculas.Data;
using ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository.IRepository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _baseDeDatos;

        public UsuarioRepository(ApplicationDbContext baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }

        public bool ExisteUsuario(string usuario)
        {
            if (_baseDeDatos.Usuarios.Any(u => u.UsuarioAcceso == usuario))
            {
                return true;
            }

            return false;
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _baseDeDatos.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _baseDeDatos.Usuarios.OrderBy(u => u.UsuarioAcceso).ToList();
        }

        public bool Guardar()
        {
            return _baseDeDatos.SaveChanges() >= 0 ? true : false;
        }

        public Usuario Login(string usuario, string password)
        {
            var user = _baseDeDatos.Usuarios.FirstOrDefault(u => u.UsuarioAcceso == usuario);

            if (user == null)
            {
                return null;
            }

            if (!VerificaPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }        

        public Usuario Registro(Usuario usuario, string password)
        {
            byte[] passwordHash, passwordSalt;

            CrearPasswordHash(password, out passwordHash, out passwordSalt);

            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _baseDeDatos.Usuarios.Add(usuario);

            Guardar();

            return usuario;
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {              
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerificaPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;                    
                }
            }
            return true;            
        }
    }
}
