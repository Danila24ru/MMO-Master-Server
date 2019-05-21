using MessagePack;

namespace MasterServer.Packets
{
    [MessagePackObject]
    public class ClientAccountData
    {
        [Key(0)]
        public string Nickname { get; set; }
        [Key(1)]
        public string Token { get; set; }
    }
}
