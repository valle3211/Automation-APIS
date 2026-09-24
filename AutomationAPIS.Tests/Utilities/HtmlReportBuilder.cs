using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AutomationAPIS.Tests.Utilities;

public record RegistroLlamadaApi(DateTime FechaHora, string Metodo, string Ruta, int CodigoEstado, string CuerpoRespuesta);

public class EntradaReportePrueba
{
    public string NombrePrueba { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
    public string? MensajeError { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraFin { get; set; }
    public double DuracionMs { get; set; }
    public List<string> Etiquetas { get; set; } = new();
    public List<RegistroLlamadaApi> Llamadas { get; set; } = new();
}

public static class ConstructorReporteHtml
{
    private static readonly List<EntradaReportePrueba> Entradas = new();
    private static readonly object Bloqueo = new();

    public static void AgregarResultadoPrueba(EntradaReportePrueba entrada)
    {
        lock (Bloqueo)
        {
            Entradas.Add(entrada);
        }
    }

    public static void GenerarReporte(string rutaSalida)
    {
        List<EntradaReportePrueba> entradas;
        lock (Bloqueo)
        {
            entradas = Entradas.OrderBy(e => e.HoraInicio).ToList();
        }

        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html><html lang=\"es\"><head><meta charset=\"UTF-8\">");
        sb.AppendLine("<title>Reporte de pruebas</title><style>");
        sb.AppendLine(Estilos);
        sb.AppendLine("</style></head><body>");

        sb.AppendLine("<div class=\"topbar\"><span class=\"logo\">&#9635; Reporte de Pruebas</span></div>");
        sb.AppendLine("<div class=\"layout\">");

        // Panel izquierdo: lista de tests
        sb.AppendLine("<div class=\"sidebar\"><div class=\"sidebar-header\">Tests</div><div class=\"test-list\">");
        for (var i = 0; i < entradas.Count; i++)
        {
            var entrada = entradas[i];
            var claseInsignia = ClaseInsignia(entrada.Resultado);
            var claseActiva = i == 0 ? " active" : "";
            sb.AppendLine($"<div class=\"test-item{claseActiva}\" onclick=\"showTest({i})\" id=\"item-{i}\">");
            sb.AppendLine("<div class=\"test-item-main\">");
            sb.AppendLine($"<div class=\"test-item-name\">{Codificar(NombreCorto(entrada.NombrePrueba))}</div>");
            sb.AppendLine($"<div class=\"test-item-meta\">{entrada.HoraInicio:d/M/yyyy h:mm tt} / {entrada.DuracionMs:0.00} ms</div>");
            sb.AppendLine("</div>");
            sb.AppendLine($"<span class=\"badge {claseInsignia}\">{Codificar(ResultadoCorto(entrada.Resultado))}</span>");
            sb.AppendLine("</div>");
        }
        sb.AppendLine("</div></div>");

        // Panel derecho: detalle por test
        sb.AppendLine("<div class=\"details-pane\">");
        for (var i = 0; i < entradas.Count; i++)
        {
            var entrada = entradas[i];
            var estiloVisible = i == 0 ? "flex" : "none";
            sb.AppendLine($"<div class=\"detail-panel\" id=\"panel-{i}\" style=\"display:{estiloVisible}\">");
            sb.AppendLine($"<h1>{Codificar(NombreCorto(entrada.NombrePrueba))}</h1>");
            sb.AppendLine("<div class=\"meta-row\">");
            sb.AppendLine($"<span class=\"pill start\">{entrada.HoraInicio:d/M/yyyy h:mm tt}</span>");
            sb.AppendLine($"<span class=\"pill end\">{entrada.HoraFin:d/M/yyyy h:mm tt}</span>");
            sb.AppendLine($"<span class=\"pill duration\">{entrada.DuracionMs:0.00} ms</span>");
            sb.AppendLine($"<span class=\"pill test-id\">#test-id={i + 1}</span>");
            sb.AppendLine("</div>");

            if (entrada.Etiquetas.Count > 0)
            {
                sb.AppendLine("<div class=\"tags-row\">");
                foreach (var etiqueta in entrada.Etiquetas)
                {
                    sb.AppendLine($"<span class=\"tag\">&#128278; {Codificar(etiqueta)}</span>");
                }
                sb.AppendLine("</div>");
            }

            sb.AppendLine("<table class=\"steps\">");
            sb.AppendLine("<thead><tr><th>STATUS</th><th>TIMESTAMP</th><th>DETAILS</th></tr></thead><tbody>");

            sb.AppendLine(FilaPaso("Info", entrada.HoraInicio, "Inicio de la prueba"));
            foreach (var llamada in entrada.Llamadas)
            {
                var resumen = $"{llamada.Metodo} {Codificar(llamada.Ruta)} &rarr; Status <strong>{llamada.CodigoEstado}</strong>";
                var cuerpoHtml = $"<pre>{Codificar(Truncar(FormatearJson(llamada.CuerpoRespuesta), 3000))}</pre>";
                sb.AppendLine(FilaPaso("Info", llamada.FechaHora, resumen + cuerpoHtml, crudo: true));
            }

            var estadoFinal = entrada.Resultado == "Passed" ? "Pass" : entrada.Resultado == "Failed" ? "Fail" : "Skip";
            var detalleFinal = entrada.Resultado == "Passed"
                ? "Se completo la prueba correctamente"
                : !string.IsNullOrWhiteSpace(entrada.MensajeError)
                    ? $"<pre class=\"error\">{Codificar(entrada.MensajeError)}</pre>"
                    : "La prueba no se ejecuto";
            sb.AppendLine(FilaPaso(estadoFinal, entrada.HoraFin, detalleFinal, crudo: true));

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("</div>");
        }

        if (entradas.Count == 0)
        {
            sb.AppendLine("<div class=\"empty\">No hay resultados de pruebas para mostrar.</div>");
        }

        sb.AppendLine("</div></div>");

        sb.AppendLine("<script>");
        sb.AppendLine(@"
            function showTest(index) {
                document.querySelectorAll('.detail-panel').forEach(function (el) { el.style.display = 'none'; });
                document.querySelectorAll('.test-item').forEach(function (el) { el.classList.remove('active'); });
                document.getElementById('panel-' + index).style.display = 'flex';
                document.getElementById('item-' + index).classList.add('active');
            }
        ");
        sb.AppendLine("</script></body></html>");

        var directorio = Path.GetDirectoryName(rutaSalida);
        if (!string.IsNullOrEmpty(directorio))
        {
            Directory.CreateDirectory(directorio);
        }

        File.WriteAllText(rutaSalida, sb.ToString());
    }

    private static string FilaPaso(string estado, DateTime fechaHora, string detalles, bool crudo = false)
    {
        var claseEstado = estado switch
        {
            "Pass" => "status-pass",
            "Fail" => "status-fail",
            "Skip" => "status-skip",
            _ => "status-info"
        };

        var detallesHtml = crudo ? detalles : Codificar(detalles);
        return $"<tr><td><span class=\"badge {claseEstado}\">{estado}</span></td><td>{fechaHora:MMM d, yyyy h:mm:ss tt}</td><td>{detallesHtml}</td></tr>";
    }

    private static string NombreCorto(string nombreCompleto)
    {
        var ultimoPunto = nombreCompleto.LastIndexOf('.');
        return ultimoPunto >= 0 ? nombreCompleto[(ultimoPunto + 1)..] : nombreCompleto;
    }

    private static string ResultadoCorto(string resultado) => resultado switch
    {
        "Passed" => "Pass",
        "Failed" => "Fail",
        _ => "Skip"
    };

    private static string ClaseInsignia(string resultado) => resultado switch
    {
        "Passed" => "status-pass",
        "Failed" => "status-fail",
        _ => "status-skip"
    };

    private static string Codificar(string valor) => HtmlEncoder.Default.Encode(valor);

    private static string Truncar(string valor, int max)
        => valor.Length <= max ? valor : valor[..max] + "... (truncado)";

    private static string FormatearJson(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (JsonException)
        {
            return json;
        }
    }

    private const string Estilos = @"
        * { box-sizing: border-box; }
        body { margin:0; font-family: 'Segoe UI', Arial, sans-serif; background:#0f1620; color:#d7dde5; }
        .topbar { background:#0a0f16; padding:14px 20px; border-bottom:1px solid #1e2733; }
        .logo { color:#4da3ff; font-weight:600; font-size:16px; }
        .layout { display:flex; height:calc(100vh - 49px); }
        .sidebar { width:340px; border-right:1px solid #1e2733; overflow-y:auto; background:#0f1620; }
        .sidebar-header { padding:14px 16px; font-size:13px; color:#8b96a5; border-bottom:1px solid #1e2733; text-transform:uppercase; letter-spacing:.5px; }
        .test-item { display:flex; justify-content:space-between; align-items:center; padding:12px 16px; border-bottom:1px solid #1a222c; cursor:pointer; }
        .test-item:hover { background:#151d28; }
        .test-item.active { background:#16212c; border-left:3px solid #4da3ff; }
        .test-item-name { font-size:14px; color:#e7ecf2; margin-bottom:4px; }
        .test-item-meta { font-size:12px; color:#7c8794; }
        .details-pane { flex:1; overflow-y:auto; padding:24px 32px; }
        .detail-panel { display:none; flex-direction:column; gap:12px; }
        .detail-panel h1 { margin:0; font-size:20px; color:#4da3ff; }
        .meta-row, .tags-row { display:flex; gap:8px; flex-wrap:wrap; }
        .pill { font-size:12px; padding:5px 10px; border-radius:4px; background:#1c2733; color:#c6cdd6; }
        .pill.start { background:#1565c0; color:#fff; }
        .pill.end { background:#ad1457; color:#fff; }
        .tag { font-size:12px; padding:4px 10px; border-radius:12px; background:#22303c; color:#9fd3ff; }
        table.steps { width:100%; border-collapse:collapse; margin-top:8px; }
        table.steps th { text-align:left; font-size:12px; color:#7c8794; padding:10px 12px; border-bottom:1px solid #1e2733; }
        table.steps td { padding:12px; border-bottom:1px solid #17202a; font-size:13px; vertical-align:top; }
        .badge { display:inline-block; font-size:11px; font-weight:700; padding:3px 10px; border-radius:4px; color:#fff; }
        .status-pass { background:#43a047; }
        .status-fail { background:#e53935; }
        .status-skip { background:#fb8c00; }
        .status-info { background:#1e88e5; }
        pre { white-space:pre-wrap; word-break:break-word; font-size:12px; background:#0a0f16; color:#c6cdd6; padding:10px; border-radius:4px; max-height:260px; overflow:auto; margin-top:6px; }
        pre.error { background:#2a1414; color:#ff9e9e; }
        .empty { padding:40px; color:#7c8794; }
    ";
}

