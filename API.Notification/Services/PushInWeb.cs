using API.Notification.Hubs;

namespace API.Notification.Services
{
    public class PushInWeb
    {
        private readonly MessageHub _hub;
        public PushInWeb(MessageHub hub)
        {
            _hub = hub;
        }

    }
}
