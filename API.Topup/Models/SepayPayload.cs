using System.ComponentModel.DataAnnotations.Schema;

namespace API.Topup.Models
{
    public record SepayPayload(
        string gateway,
        string transactionDate, 
        string? accountNumber, 
        string? subAccount, 
        string? code, 
        string? content, 
        string transferType, 
        int transferAmount,
        string? referenceCode, 
        decimal accumulated, 
        int id
    );
}
