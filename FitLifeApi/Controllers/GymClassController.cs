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
        return Ok(gymClasses);
    }
}
