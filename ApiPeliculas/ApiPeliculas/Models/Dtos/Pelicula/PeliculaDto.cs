using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ApiPeliculas.Models.Pelicula;

namespace ApiPeliculas.Models.Dtos
{
    public class PeliculaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La RutaImagen es obligatoria.")]
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "La Descripcion es obligatoria.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La Duracion es obligatoria.")]
        public string Duracion { get; set; }        
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }        
        public Categoria Categoria { get; set; }
    }
}
