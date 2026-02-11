# 🏋️‍♀️ Sistema de Gestión de Clases "FitLife Gym"

## Contexto
El gimnasio "FitLife" necesita digitalizar su pizarra de horarios. Actualmente, anotan las clases en un cuaderno y suelen cometer errores, como asignar dos clases al mismo tiempo en el **Salón Principal** (el gimnasio solo tiene un salón).

El objetivo es desarrollar una API que permita administrar el cronograma semanal, asegurando que los datos sean consistentes y que **no haya conflictos de horario**.

---

## 🛠 Modelo Sugerido

### Clase `GymClass`
Representa una clase dictada en el gimnasio.
* `Id` (int): Identificador único.
* `Nombre` (string): Nombre de la clase (ej: "Zumba", "Pilates").
* `Instructor` (string): Nombre del profesor.
* `Dia` (string): Día de la semana (ej: "Lunes", "Martes").
* `HoraInicio` (int): Hora de inicio en formato 24hs (ej: `14` para las 14:00, `9` para las 09:00). *Asumir que siempre empiezan en punto.*
* `DuracionMinutos` (int): Duración de la clase.
* `CupoMaximo` (int): Cantidad máxima de alumnos.

---

## 📝 Tareas a Realizar

1.  **Acceso a Datos:** Crear una clase `AccesoADatos` que lea y escriba el archivo `clases.json`.
2.  **API REST:** Desarrollar un controlador que permita el **ABM Completo**.

### Tabla de Endpoints

| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| **GET** | `/api/clases` | Lista todas las clases disponibles. |
| **GET** | `/api/clases/{id}` | Obtiene una clase por su ID. |
| **POST** | `/api/clases` | Crea una nueva clase (Validar Annotations y Solapamiento). |
| **PUT** | `/api/clases/{id}` | Modifica una clase existente (Validar todo nuevamente). |
| **DELETE** | `/api/clases/{id}` | Elimina una clase del sistema. |
| **GET** | `/api/clases/buscar/{dia}` | Devuelve todas las clases de un día específico (ej: "Lunes"). |

---

## 🛡️ Validaciones y Reglas de Negocio

### 1. Validaciones por Data Annotations (En el Modelo)
Decorar la clase `GymClass` para validar automáticamente:
* **Nombre:** Obligatorio, Máximo 50 caracteres.
* **Instructor:** Obligatorio.
* **CupoMaximo:** Obligatorio. Debe estar entre **5 y 30** personas (inclusive).
* **DuracionMinutos:** Obligatorio.

### 2. Validaciones de Lógica (En el Controller)
Estas validaciones deben hacerse manualmente en el controlador:

* **Duración Permitida:** Las clases solo pueden durar **60, 90 o 120 minutos**. Cualquier otro valor debe ser rechazado con un `BadRequest`.
* **Solapamiento (El punto crítico):**
    * No se puede crear (ni modificar) una clase si ya existe otra en el **mismo día** que se superponga en horario.
    * *Ayuda:* Calcular la hora de fin usando `HoraInicio + (Duracion / 60)`. (Ojo con los decimales si dura 90 mins).
    * *Lógica:* Dos intervalos se superponen si `Inicio A < Fin B` Y `Fin A > Inicio B`.
