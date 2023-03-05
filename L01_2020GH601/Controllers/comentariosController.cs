using L01_2020GH601.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020GH601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : Controller
    {
        private readonly blogDBContext _blogDBContexto;
        public comentariosController(blogDBContext blogDBContexto)
        {
            _blogDBContexto = blogDBContexto;
        }

        //todos los comentarios de una tabla 
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<comentarios> listadoComentarios = (from e in _blogDBContexto.comentarios
                                                      select e).ToList();

            if (listadoComentarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoComentarios);
        }

        //guardar un comentario en la base de datos
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarComentario([FromBody] comentarios comentario)
        {
            try
            {
                _blogDBContexto.comentarios.Add(comentario);
                _blogDBContexto.SaveChanges();
                return Ok(comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //modificar y actualizar un comentario
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarPublicacion(int id, [FromBody] comentarios comentarioModificar)
        {
            comentarios? comentarioActual = (from e in _blogDBContexto.comentarios
                                             where e.comentarioId == id
                                                select e).FirstOrDefault();

            if (comentarioActual == null)
            { return NotFound(); }

            comentarioActual.publicacionId = comentarioModificar.publicacionId;
            comentarioActual.comentario = comentarioModificar.comentario;
            comentarioActual.usuarioId = comentarioModificar.usuarioId;


            _blogDBContexto.Entry(comentarioActual).State = EntityState.Modified;
            _blogDBContexto.SaveChanges();
            return Ok(comentarioModificar);
        }

        //eliminar un comentario
        [HttpDelete]
        [Route("eliminar/{id}")]

        public ActionResult EliminarComentario(int id)
        {
            comentarios? comentario = (from e in _blogDBContexto.comentarios
                                          where e.comentarioId == id
                                          select e).FirstOrDefault();

            if (comentario == null)
                return NotFound();

            _blogDBContexto.comentarios.Attach(comentario);
            _blogDBContexto.comentarios.Remove(comentario);
            _blogDBContexto.SaveChanges();

            return Ok(comentario);
        }

        //metodo para filtrar todos los comentarios por una publicacion especifica
        [HttpGet]
        [Route("Find/{filtroComentarios}")]

        public IActionResult FindByComments(int filtroComentarios)
        {
            List<comentarios> listadoComentarios = (from e in _blogDBContexto.comentarios
                                                        where e.publicacionId == filtroComentarios
                                                        select e).ToList();

            if (listadoComentarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoComentarios);
        }

    }
}
