using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Incyte.Entities
{
    public class Chats
    {
        public Chat[] chats;
    }
    public class Chat
    {
        public long ChatId {get;set;}

        public string Message { get; set; }

        public int FromUserId { get; set; }
        
        public int ToUserId { get; set; }
    }
}
