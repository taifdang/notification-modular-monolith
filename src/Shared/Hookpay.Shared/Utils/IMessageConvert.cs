using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Utils;

public interface IMessageConvert
{
    string FileContent(string path);
    Dictionary<string, string> GetTemplate(string json);
    string MessageRender(string eventType, Dictionary<string, object> data);
}
