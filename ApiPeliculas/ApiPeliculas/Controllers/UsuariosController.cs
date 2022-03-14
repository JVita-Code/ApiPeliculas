using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Models.Dtos.Usuario;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiPeliculas.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuariosController(IUsuarioRepository usuarioRepo,
                                    IMapper mapper,
                                    IConfiguration config)
        {
            _usuarioRepo = usuarioRepo;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usuarioRepo.GetUsuarios();

            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var usuario in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(usuario));
            }

            return Ok(listaUsuariosDto);
        }


        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        public IActionResult GetUsuario(int usuarioId)
        {
            var user = _usuarioRepo.GetUsuario(usuarioId);

            if (user == null)
            {
                return NotFound();
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(user);

            return Ok(usuarioDto);
        }

        [HttpPost("Registro")]
        public IActionResult Registro(UsuarioAuthDto usuarioAuthDto)
        {
            usuarioAuthDto.Usuario = usuarioAuthDto.Usuario.ToLower();

            if (_usuarioRepo.ExisteUsuario(usuarioAuthDto.Usuario))
            {
                return BadRequest("El usuario ya existe");
            }

            var usuarioACrear = new Usuario
            {
                UsuarioAcceso = usuarioAuthDto.Usuario
            };

            var usuarioCreado = _usuarioRepo.Registro(usuarioACrear, usuarioAuthDto.Password);

            return Ok(usuarioCreado);
        }

        [HttpPost("Login")]
        public IActionResult Login(UsuarioAuthLoginDto usuarioAuthLoginDto)
        {
            var usuarioDelRepo = _usuarioRepo.Login(usuarioAuthLoginDto.Usuario, usuarioAuthLoginDto.Password);

            if (usuarioDelRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDelRepo.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuarioDelRepo.UsuarioAcceso.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
        //    [HttpPost]
        //    public IActionResult CrearCategoria([FromBody] CategoriaDto categoriaDto)
        //    {
        //        if (categoriaDto == null)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        if (_usuarioRepo.ExisteCategoria(categoriaDto.Nombre))
        //        {
        //            ModelState.AddModelError("", "La categoría ya existe.");

        //            return StatusCode(404, ModelState);
        //        }

        //        var categoria = _mapper.Map<Categoria>(categoriaDto);

        //        if (!_usuarioRepo.CrearCategoria(categoria))
        //        {
        //            ModelState.AddModelError("", $"Error al guardar el registro.{categoria.Nombre}");
        //            return StatusCode(404, ModelState);
        //        }

        //        return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        //    }

        //    [HttpPatch("{categoriaId:int}", Name = "ActualizarCategoria")]
        //    public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        //    {
        //        if (categoriaDto == null || categoriaId != categoriaDto.Id)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var categoria = _mapper.Map<Categoria>(categoriaDto);

        //        if (!_usuarioRepo.ActualizarCategoria(categoria))
        //        {
        //            ModelState.AddModelError("", $"Error al actualizar el registro.{categoria.Nombre}");
        //            return StatusCode(500, ModelState);
        //        }

        //        return NoContent();
        //    }

        //    [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        //    public IActionResult BorrarCategoria(int categoriaId)
        //    {
        //        if (!_usuarioRepo.ExisteCategoria(categoriaId))
        //        {
        //            return NotFound();
        //        }

        //        var categoria = _usuarioRepo.GetCategoria(categoriaId);

        //        if (!_usuarioRepo.BorrarCategoria(categoria))
        //        {
        //            ModelState.AddModelError("", $"Error al borrar el registro.{categoria.Nombre}");
        //            return StatusCode(500, ModelState);
        //        }

        //        return NoContent();
        //    }
        //}
    }
}
