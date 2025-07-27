
using Hookpay.Modules.Notifications.Core.Messages.Enums;

namespace Hookpay.Modules.Notifications.Core.Messages.Models;

public class Alert
{
    public int? UserId { get; set; } //ex: userId
    public string? Target { get; set; } //ex: deviceToken/userId/Email
    public PushType PushType { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Dictionary<string, object?>? Data { get; set; } = new Dictionary<string, object?>();
    public MessagePriority Priority { get; set; }
}
