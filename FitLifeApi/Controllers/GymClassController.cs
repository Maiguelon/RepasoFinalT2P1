using Microsoft.AspNetCore.Mvc;
using fitlife;
using System.Text.Json;
namespace ConstructoraApi.Controllers;

[ApiController]
[Route("[controller]")]

public class GymClassController : ControllerBase
{
    private List<GymClass> gymClasses;
    private AccesoADatosGymClassJson ADGymClass;
    string rutaGymClasses = Path.Combine("Data", "Data.Json");

    public GymClassController()
    {
        ADGymClass = new AccesoADatosGymClassJson();
        gymClasses = ADGymClass.getDatosGymClass(rutaGymClasses);
    }

    // ---- Get -----
    [HttpGet]
    public ActionResult<List<GymClass>> GetGymClasses()
    {
        if (gymClasses == null)
        {
            return BadRequest("No hay clases para mostrar");
        }
        return Ok(gymClasses);
    }

    [HttpGet("{id}")]
    public ActionResult<GymClass> GetGymClassById(int id)
    {
        GymClass aMostrar = gymClasses.FirstOrDefault(g =>
        g.Id == id);

        if (aMostrar == null)
        {
            return NotFound("No hay clase con ese Id.");
        }

        return Ok(aMostrar);
    }

    [HttpGet("Buscar/{dia}")]
    public ActionResult<List<GymClass>> GetGymClassesByDay(GymClass.dia_semana dia)
    {
        List<GymClass> delDia = gymClasses.Where(g=>
        g.Dia == dia).ToList();
        if (delDia == null)
        {
            return BadRequest("No se encontraron classes ese dia");
        }
        return Ok(delDia);
    }

    // ----- Post -----
    [HttpPost]
    public ActionResult<string> AgregarGymClass([FromBody] GymClass aAgregar)
    {
        // Validaciones
        if (aAgregar == null)
        {
            return BadRequest("Error al recibir la nueva GymClass");
        }
        if (!aAgregar.DuracionValida(aAgregar.DuracionMinutos)) // uso la funcion definida
        {
            return BadRequest("Las GymClasses solo pueden durar 60, 90 o 120 minutos");
        }

        // check solapamiento
        List<GymClass> mismoDia = gymClasses.Where(g => // lista de clases el mismo dia
        g.Dia == aAgregar.Dia).ToList();
        if (mismoDia != null)
        {
            bool solapa = mismoDia.Any(c => // logica de solapamiento
            c.HoraInicio <= aAgregar.HoraFin() &&
            c.HoraFin() >= aAgregar.HoraInicio);
            if (solapa)
            {
                return BadRequest("La GymClass se solapa con otra existente");
            }
        }

        // Id autoincremental
        aAgregar.Id = gymClasses.Count > 0 ? gymClasses.Max(g => g.Id) + 1 : 1;

        // Guardo la GymClass
        gymClasses.Add(aAgregar);
        ADGymClass.guardarGymClass(rutaGymClasses, gymClasses);
        return Created("", aAgregar);
    }

    // ----- PUT -----
    [HttpPut("{id}")]
    public ActionResult ModificarGymClass(int id, [FromBody] GymClass aAgregar)
    {
        // Validaciones iguales que el post
        GymClass aModificar = gymClasses.FirstOrDefault(g =>
        g.Id == id);
        if (aModificar == null) // Valido exisstencia de clase a modificar
        {
            return NotFound("No se encontró clase con ese id para modificar");
        }
        if (aAgregar == null)
        {
            return BadRequest("Error al recibir la nueva GymClass");
        }
        if (!aAgregar.DuracionValida(aAgregar.DuracionMinutos)) 
        {
            return BadRequest("Las GymClasses solo pueden durar 60, 90 o 120 minutos");
        }

        // check solapamiento
        List<GymClass> mismoDia = gymClasses.Where(g => // lista de clases el mismo dia
        g.Dia == aAgregar.Dia).ToList();
        if (mismoDia != null)
        {
            bool solapa = mismoDia.Any(c => // logica de solapamiento
            c.HoraInicio <= aAgregar.HoraFin() &&
            c.HoraFin() >= aAgregar.HoraInicio);
            if (solapa)
            {
                return BadRequest("La GymClass se solapa con otra existente");
            }
        }
        // Mantengo id
        aAgregar.Id = aModificar.Id;

        // Guardo la GymClass
        gymClasses.Add(aAgregar);
        ADGymClass.guardarGymClass(rutaGymClasses, gymClasses);
        return Created("", aAgregar);
    }

    // ----- DELETE -----
    [HttpDelete("{id}")]
    public ActionResult<string> BorrarGymClass(int id)
    {
        GymClass aBorrar = gymClasses.FirstOrDefault(g=>
        g.Id == id);
        if (aBorrar == null)
        {
            return BadRequest("No se encontró GymClass con ese id para borrar.");
        }
        gymClasses.Remove(aBorrar);
        ADGymClass.guardarGymClass(rutaGymClasses, gymClasses);
        return Ok("GymClass aniquilada satisfactoriamente");
    }
}
