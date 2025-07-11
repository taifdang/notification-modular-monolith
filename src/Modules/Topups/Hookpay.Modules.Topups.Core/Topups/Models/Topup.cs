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
    public class Topup:Entity
    {
        public Topup() { }

        public int topup_id { get; set; }
        public int topup_trans_id { get; set; } = default!;
        public string? topup_source { get; set; }
        public string? topup_creator { get; set; }
        public decimal topup_tranfer_amount { get; set; }
        public DateTime topup_created_at { get; set; }

        public static Topup Create(int transId,string creator,decimal tranfer_amount)
        {
            var topup = new Topup
            {
                topup_trans_id = transId,
                topup_creator = creator,
                topup_tranfer_amount = tranfer_amount,
                topup_created_at = DateTime.UtcNow,
            };
            var @event = new CreateTopupDomainEvent(topup.topup_creator, topup.topup_tranfer_amount);
            topup.AddDomainEvent(@event);

            return topup;
        }



    }
}
