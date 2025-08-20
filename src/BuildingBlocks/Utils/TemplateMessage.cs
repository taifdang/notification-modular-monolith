using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
namespace BuildingBlocks.Utils;

public interface ITemplateMessage 
{
    string FileContent(string path);
    Dictionary<string, string> GetTemplate(string json);
    string RenderTemplate(string eventType);
}

public class TemplateMessage : ITemplateMessage
{
    private readonly Dictionary<string, string> _template;
    private readonly ILogger<TemplateMessage> _logger;
    public string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\template.json";
    public TemplateMessage(ILogger<TemplateMessage> logger)
    {
        _logger = logger;
        var json = FileContent(path);
        _template = GetTemplate(json);
    }

    public string FileContent(string path)
    {
        try
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            return json;
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"[json.load] Occur failed when load json file");
            throw new NotSupportedException();
        }
    }

    public Dictionary<string, string> GetTemplate(string json)
    {
        try
        {
            var dns = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return dns;
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"[json.convert] Occur failed when load json file");
            throw new NotSupportedException();
        }
    }

    public string RenderTemplate(string eventType)
    {
        if (!_template.TryGetValue(eventType, out var template))
        {
            throw new NotSupportedException("not support this message template");
        }

        return template;
    }
}
