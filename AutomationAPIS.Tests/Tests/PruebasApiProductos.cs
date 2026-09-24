using System.Text.Json;
using AutomationAPIS.Tests.Base;
using AutomationAPIS.Tests.Models;

namespace AutomationAPIS.Tests.Tests;

public class PruebasApiProductos : PruebaBaseApi
{
    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Test]
    public async Task ObtenerProductos_RetornaEstadoOk()
    {
        var (respuesta, _) = await ObtenerAsync("/products");

        Assert.That(respuesta.Status, Is.EqualTo(200));
        Assert.That(respuesta.Ok, Is.True);
    }

    [Test]
    public async Task ObtenerProductoPorId_RetornaProductoCoincidente()
    {
        const int idProducto = 1;
        var (respuesta, cuerpo) = await ObtenerAsync($"/products/{idProducto}");
        var producto = JsonSerializer.Deserialize<Producto>(cuerpo, OpcionesJson);

        Assert.That(respuesta.Status, Is.EqualTo(200));
        Assert.That(producto, Is.Not.Null);
        Assert.That(producto!.Id, Is.EqualTo(idProducto));
        Assert.That(producto.Titulo, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task CrearProducto_CreaNuevoProducto()
    {
        var datos = new
        {
            title = "Producto de prueba",
            price = 19.99,
            description = "Producto creado desde una prueba automatizada",
            category = "CategorriaPrueba",
            image = "https://www.google.com/url?sa=t&source=web&rct=j&url=https%3A%2F%2Fregularshow.fandom.com%2Fes%2Fwiki%2FBenson&ved=0CBYQjRxqFwoTCIjRo_jah5cDFQAAAAAdAAAAABAF&opi=89978449"
        };

        var (respuesta, cuerpo) = await CrearAsync("/products", datos);
        var producto = JsonSerializer.Deserialize<Producto>(cuerpo, OpcionesJson);

        Assert.That(respuesta.Status, Is.EqualTo(201));
        Assert.That(producto, Is.Not.Null);
        Assert.That(producto!.Titulo, Is.EqualTo(datos.title));
        Assert.That(producto.Precio, Is.EqualTo((decimal)datos.price));
        Assert.That(producto.Categoria, Is.EqualTo(datos.category));
    }

    [Test]
    public async Task ActualizarProductoPorId_ActualizaProductoExistente()
    {
        const int idProducto = 1;
        var datos = new
        {
            title = "Producto actualizado",
            price = 29.99,
            description = "Descripcion actualizada desde una prueba automatizada",
            category = "electronics",
            image = "https://www.google.com/url?sa=t&source=web&rct=j&url=https%3A%2F%2Fregularshow.fandom.com%2Fes%2Fwiki%2FBenson&ved=0CBYQjRxqFwoTCIjRo_jah5cDFQAAAAAdAAAAABAF&opi=89978449"
        };

        var (respuesta, cuerpo) = await ActualizarAsync($"/products/{idProducto}", datos);
        var producto = JsonSerializer.Deserialize<Producto>(cuerpo, OpcionesJson);

        Assert.That(respuesta.Status, Is.EqualTo(200));
        Assert.That(producto, Is.Not.Null);
        Assert.That(producto!.Id, Is.EqualTo(idProducto));
        Assert.That(producto.Titulo, Is.EqualTo(datos.title));
        Assert.That(producto.Precio, Is.EqualTo((decimal)datos.price));
    }

[Test]
public async Task EndpointInvalido_Retorna404()
{
    var (respuesta, _) = await ObtenerAsync("/productos");

    Assert.That(respuesta.Status, Is.EqualTo(404));
}
}