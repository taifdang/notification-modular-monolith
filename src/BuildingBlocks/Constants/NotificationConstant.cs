namespace BuildingBlocks.Constants;
public static class NotificationConstant
{
    public class Preferences
    {
        public ChannelSettings Channels { get; set; } = new();
        public EventSettings Events { get; set; } = new();
    }

    public class ChannelSettings
    {
        public bool Email { get; set; } = true;
        public bool Sms { get; set; } = false;
        public bool Push { get; set; } = true;
    }

    public class EventSettings
    {
        public bool TopupCreated { get; set; } = true;
    }

    public static Preferences PreferencesSeed { get; }

    static NotificationConstant()
    {
       PreferencesSeed = new Preferences()
       {
           Channels = new ChannelSettings
           {
               Email = true,
               Sms = false,
               Push = true,
           },

           Events = new EventSettings
           {
               TopupCreated = true,
           }
       };
    }

}
