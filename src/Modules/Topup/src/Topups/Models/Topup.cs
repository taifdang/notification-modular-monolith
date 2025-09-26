using BuildingBlocks.Core.Model;
using Topup.Topups.ValueObjects;

namespace Topup.Topups.Models;
public record Topup : Aggregate<TopupId>
{
    public TransactionId TransactionId { get; private set; } = default!;
    public TransferAmount TransferAmount { get; private set; } = default!;
    public CreateByName CreateByName { get; private set; } = default!;
    public Enums.TopupStatus Status { get; private set; }

    public static Topup Create(
        TopupId id,
        TransactionId transactionId,
        TransferAmount transferAmount,
        CreateByName createByName,
        Enums.TopupStatus status)
    {
        var topup = new Topup
        {
            Id = id,
            TransactionId = transactionId,
            TransferAmount = transferAmount,
            CreateByName = createByName,
            Status = status,
            IsDeleted = false,
        };


        return topup;
    }
}
