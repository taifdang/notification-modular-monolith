using InboxHandler.Services;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.DTO;
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
                var _scope = _provider.CreateScope();
                var db = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                using var transaction = await db.Database.BeginTransactionAsync();
                //var unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                //#GET INBOX
                var inboxTopup_tbl = await db.inbox_topup.Where(x => x.process_at == null).Take(10).ToListAsync(stoppingToken);
                if (inboxTopup_tbl is null) await Task.Delay(1000,stoppingToken);
                else
                {
                    foreach (var inbox in inboxTopup_tbl!)
                    {
                        try
                        {
                            //[#command]: transaction_tbl nen luu cung inbox_tbl
                            #region COMMAND
                            ////service mapping data theo event_type
                            //var inbox_data = MappingData.MapData(inbox.event_type, inbox.payload!);
                            ////unitofwork
                            //await unitOfWork.BeginTransactionAsync();
                            ////topup_tbl
                            //await unitOfWork.topup_repository.AddTopupRecord(inbox_data);
                            ////user_tbl //divide step => project
                            //await unitOfWork.user_repository.UpdateUserCoin(inbox_data);
                            #endregion
                            //mapping data                      
                            var inbox_data = MappingData.MapData(inbox.event_type, inbox.payload!);
                            //find user_id or find cache redis(bit or key_value_field)=> getid 
                            var user = await db.users.FirstOrDefaultAsync(x => x.username == inbox_data.username!.ToLower());
                            //process user null ?
                            //#TOPUP
                            Transactions topup_tbl = inbox_data;
                            db.transactions.Add(topup_tbl);
                            await db.SaveChangesAsync();//get id
                            //#UPDATE COIN (not null)
                            user!.balance += inbox_data.tranfer_amount;
                            //#OUTBOX
                            #region OUTBOX
                            var _dateTime = DateTime.Now;
                            //PAYLOAD
                            var payload = new EventMessage<Topup_Details>
                            {
                                //message_id = inbox_data.id,//id de service khac nhan
                                message_id = Guid.NewGuid(),
                                message_type = "topup.created",
                                timestamp = _dateTime,
                                source = "topup_service",
                                data = new DataPayload<Topup_Details>
                                {
                                    entity_id = topup_tbl.id,//topup_tbl
                                    status = "waiting",    
                                    action = "inapp",//push_type
                                    user_id = user!.id,
                                    detail = new Topup_Details//dynamic field
                                    {                                     
                                        username = inbox_data.username!,
                                        transfer_amount = inbox_data.tranfer_amount
                                    },
                                    priority = "high"                
                                },             
                                status = "pending"
                            };

                            var outbox_tbl = new OutboxTopup
                            {                            
                                source = "topup_service",
                                event_type = "created",
                                payload = JsonSerializer.Serialize(payload),
                                created_at = _dateTime,
                                status = "pending",
                            };
                            db.outbox_topup.Add(outbox_tbl);
                            #endregion
                            //#UPDATE INBOX
                            inbox.process_at = DateTime.Now;
                            //await unitOfWork.CommitAsync();
                            await db.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            //await unitOfWork.RollbackAsync();
                            await transaction.RollbackAsync();
                            _logger.LogError(ex, $"Error {inbox.id}");
                            //ADD ERROR RECORD
                            var _scopeError = _provider.CreateScope();
                            var dbError = _scopeError.ServiceProvider.GetRequiredService<DatabaseContext>();
                            //log error
                            var inbox_err = await db.inbox_topup.FindAsync(inbox.id);
                            inbox_err!.error = ex.ToString();
                            inbox.process_at = DateTime.Now;
                            await dbError.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
