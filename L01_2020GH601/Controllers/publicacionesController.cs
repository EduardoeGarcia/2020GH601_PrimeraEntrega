using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2020GH601.Models;

namespace L01_2020GH601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class publicacionesController : Controller
    {
        private readonly blogDBContext _blogDBContexto;
        public publicacionesController(blogDBContext blogDBContexto)
        {
            _blogDBContexto = blogDBContexto;
        }

        //todos las publicaciones de una tabla 
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<publicaciones> listadoPublicaciones = (from e in _blogDBContexto.publicaciones
                                                        select e).ToList();

            if (listadoPublicaciones.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoPublicaciones);
        }

        //guardar una publicacion en la base de datos
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarPublicacion([FromBody] publicaciones publicacion)
        {
            try
            {
                _blogDBContexto.publicaciones.Add(publicacion);
                _blogDBContexto.SaveChanges();
                return Ok(publicacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //modificar y actualizar una publicacion
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarPublicacion(int id, [FromBody] publicaciones publicacionModificar)
        {
            publicaciones? publicacionActual = (from e in _blogDBContexto.publicaciones
                                                where e.publicacionId == id
                                                select e).FirstOrDefault();

            if (publicacionActual == null)
            { return NotFound(); }

            publicacionActual.titulo = publicacionModificar.titulo;
            publicacionActual.descripcion = publicacionModificar.descripcion;
            publicacionActual.usuarioId = publicacionModificar.usuarioId;


            _blogDBContexto.Entry(publicacionActual).State = EntityState.Modified;
            _blogDBContexto.SaveChanges();
            return Ok(publicacionModificar);
        }

        //eliminar una publicacion
        [HttpDelete]
        [Route("eliminar/{id}")]

        public ActionResult EliminarPublicacion(int id)
        {
            publicaciones? publicacion = (from e in _blogDBContexto.publicaciones
                                 where e.publicacionId == id
                                 select e).FirstOrDefault();

            if (publicacion == null)
                return NotFound();

            _blogDBContexto.publicaciones.Attach(publicacion);
            _blogDBContexto.publicaciones.Remove(publicacion);
            _blogDBContexto.SaveChanges();

            return Ok(publicacion);
        }

        //metodo para filtrar por usuarioEspecifico
        [HttpGet]
        [Route("Find/{filtroUsuario}")]

        public IActionResult FindByUser(int filtroUsuario)
        {
            List<publicaciones> listadoPublicaciones = (from e in _blogDBContexto.publicaciones
                                                        where e.usuarioId == filtroUsuario
                                                        select e).ToList();

            if (listadoPublicaciones.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoPublicaciones);
        }

    }
}
