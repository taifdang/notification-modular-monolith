using Hookpay.Modules.Topups.Core.Topups.Events;
using Hookpay.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Models
{
    public class Topup:Aggregate
    {
        public Topup() { }

        public int Id { get; set; }
        public int TransactionId { get; set; } = default!;
        public string? Source { get; set; }
        public string? Creator { get; set; }
        public decimal TranferAmount { get; set; }
        //public DateTime topup_created_at { get; set; }

        public static Topup Create(int transId,string creator,decimal tranfer_amount)
        {
            var topup = new Topup
            {
                TransactionId = transId,
                Creator = creator,
                TranferAmount = tranfer_amount,
                //topup_created_at = DateTime.UtcNow,
            };
            var @event = new CreateTopupDomainEvent(topup.Creator, topup.TranferAmount);
            topup.AddDomainEvent(@event);

            return topup;
        }



    }
}
