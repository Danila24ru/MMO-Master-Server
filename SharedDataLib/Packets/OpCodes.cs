using System;
using System.Collections.Generic;
using System.Text;

namespace MasterServer.Packets
{
    public enum OpCodes
    {
        ASK_LoginRequest = 10000,
        ANS_LoginResult = 10001,
        RegisterGameServer = 20000,
        UpdateGameServerData = 20001,
        RemoveGameServer = 20002,
        ASK_RequestListServer = 20003,
        ANS_ServersList = 20004,
        PlayerConnected = 30000,
        PlayerDisconnected = 30001,
        UpdatePlayerData = 30002
    }
}
