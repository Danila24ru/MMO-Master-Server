using MessagePack;

namespace MasterServer.Packets
{
    [MessagePackObject]
    public class NetMessage
    {
        [Key(0)]
        public OpCodes opCode;
        [Key(1)]
        public byte[] data;
    }
}
