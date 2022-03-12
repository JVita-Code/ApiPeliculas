using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _baseDeDatos;

        public CategoriaRepository(ApplicationDbContext baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }


        public bool ActualizarCategoria(Categoria categoria)
        {
            _baseDeDatos.Categorias.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _baseDeDatos.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            _baseDeDatos.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _baseDeDatos.Categorias
                        .Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());

            return valor;
        }

        public bool ExisteCategoria(int categoriaId)
        {
            return _baseDeDatos.Categorias
                        .Any(c => c.Id == categoriaId);            
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _baseDeDatos.Categorias.FirstOrDefault(c => c.Id == categoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _baseDeDatos.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _baseDeDatos.SaveChanges() >= 0 ? true : false;
        }
    }
}
