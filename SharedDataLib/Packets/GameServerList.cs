using MessagePack;
using System.Collections.Generic;

namespace MasterServer.Packets
{

    [MessagePackObject]
    public class GameServerList
    {
        [Key(0)]
        public List<GameServerData> gameServers;
    }
    
}
