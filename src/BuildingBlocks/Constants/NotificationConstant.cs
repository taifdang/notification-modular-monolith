namespace BuildingBlocks.Constants;
public static class NotificationConstant
{
    public class Preferences
    {
        public Dictionary<string, List<string>> channels { get; set; } = new();
        public OptOutSetting optOut { get; set; } = new();    
    }

    public class OptOutSetting
    {
        public bool email { get; set; } = false;
        public bool sms { get; set; } = true;
        public bool push { get; set; } = false;
    }

    public static Preferences PreferencesSeed { get; }

    static NotificationConstant()
    {
       PreferencesSeed = new Preferences()
       {
           optOut = new OptOutSetting
           {
               email = true,
               sms = false,
               push = true,
           },

           channels = new Dictionary<string, List<string>>()
           {
               {Events.Promotion,[Channels.Email, Channels.Sms, Channels.Push]},
               {Events.TopupCreated,[Channels.Email, Channels.Push]}
           }
       };
    }

    public static class Channels
    {
        public const string Email = "email";
        public const string Sms = "sms";
        public const string Push = "push";
    }
    public static class Events
    {
        public const string Promotion = "promotion";
        public const string TopupCreated = "topupCreated";
    }
}
