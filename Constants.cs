using System;
using System.Collections.Generic;

namespace WAChatFlow.Shared.Common
{
    public static class Constants
    {
        public static class Bytes
        {
            public const long KiB = 1024L;
            public const long MiB = 1024L * 1024L;
            public const long GiB = 1024L * 1024L * 1024L;
        }

        public static class Channels
        {
            public const string WhatsApp = "whatsapp";
            public const string Email = "email";
            public const string Sms = "sms";
        }

        public static class RecipientTypes
        {
            public const string Individual = "individual";
            public const string Group = "group";
        }
    }
}
