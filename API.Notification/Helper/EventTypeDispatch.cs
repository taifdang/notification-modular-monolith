using API.Notification.Models;
using API.Notification.Services;
using ShareCommon.Model;

namespace API.Notification.Helper
{
    public class EventTypeDispatch
    {
        private readonly Dictionary<string, IEventHandler> _handlers;
        public EventTypeDispatch(IEnumerable<IEventHandler> handlers)
        {
            //list module inheritance IEventHandler 
            _handlers = handlers.ToDictionary(x => x.event_type);
        }
        public async Task Dispatch(InboxNotification message)
        {
            //dictionary key-value
            if(_handlers.TryGetValue(message.event_type,out var handler))
            {
                await handler.HandleAsync(message);        
            }
            else
            {
                Console.WriteLine("[dispatch_error] event_type not support");
            }           
        }
    }
   
}
