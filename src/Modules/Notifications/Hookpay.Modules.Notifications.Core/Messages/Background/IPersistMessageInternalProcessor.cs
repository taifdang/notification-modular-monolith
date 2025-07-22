using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;

namespace Hookpay.Modules.Notifications.Core.Messages.Background;

public interface IPersistMessageInternalProcessor
{
    Task ProcessAllAsync(CancellationToken cancellationToken = default);

    Task ProcessAsync(int messsageId, MessageType messageType, CancellationToken cancellationToken = default);

    Task ChangeMessageStatusAsync(Message message, CancellationToken cancellationToken = default);
}
