using BuildingBlocks.Contracts;
using HandlebarsDotNet;
using System.Text;
using System.Text.Json;

namespace Notification.Configurations.Templates;
//ref: defalut file template.json
public class NotificationTemplate
{
    public static string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\template.json";
    public static Dictionary<string, string>? _template;

    public NotificationTemplate()
    {
        var json = ReadAllFile(filePath);
        _template = GetNotificationTemplate(json);
    }
    public static string ReadAllFile(string path)
    {
        try
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            return json;
        }
        catch (Exception ex)
        {        
            throw new Exception("Occur failed when load json file");
        }
    }
    public static Dictionary<string, string> GetNotificationTemplate(string json)
    {
        try
        {
            var dns = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return dns;
        }
        catch (Exception ex)
        {      
            throw new NotSupportedException();
        }
    }
    public static string LoadingTemplate(string eventType)
    {
        //?
        if (!_template.TryGetValue(eventType, out var template))
        {
            throw new NotSupportedException("not support this message template");
        }

        return template;
    }
    public static string RenderMessage(NotificationType notificationType, string dataSchema)
    {
        var source = LoadingTemplate(notificationType.ToString());
        //?
        if (string.IsNullOrEmpty(source))
        {
            throw new Exception("Not support template");
            //use template default
        }

        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(dataSchema);
        var template = Handlebars.Compile(source);

        return template(data);

    }
}
