using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2020GH601.Models;

namespace L01_2020GH601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly blogDBContext _blogDBContexto;
        public usuariosController(blogDBContext blogDBContexto)
        {
            _blogDBContexto = blogDBContexto;
        }

        //todos los registros de una tabla 
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<usuarios> listadoUsuarios = (from e in _blogDBContexto.usuarios
                                           select e).ToList();

            if (listadoUsuarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }

        //guardar un registro en la base de datos
        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarUsuario([FromBody] usuarios usuario)
        {
            try
            {
                _blogDBContexto.usuarios.Add(usuario);
                _blogDBContexto.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //modificar y actualizar un registro
        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuarioModificar)
        {
            usuarios? usuarioActual = (from e in _blogDBContexto.usuarios
                                     where e.usuarioId == id
                                     select e).FirstOrDefault();

            if (usuarioActual == null)
            { return NotFound(); }

            usuarioActual.rolId = usuarioModificar.rolId;
            usuarioActual.nombreUsuario = usuarioModificar.nombreUsuario;
            usuarioActual.clave = usuarioModificar.clave;
            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.apellido = usuarioModificar.apellido;


            _blogDBContexto.Entry(usuarioActual).State = EntityState.Modified;
            _blogDBContexto.SaveChanges(); 
            return Ok(usuarioModificar);
        }

        //eliminar un registro
        [HttpDelete]
        [Route("eliminar/{id}")]

        public ActionResult EliminarUsuario(int id)
        {
            usuarios? usuario = (from e in _blogDBContexto.usuarios
                               where e.usuarioId == id
                               select e).FirstOrDefault();

            if (usuario == null)
                return NotFound();

            _blogDBContexto.usuarios.Attach(usuario);
            _blogDBContexto.usuarios.Remove(usuario);
            _blogDBContexto.SaveChanges();

            return Ok(usuario);
        }

    }
}
