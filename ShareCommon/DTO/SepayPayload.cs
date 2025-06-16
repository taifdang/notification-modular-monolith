using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.DTO
{
    public class SepayPayload
    {
        public string gateway { get; set; } = default!;
        public string transactionDate { get; set; } = default!;
        public string? accountNumber { get; set; } = null;
        public string? subAccount { get; set; } = null;
        public string? code { get; set; } = null;
        public string? content { get; set; } = null;
        public string transferType { get; set; } = default!;
        public string? description { get; set; } = null;
        public int transferAmount { get; set; } = default!;
        public string? referenceCode { get; set; } = null;
        [Column(TypeName = "decimal(20,2)")]
        public decimal accumulated { get; set; } = 0;
        public int id { get; set; }
    }
}
