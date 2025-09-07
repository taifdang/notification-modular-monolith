using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification.Data;
using HandlebarsDotNet;
using System.Text.Json;

//ref: https://www.csharptutorial.net/csharp-linq
//case: for personal notiication, we can check if user change preference after notification created -> if change, we will skip send notification
namespace Notification.PersistNotificationProcessor.Consumers.RenderingMessage;
public class RenderMessage : IConsumer<NotificationReadyToRender>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    public RenderMessage(NotificationDbContext notificationDbContext, IPublishEndpoint publishEndpoint)
    {
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationReadyToRender> context)
    {
        var @event = context.Message;

        var recipient =
            await _notificationDbContext.Recipients
                .Where(x => 
                    x.NotificationId == @event.Id && 
                    x.UserId == @event.UserId)
                .ToListAsync();

        if (!recipient.Any())
            return;

        var existingChannels = recipient.Select(x => x.Channel);
        if (!existingChannels.OrderBy(x => x).SequenceEqual(@event.channel.OrderBy(x => x)))
            return;

        foreach (var r in recipient)
        {
            //get template from db
            //case: more event type, one channel -> more template -> need to select template base on event type and channel
            //currently: one event type = notification type, one channel -> one template
            var template = await _notificationDbContext.Templates
                .SingleOrDefaultAsync(x => x.NotificationType == @event.Type && x.Channel == r.Channel);

            //check input template
            if (template is null)
                throw new Exception($"Not found template for notification type {@event.Type} and channel {r.Channel}");
            
            //render message with handlebars
            
            //map data to template
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(@event.DataSchema);
            var compiledTemplate = Handlebars.Compile(template.Content);
            var messageContent = compiledTemplate(data);

            //render message base on template with json template file
            //var messageContent = NotificationTemplate.RenderMessage(@event.Type, @event.DataSchema);

            //create object message
            var notificationMessage = new NotificationMessage(
                MessageId: Guid.NewGuid(),
                NotificationType: @event.Type,
                Channel: r.Channel,
                Recipient: new Recipient(@event.UserId, r.Target),         
                MessageContent: messageContent,
                MetaData: new Dictionary<string, object?>
                {
                    { "Priority", @event.Priority.ToString() },
                    { "Template", template.Name },
                    { "RequestId", @event.RequestId.ToString() }
                });

            await _publishEndpoint.Publish(new NotificationRendered(@event.Id, notificationMessage));
        }   
    }
}
