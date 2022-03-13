using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly ApplicationDbContext _baseDeDatos;

        public PeliculaRepository(ApplicationDbContext baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }


        public bool ActualizarPelicula(Pelicula pelicula)
        {
            _baseDeDatos.Peliculas.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _baseDeDatos.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _baseDeDatos.Peliculas;

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre)));
            }

            return query.ToList();
        }        

        public bool CrearPelicula(Pelicula pelicula)
        {
            throw new NotImplementedException();
        }

        public bool ExistePelicula(string nombre)
        {
            bool valor = _baseDeDatos.Peliculas
                        .Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());

            return valor;
        }

        public bool ExistePelicula(int peliculaId)
        {
            return _baseDeDatos.Peliculas
                        .Any(c => c.Id == peliculaId);            
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _baseDeDatos.Peliculas.FirstOrDefault(c => c.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _baseDeDatos.Peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _baseDeDatos.Peliculas.Include(ca => ca.Categoria).Where(ca => ca.Id == categoriaId).ToList();
        }

        public bool Guardar()
        {
            return _baseDeDatos.SaveChanges() >= 0 ? true : false;
        }
    }
}
