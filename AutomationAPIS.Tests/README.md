# Playwright C# Automation Framework

## Objetivo

investigar e implementar automatización utilizando Playwright con C# y .NET en Visual Studio Code para fortalecer conocimientos de QA Automation y desarrollar pruebas automatizadas cn APIs.

---

# Tecnologías

- .NET 8
- C#
- Playwright
- NUnit
- VS Code
- Git

---

# Instalación y Configuración de Playwright con C# en Visual Studio Code

## ¿Qué es Playwright?

Playwright es una herramienta de automatización desarrollada por Microsoft que permite simular la interacción de un usuario con aplicaciones web y APIs. Puede utilizarse para automatizar pruebas funcionales, pruebas de regresión y validaciones de servicios REST.

En este proyecto se utiliza Playwright junto con C# y NUnit para automatizar pruebas de APIs.

---

## Prerrequisitos

Antes de comenzar es necesario tener instalado:

- Visual Studio Code
- .NET SDK 8 o superior

Para verificar que .NET está instalado, abrir una terminal y ejecutar:

```bash
dotnet --version
```

Si el comando devuelve una versión, significa que .NET está correctamente instalado.

---

## Crear la Solución

Crear una carpeta para el proyecto:

```bash
mkdir AutomationAPIS
cd AutomationAPIS
```

Crear una solución:

```bash
dotnet new sln -n AutomationAPIS
```

Crear un proyecto de pruebas NUnit:

```bash
dotnet new nunit -n AutomationAPIS.Tests
```

Agregar el proyecto a la solución:

```bash
dotnet sln add AutomationAPIS.Tests
```

Abrir el proyecto en Visual Studio Code:

```bash
code .
```

---

## Instalar Playwright

Ubicarse dentro del proyecto de pruebas:

```bash
cd AutomationAPIS.Tests
```

Instalar el paquete de Playwright:

```bash
dotnet add package Microsoft.Playwright
```

Restaurar las dependencias del proyecto:

```bash
dotnet restore
```

Compilar el proyecto:

```bash
dotnet build
```

---

## Instalar los Navegadores de Playwright

Una vez compilado el proyecto, es necesario instalar los navegadores que Playwright utilizará para ejecutar las pruebas.

Para instalar todos los navegadores:

```bash
pwsh bin/Debug/net8.0/playwright.ps1 install
```

Para instalar únicamente Chromium:

```bash
pwsh bin/Debug/net8.0/playwright.ps1 install chromium
```

Cuando la instalación finalice, Playwright estará listo para utilizarse.

---

## Ejecutar las Pruebas

Para ejecutar todas las pruebas del proyecto:

```bash
dotnet test
```

Para ejecutar una prueba específica:

```bash
dotnet test --filter NombreDelMetodo
```

---

## Estructura General del Proyecto

El proyecto está organizado siguiendo buenas prácticas de automatización:

```text
AutomationAPIS
│
├── Base
│   └── ApiTestBase.cs
│   └── BaseTest.cs
│   └── GlobalTestHooks.cs
│
├── Models
│   └── Producto.cs
│
├── Tests
│   └── PruebasApiProductos.cs
│
└── AutomationAPIS.Tests.csproj
```

### Base

Contiene clases reutilizables para la configuración y ejecución de las pruebas.

### Models

Contiene los modelos utilizados para mapear las respuestas JSON de la API a objetos de C#.

### Tests

Contiene los casos de prueba automatizados que validan el comportamiento de la API.

---

## Beneficios de Utilizar Playwright

- Permite automatizar pruebas de forma rápida y confiable.
- Soporta múltiples navegadores.
- Se integra fácilmente con C# y .NET.
- Facilita la validación de respuestas HTTP y datos JSON.
- Permite crear pruebas mantenibles y escalables.
- Es una herramienta ampliamente utilizada en automatización moderna.

# Conceptos Básicos de API REST

## ¿Qué es una API REST?

Una API (Application Programming Interface) es un conjunto de reglas que permite la comunicación entre diferentes aplicaciones mediante solicitudes y respuestas.

REST (Representational State Transfer) es un estilo arquitectónico utilizado para diseñar servicios web que intercambian información utilizando el protocolo HTTP.

Las APIs REST trabajan con recursos, los cuales son identificados mediante URLs.

Ejemplo:

```text
https://api.ejemplo.com/users
```

En este ejemplo, `users` es el recurso que puede ser consultado, creado, actualizado o eliminado.

---

# Arquitectura Cliente-Servidor

En una API REST intervienen principalmente dos componentes:

## Cliente

Es la aplicación que envía solicitudes al servidor.

Ejemplos:

- Aplicación Web
- Aplicación Móvil
- Framework de Automatización
- Postman
- Playwright
- Aplicaciones de terceros

## Servidor

Es la aplicación que procesa las solicitudes y devuelve una respuesta.

---

# Estructura de una Solicitud HTTP

Toda petición a una API REST está compuesta por:

1. Método HTTP
2. URL o Endpoint
3. Headers
4. Body (opcional)

Ejemplo:

```http
POST /users HTTP/1.1
Host: api.ejemplo.com
Content-Type: application/json
Authorization: Bearer token123

{
  "name": "Douglas",
  "email": "douglas@email.com"
}
```

---

# Métodos HTTP

Los métodos HTTP indican qué operación se desea realizar sobre un recurso.

## GET

Se utiliza para obtener información.

### Ejemplo

```http
GET /users/1
```

### Respuesta

```json
{
  "id": 1,
  "name": "Douglas"
}
```

### Casos de Uso

- Consultar usuarios
- Obtener órdenes
- Consultar productos
- Obtener información de un recurso

---

## POST

Se utiliza para crear nuevos recursos.

### Ejemplo

```http
POST /users
```

### Body

```json
{
  "name": "Douglas",
  "email": "douglas@email.com"
}
```

### Casos de Uso

- Registrar usuarios
- Crear productos
- Generar órdenes
- Crear registros

---

## PUT

Se utiliza para actualizar completamente un recurso existente.

### Ejemplo

```http
PUT /users/1
```

### Body

```json
{
  "name": "Douglas Jimenez",
  "email": "douglas@email.com"
}
```

### Casos de Uso

- Actualizar perfiles
- Modificar información completa de un registro

---

## PATCH

Se utiliza para actualizar parcialmente un recurso.

### Ejemplo

```http
PATCH /users/1
```

### Body

```json
{
  "email": "nuevo@email.com"
}
```

### Casos de Uso

- Actualizar únicamente un campo
- Cambiar estados
- Actualizar configuraciones específicas

---

## DELETE

Se utiliza para eliminar recursos.

### Ejemplo

```http
DELETE /users/1
```

### Casos de Uso

- Eliminar usuarios
- Eliminar productos
- Eliminar registros

---

# Códigos de Estado HTTP

Los códigos de estado indican el resultado de una solicitud.

## Respuestas Exitosas (2XX)

### 200 OK

La solicitud fue procesada correctamente.

```http
HTTP/1.1 200 OK
```

---

### 201 Created

El recurso fue creado exitosamente.

```http
HTTP/1.1 201 Created
```

---

### 202 Accepted

La solicitud fue aceptada para procesamiento.

```http
HTTP/1.1 202 Accepted
```

---

### 204 No Content

La operación fue exitosa y no hay contenido para devolver.

Muy común en operaciones DELETE.

```http
HTTP/1.1 204 No Content
```

---

## Errores del Cliente (4XX)

### 400 Bad Request

La solicitud enviada es inválida.

```http
HTTP/1.1 400 Bad Request
```

---

### 401 Unauthorized

El usuario no está autenticado.

```http
HTTP/1.1 401 Unauthorized
```

---

### 403 Forbidden

El usuario está autenticado pero no tiene permisos.

```http
HTTP/1.1 403 Forbidden
```

---

### 404 Not Found

El recurso solicitado no existe.

```http
HTTP/1.1 404 Not Found
```

---

### 405 Method Not Allowed

El método HTTP no está permitido para el recurso.

```http
HTTP/1.1 405 Method Not Allowed
```

---

### 409 Conflict

Existe un conflicto con los datos enviados.

```http
HTTP/1.1 409 Conflict
```

---

## Errores del Servidor (5XX)

### 500 Internal Server Error

Error interno del servidor.

```http
HTTP/1.1 500 Internal Server Error
```

---

### 502 Bad Gateway

Error de comunicación entre servidores.

```http
HTTP/1.1 502 Bad Gateway
```

---

### 503 Service Unavailable

El servicio no está disponible temporalmente.

```http
HTTP/1.1 503 Service Unavailable
```

---

# JSON (JavaScript Object Notation)

JSON es el formato más utilizado para intercambiar información entre cliente y servidor.

## Ejemplo de JSON

```json
{
  "id": 1,
  "firstName": "Douglas",
  "lastName": "Jimenez",
  "active": true
}
```

---

# Tipos de Datos en JSON

## String

```json
{
  "name": "Douglas"
}
```

---

## Number

```json
{
  "age": 30
}
```

---

## Decimal

```json
{
  "salary": 1500.75
}
```

---

## Boolean

```json
{
  "active": true
}
```

---

## Array

```json
{
  "roles": [
    "Admin",
    "QA",
    "Automation"
  ]
}
```

---

## Object

```json
{
  "address": {
    "country": "Costa Rica",
    "city": "San Jose"
  }
}
```

---

# Request Body

El Request Body contiene la información enviada al servidor.

Ejemplo:

```json
{
  "name": "Douglas",
  "email": "douglas@email.com",
  "role": "QA"
}
```

Generalmente se utiliza en:

- POST
- PUT
- PATCH

---

# Response Body

Es la información devuelta por el servidor.

Ejemplo:

```json
{
  "id": 100,
  "name": "Douglas",
  "email": "douglas@email.com",
  "createdAt": "2026-09-24T10:00:00Z"
}
```
---

# Endpoint

Un endpoint es una URL específica que representa un recurso dentro de una API.

Ejemplos:

```text
GET /users
```

Obtener usuarios.

```text
GET /users/1
```

Obtener un usuario específico.

```text
POST /users
```

Crear un usuario.

```text
PUT /users/1
```

Actualizar un usuario.

```text
DELETE /users/1
```

Eliminar un usuario.

---

# URL Parameters

Permiten identificar recursos específicos.

Ejemplo:

```http
GET /users/10
```

En este caso:

```text
10
```

es el identificador del usuario.

---

# Query Parameters

Permiten filtrar información.

Ejemplo:

```http
GET /users?page=1&size=10
```

Parámetros:

```text
page = 1
size = 10
```

# Flujo Típico en una API REST

1. El cliente envía una solicitud HTTP.
2. El servidor recibe la solicitud.
3. El servidor procesa la información.
4. El servidor devuelve una respuesta.
5. El cliente valida el código de estado y el contenido recibido.

Ejemplo:

```text
Cliente
   |
   | POST /users
   |
Servidor
   |
   | 201 Created
   |
Cliente
```
# Conocimiento Aplicado en el Ejercicio Práctico de API Testing

El objetivo de este ejercicio fue aplicar los conceptos fundamentales de API REST mediante la automatización de pruebas utilizando C# y Playwrigh. 


# Conceptos Aplicados

Durante la implementación de las pruebas se pusieron en práctica los siguientes conceptos:

- Métodos HTTP (GET, POST y PUT).
- Validación de códigos de estado HTTP.
- Consumo de APIs REST.
- Serialización y deserialización de objetos JSON.
- Validación de respuestas.
- Manejo de endpoints.
- Automatización de pruebas API.
- Diseño de pruebas positivas y negativas.

---

# Prueba 1: Obtener Lista de Productos

## Escenario

Consultar el endpoint de productos para validar que la API responde correctamente.

## Método HTTP Utilizado

```http
GET /products
```

## Validaciones Realizadas

- El endpoint responde correctamente.
- El código de estado es 200.
- La respuesta indica éxito.

## Conocimientos Aplicados

- Consumo de endpoints REST.
- Método HTTP GET.
- Validación de códigos de estado.
- Verificación de respuestas exitosas.

## Aprendizaje Obtenido

Aprendí a realizar solicitudes GET para recuperar información desde una API y validar que la respuesta se procese correctamente mediante las propiedades de la respuesta HTTP.

---

# Prueba 2: Obtener Producto por ID

## Escenario

Consultar un producto específico utilizando su identificador.

## Método HTTP Utilizado

```http
GET /products/{id}
```

## Validaciones Realizadas

- Código de estado 200.
- El producto retornado no es nulo.
- El identificador coincide con el solicitado.
- El título del producto contiene información.

## Conocimientos Aplicados

- Uso de parámetros en la URL.
- Deserialización de respuestas JSON.
- Validación de propiedades específicas.
- Validación de datos recibidos desde la API.

## Aprendizaje Obtenido

Aprendí a trabajar con endpoints parametrizados y a convertir respuestas JSON en objetos C# utilizando `JsonSerializer`, lo que facilita la validación de datos específicos dentro de una respuesta.

---

# Prueba 3: Crear Producto

## Escenario

Crear un nuevo producto mediante una solicitud POST.

## Método HTTP Utilizado

```http
POST /products
```

## Request Body Utilizado

```json
{
  "title": "Producto de prueba",
  "price": 19.99,
  "description": "Producto creado desde una prueba automatizada",
  "category": "CategoriaPrueba"
}
```

## Validaciones Realizadas

- El código de estado es 201 Created.
- El producto fue creado correctamente.
- Los datos retornados coinciden con los enviados.
- Se valida título, precio y categoría.

## Conocimientos Aplicados

- Método HTTP POST.
- Construcción de payloads JSON.
- Validación de respuestas después de una creación.
- Comparación entre datos enviados y recibidos.

## Aprendizaje Obtenido

Aprendí a enviar información mediante un cuerpo JSON y validar que el servidor procese correctamente la creación de recursos. También reforcé la comprensión del código de estado 201 como indicador de una creación exitosa.

---

# Prueba 4: Actualizar Producto Existente

## Escenario

Actualizar un producto existente utilizando un identificador específico.

## Método HTTP Utilizado

```http
PUT /products/{id}
```

## Request Body Utilizado

```json
{
  "title": "Producto actualizado",
  "price": 29.99,
  "description": "Descripcion actualizada desde una prueba automatizada",
  "category": "electronics"
}
```

## Validaciones Realizadas

- Código de estado 200.
- El identificador del producto se mantiene.
- El título se actualiza correctamente.
- El precio se actualiza correctamente.

## Conocimientos Aplicados

- Método HTTP PUT.
- Actualización de recursos.
- Validación de persistencia de identificadores.
- Verificación de cambios en la información.

## Aprendizaje Obtenido

Aprendí a validar operaciones de actualización y a verificar que los cambios enviados al servidor sean reflejados correctamente en la respuesta recibida.

---

# Prueba 5: Validación de Endpoint Inválido

## Escenario

Enviar una solicitud a un endpoint inexistente.

## Método HTTP Utilizado

```http
GET /productos
```

## Validaciones Realizadas

- Código de estado 404 Not Found.

## Conocimientos Aplicados

- Escenarios negativos.
- Manejo de errores.
- Validación de recursos inexistentes.
- Interpretación de códigos HTTP.

## Aprendizaje Obtenido

Aprendí la importancia de validar escenarios negativos para garantizar que la aplicación responda adecuadamente ante solicitudes incorrectas o recursos inexistentes.

---

# Manejo de JSON

Durante la práctica se utilizó deserialización de datos JSON hacia objetos de tipo `Producto`.

Ejemplo:

```csharp
var producto = JsonSerializer.Deserialize<Producto>(cuerpo, OpcionesJson);
```

## Beneficios

- Facilita la lectura de datos.
- Permite validar propiedades específicas.
- Mejora la mantenibilidad del código.
- Reduce la manipulación manual de cadenas JSON.

---

# Validación de Códigos de Estado

Durante el ejercicio se validaron los siguientes códigos HTTP:

| Código | Significado | Escenario |
|----------|------------|------------|
| 200 | OK | Consultas exitosas |
| 201 | Created | Creación de productos |
| 404 | Not Found | Endpoint inexistente |

## Aprendizaje Obtenido

Comprendí la importancia de validar los códigos de estado para determinar rápidamente si la operación ejecutada fue exitosa o si ocurrió algún error.

---

# Buenas Prácticas Aplicadas

Durante el desarrollo de las pruebas se aplicaron las siguientes buenas prácticas:

- Separación de lógica mediante una clase base.
- Uso de métodos reutilizables para solicitudes HTTP.
- Uso de modelos para representar respuestas.
- Validaciones específicas para cada escenario.
- Cobertura de escenarios positivos y negativos.
- Nombres descriptivos para los métodos de prueba.
- Uso del patrón Arrange, Act y Assert.

---

# Resultados Obtenidos

Al finalizar el ejercicio se logró:

- Comprender el funcionamiento de una API REST.
- Consumir endpoints mediante solicitudes HTTP.
- Validar respuestas y códigos de estado.
- Trabajar con datos JSON.
- Automatizar escenarios reales utilizando C# y Playwright API.
- Implementar pruebas positivas y negativas.
- Aplicar buenas prácticas de automatización para pruebas de servicios.

---

# Conclusión

Este ejercicio práctico permitió aplicar de manera integral los conceptos fundamentales de API Testing mediante la automatización de pruebas REST. A través de la validación de operaciones de consulta, creación y actualización de productos, así como del manejo de escenarios negativos, se fortalecieron conocimientos relacionados con HTTP, JSON, validación de respuestas y automatización de servicios utilizando C# y Playwright. La experiencia adquirida sienta una base sólida para desarrollar frameworks más robustos y ampliar la cobertura de pruebas automatizadas en proyectos futuros.