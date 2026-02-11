namespace fitlife;
using System.ComponentModel.DataAnnotations;

public class GymClass
{
    public enum dia_semana
    {
        Lunes = 1,
        Martes = 2,
        Miercoles = 3,
        Jueves = 4,
        Viernes = 5,
        Sabado = 6,
        Domingo = 7
    }
    public int Id {get; set;}

    [Required(ErrorMessage = "Nombre de la clase es obligatoria.")]
    [StringLength(maximumLength: 50, ErrorMessage = "Hasta 50 caracteres para nombres de clases.")]
    public string Nombre {get; set;}

    [Required(ErrorMessage = "Nombre del instructor obligatorio")]
    public string Instructor {get; set;}

    public dia_semana Dia {get; set;}
    public DateTime HoraInicio {get; set;}
    
    [Required(ErrorMessage = "Duración en minutos requerida")]
    public int DuracionMinutos {get; set;}

    [Required(ErrorMessage = "Cupo requerido")]
    [Range(5, 30, ErrorMessage = "Solo se aceptan clases entre 5 y 30 alumnos.")]
    public int CupoMaximo {get; set;}

}