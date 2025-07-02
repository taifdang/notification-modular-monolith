using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Module.Sender.Services
{
    public class MessageFill
    {
        private readonly Dictionary<string, string> _template;
        private static readonly Regex _regex = new(@"\{(\w+)\}",RegexOptions.Compiled);
       
        public MessageFill()
        {
            //_template = configuration.GetSection("TemplateMessage").Get<Dictionary<string, string>>()!;
            //var data_templates = "{\r\n  \"topup.created\": \"Đơn hàng #{user_id},{username} thanh toán thành công {transfer_amount} vnd.\"\r\n}";
            var json = File.ReadAllText(@"C:\Users\taida\Backend\hook-pay\ShareCommon\template.json",Encoding.UTF8);       
            _template = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;        
        }
        public string MessageRender(string event_type,Dictionary<string, object> data)
        {
            if(!_template.TryGetValue(event_type, out var template))
            {
                return $"not support this message template";
            }
            return _regex.Replace(template, match =>
            {
                var key = match.Groups[1].Value;
                return data.TryGetValue(key, out var value) ? value?.ToString() ?? "" : $"{{{key}}}";
            });

        }
    }
}
