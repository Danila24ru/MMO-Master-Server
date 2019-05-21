using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterServer.Packets
{
    [MessagePackObject]
    public class AuthData
    {
        [Key(0)]
        public string Token;
    }
}
