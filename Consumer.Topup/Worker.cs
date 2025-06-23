using Consumer.Topup.Repositories;
using InboxHandler.Services;
using ShareCommon.DTO;
using ShareCommon.Enum;
using ShareCommon.Generic;
using ShareCommon.Model;
using System.Text.Json;

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
                using var scope = _provider.CreateScope();
                var unitofwork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();               
                //get list
                var listInbox = await unitofwork.inbox.GetListInbox();              
                if (listInbox is null || listInbox.Count == 0)
                {
                    _logger.LogWarning($"[topup]:error >> not found <<");
                    await Task.Delay(200, stoppingToken);
                }
                else
                {
                    foreach (var inbox in listInbox)
                    {
                        await unitofwork.CreateAsync();
                        try
                        {
                            //add transaction_tbl
                            var trans_data = MappingData.MapData(inbox.itopup_event_type, inbox.itopup_payload!);
                            await unitofwork.trans.AddTransaction(trans_data);
                            //update user_balance
                            var user = await unitofwork.user.FindUser(trans_data.topup_creator!);
                            if (user is null)
                            {
                                _logger.LogWarning($"[topup]:error >> user not exist <<");
                                await unitofwork.RollbackAsync();
                                return;
                            }
                            await unitofwork.user.UpdateBalance(user.user_id, trans_data.topup_tranfer_amount);
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
                                    entity_id = trans_data.topup_id,//topup_tbl
                                    event_type = "topup.created",
                                    action = PushType.InWeb,//push_type
                                    user_id = user.user_id,
                                    detail = new TopupDetail//dynamic field
                                    {
                                        username = trans_data.topup_creator!,
                                        transfer_amount = trans_data.topup_tranfer_amount
                                    },
                                    priority = PriorityMessage.High
                                },
                                status = "pending"
                            };
                            var outbox_data = new OutboxTopup
                            {
                                otopup_source = "topup_service",
                                otopup_event_type = "created",
                                otopup_payload = JsonSerializer.Serialize(payload),
                                otopup_created_at = _dateTime,
                                //otopup_status = "pending",
                            };
                            await unitofwork.outbox.AddToOutBox(outbox_data);
                            //update inbox_topup updated_at
                            await unitofwork.inbox.UpdateStatus(inbox);
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
