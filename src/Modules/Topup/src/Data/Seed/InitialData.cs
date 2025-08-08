
using MassTransit;
namespace Topup.Data.Seed;

using Topup.Topups.ValueObjects;
using Topups.Models;

public static class InitialData
{
    public static List<Topup> Topups { get; }
    
    static InitialData()
    {
       Topups = new List<Topup>()
       {
           Topup.Create(
               TopupId.Of(new Guid("3fD5e9fa-BeCD-b34a-7294-804aDbE01Bca")),
               TransactionId.Of(1400000),
               TransferAmount.Of(10000),
               CreateByName.Of("user3"),
               global:: Topup.Topups.Enums.TopupStatus.InProcess)
       };  
    }
}
