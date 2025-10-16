
using HandlebarsDotNet;
using System.Text.Json;

public static class TemplateExtensions
{
    public static string RenderMessage(Notification.Domain.Entities.Template template, string metadata)
    {
        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(metadata);
        var compiledTemplate = Handlebars.Compile(template.Content);
        return compiledTemplate(data);
    }
}
