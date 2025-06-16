using Consumer.Topup.Repositories;
using InboxHandler.Services;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.DTO;
using ShareCommon.Generic;
using ShareCommon.Model;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace InboxHandler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _provider;
        public Worker(ILogger<Worker> logger,IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var scope = _provider.CreateScope();
                var unitofwork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();               
                //get list
                var listInbox = await unitofwork.inbox.GetListInbox();              
                if (listInbox is null) await Task.Delay(1000,stoppingToken);
                else
                {
                    foreach (var inbox in listInbox)
                    {
                        await unitofwork.CreateAsync();
                        try
                        {
                            //add transaction_tbl
                            var trans_data = MappingData.MapData(inbox.event_type, inbox.payload!);
                            await unitofwork.trans.AddTransaction(trans_data);
                            //update balance
                            var user = await unitofwork.user.FindUser(trans_data.username!);
                            if (user is null)
                            {
                                _logger.LogWarning($"[topup]:error >> user not exist <<");
                                await unitofwork.RollbackAsync();
                                return;
                            }
                            await unitofwork.user.UpdateBalance(user.id, trans_data.tranfer_amount);
                            //add outbox_tbl
                            var _dateTime = DateTime.Now;
                            var payload = new EventMessage<TopupDetail>
                            {                              
                                message_id = Guid.NewGuid(),
                                message_type = "topup.created",// >> detail
                                timestamp = _dateTime,
                                source = "topup_service",
                                data = new DataPayload<TopupDetail>
                                {
                                    entity_id = trans_data.id,//topup_tbl
                                    status = "waiting",
                                    action = "inapp",//push_type
                                    user_id = user.id,
                                    detail = new TopupDetail//dynamic field
                                    {
                                        username = trans_data.username!,
                                        transfer_amount = trans_data.tranfer_amount
                                    },
                                    priority = "high"
                                },
                                status = "pending"
                            };
                            var outbox_data = new OutboxTopup
                            {
                                source = "topup_service",
                                event_type = "created",
                                payload = JsonSerializer.Serialize(payload),
                                created_at = _dateTime,
                                status = "pending",
                            };
                            await unitofwork.outbox.AddToOutBox(outbox_data);
                            await unitofwork.SaveChangeAsync();
                            await unitofwork.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"[topup]:error >>{ex.ToString()}");
                            await unitofwork.RollbackAsync();
                        }                     
                    }
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
