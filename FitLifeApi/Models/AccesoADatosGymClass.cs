namespace fitlife;
using System.Text.Json;

public class AccesoADatosGymClassJson
{
    public List<GymClass> getDatosGymClass(string archivo)
    {
        // Si el archivo no tiene programas, devuelve una lista vacía
        if (!File.Exists(archivo))
        {
            return new List<GymClass>();
        }

        string linea = File.ReadAllText(archivo);
        List<GymClass> gymClasees = JsonSerializer.Deserialize<List<GymClass>>(linea);
        return gymClasees ?? new List<GymClass>();
    }

    public void guardarGymClass(string archivo, List<GymClass> gymClasses)
    {
        var opciones = new JsonSerializerOptions {WriteIndented = true};
        string json = JsonSerializer.Serialize(gymClasses, opciones);
        File.WriteAllText(archivo, json);
    }
}