using Hookpay.Shared.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hookpay.Shared.Utils;

public class MessageConvert:IMessageConvert
{
    private readonly Dictionary<string, string> _template;
    private readonly Regex _regex = new(@"\{(\w+)\}",RegexOptions.Compiled);
    private readonly ILogger<MessageConvert> _logger;
    public string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\template.json";
    public MessageConvert(ILogger<MessageConvert> logger)
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
        catch (Exception ex)
        {
            _logger.LogError($"[json.load] Occur failed when load json file");
            throw new NotSupportedException();
        }
    }
    public Dictionary<string, string> GetTemplate(string json)
    {
        try
        {
            var dns = JsonSerializer.Deserialize<Dictionary<string,string>>(json);
            return dns;
        }
        catch(Exception ex)
        {
            _logger.LogError($"[json.convert] Occur failed when load json file");
            throw new NotSupportedException();
        }
    } 
    public string MessageRender(string eventType,Dictionary<string,object> data)
    {
        if (!_template.TryGetValue(eventType, out var template))
        {
           throw new NotSupportedException("not support this message template");       
        }
        return _regex.Replace(template, match =>
        {
            var key = match.Groups[1].Value;
            return data.TryGetValue(key, out var value) ? value?.ToString() ?? "" : $"{{{key}}}";
        });
    }
}
