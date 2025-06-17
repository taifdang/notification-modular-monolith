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
        public static Topup MapData(string topup_type,string body)
        {
            try
            {
                switch (topup_type)//sepay
                {
                    //case:...
                    case "sepay":
                        var data = JsonSerializer.Deserialize<SepayPayload>(body);
                        var _user = data?.description!.Split("NAPTIEN ")[1].ToLower();
                        var transaction_tbl = new Topup//>>> DTO chua co user_id <<< ???
                        {
                            topup_trans_id = data!.id,
                            topup_creator = _user,
                            topup_source = topup_type,
                            topup_tranfer_amount = data.transferAmount,
                            topup_created_at = DateTime.Now,
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
