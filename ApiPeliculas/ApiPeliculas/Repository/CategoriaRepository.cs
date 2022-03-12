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
            throw new NotImplementedException();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            throw new NotImplementedException();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            throw new NotImplementedException();
        }

        public bool ExisteCategoria(string nombre)
        {
            throw new NotImplementedException();
        }

        public bool ExisteCategoria(int categoriaId)
        {
            throw new NotImplementedException();
        }

        public Categoria GetCategoria(int categoriaId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Categoria> GetCategorias()
        {
            throw new NotImplementedException();
        }

        public bool Guardar()
        {
            throw new NotImplementedException();
        }
    }
}
