using HandlebarsDotNet;
using Notification.Templates.Model;
using System.Text.Json;

namespace Notification.Extensions;

public static class TemplateExtensions
{
    public static string RenderMessage(Template template, string dataSchema)
    {
        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(dataSchema);
        var compiledTemplate = Handlebars.Compile(template.Content);
        return compiledTemplate(data);
    }
}
