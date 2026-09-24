using System.Text.Json.Serialization;

namespace AutomationAPIS.Tests.Models;

public class Producto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Titulo { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Precio { get; set; }

    [JsonPropertyName("description")]
    public string Descripcion { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Categoria { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string Imagen { get; set; } = string.Empty;

    [JsonPropertyName("rating")]
    public Calificacion? Calificacion { get; set; }
}

public class Calificacion
{
    [JsonPropertyName("rate")]
    public double Valor { get; set; }

    [JsonPropertyName("count")]
    public int Cantidad { get; set; }
}
