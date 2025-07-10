using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Events;

record class CreateUserDomainEvent(string username,string email,string password, string phone):INotification;

