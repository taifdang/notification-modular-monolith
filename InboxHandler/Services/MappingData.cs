using ShareCommon.DTO;
using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InboxHandler.Services
{
    public static class MappingData
    {
        //next version:>>stategy pattern
        public static Transactions MapData(string topup_type,string body)
        {
            try
            {
                switch (topup_type)//sepay
                {
                    //case:...
                    case "sepay":
                        var data = JsonSerializer.Deserialize<SepayPayload>(body);
                        var _user = data?.description!.Split("NAPTIEN ")[1].ToLower();
                        var transaction_tbl = new Transactions//>>> DTO chua co id <<< ???
                        {
                            transaction_id = data!.id,
                            username = _user,
                            source = topup_type,
                            tranfer_amount = data.transferAmount,
                            create_at = DateTime.Now,
                        };
                        return transaction_tbl;
                    default:
                        return default!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default!;
            }


        }
    }
}
