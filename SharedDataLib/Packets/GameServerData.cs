using MessagePack;

namespace MasterServer.Packets
{
    [MessagePackObject]
    public class GameServerData
    {
        [Key(0)]
        public string ipAddress;
        [Key(1)]
        public int port;
        [Key(2)]
        public string name;
        [Key(3)]
        public string mode;
        [Key(4)]
        public int playersCount;
        [Key(5)]
        public int maxPlayersCount;
    }
}
