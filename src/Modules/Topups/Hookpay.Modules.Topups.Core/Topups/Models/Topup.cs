using Hookpay.Modules.Topups.Core.Topups.Features;
using Hookpay.Shared.Core.Model;

namespace Hookpay.Modules.Topups.Core.Topups.Models
{
    public class Topup:Aggregate
    {
        public Topup() { }

        public int Id { get; set; }
        public int TransactionId { get; set; } = default!;
        public string? Source { get; set; }
        public string? Creator { get; set; }
        public decimal TransferAmount { get; set; }       

        public static Topup Create(int transId,string creator,decimal tranfer_amount)
        {
            var topup = new Topup
            {
                TransactionId = transId,
                Creator = creator,
                TransferAmount = tranfer_amount,               
            };

            var @event = new TopupCreatedDomainEvent(topup.TransactionId ,topup.Creator, topup.TransferAmount);

            topup.AddDomainEvent(@event);

            return topup;
        }



    }
}
