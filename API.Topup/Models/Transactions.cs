using System.ComponentModel.DataAnnotations.Schema;

namespace API.Topup.Models
{
    public class Transactions// chuan hoa du lieu
    {
        public int id { get; set; }
        public int transaction_id { get; set; }//user_id co the trung nhau
        public string? source { get; set; } // momo,vnpay
        public int user_id { get; set; }
        [Column(TypeName = "deciaml(20,2)")]
        public decimal tranfer_amount { get;set; }//10.000
        public DateTime create_at { get; set; }
    }
}
