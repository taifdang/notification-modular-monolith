using Consumer.Topup.Repositories;
using InboxHandler;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<DatabaseContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("database")));  
//
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITransRepository, TransRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInboxRepository, InboxRepository>();
//
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
