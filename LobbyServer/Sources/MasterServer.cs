using System;
using System.Collections.Generic;
using System.Threading;
using Telepathy;
using MessagePack;
using MasterServer.Packets;
using System.Linq;

namespace MasterServer.Sources
{
    public class GameServer
    {
        public GameServerData info;
    }
    
    public class MasterServer
    {
        Server server;

        public Dictionary<int, GameServerData> gameServers;

        public Dictionary<string, ClientAccountData> clients;

        public MasterServer()
        {
            ServerConfig.Load();

            Start(ServerConfig.Port);
        }

        private void Start(int port)
        {
            PacketHandler.RegisterHandlers(this);

            gameServers = new Dictionary<int, GameServerData>();
            clients = new Dictionary<string, ClientAccountData>();

            server = new Server();
            server.Start(port);

            int serverFrequency = 5;

            while (true)
            {
                Message msg;
                while (server.GetNextMessage(out msg))
                {
                    switch (msg.eventType)
                    {
                        case Telepathy.EventType.Connected:
                            {
                                Console.WriteLine($"[{msg.connectionId}] Connected");
                            }
                            break;
                        case Telepathy.EventType.Data:
                            {
                                var netMessage = MessagePackSerializer.Deserialize<NetMessage>(msg.data);
                                OnNetMessageRecieved(msg.connectionId, netMessage);
                            }
                            break;
                        case Telepathy.EventType.Disconnected:
                            {
                                OnDisconnected(msg.connectionId);
                            }
                            break;
                    }
                }

                Thread.Sleep(1000 / serverFrequency);
            }
        }


        void OnNetMessageRecieved(int connectionId, NetMessage netMessage)
        {
            var client = new NetConnection() { connectionId = connectionId };

            PacketHandler.handlers[netMessage.opCode].Invoke(client, netMessage.data);
        }

        void OnDisconnected(int connectionId)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            if (!gameServers.ContainsKey(connectionId))
            {
                Console.WriteLine($"[{connectionId}] Disconnected");
            }
            else
            {
                Console.WriteLine($"Game Server [{connectionId}] Disconnected: IP {gameServers[connectionId].ipAddress} | Port {gameServers[connectionId].port} | Name {gameServers[connectionId].name}");

                gameServers.Remove(connectionId);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        void SendMessage<T>(int connectionId, OpCodes opCode, T data)
        {
            NetMessage netMessage = new NetMessage();

            netMessage.opCode = opCode;
            netMessage.data = MessagePackSerializer.Serialize(data);

            var bytes = MessagePackSerializer.Serialize(netMessage);

            server.Send(connectionId, bytes);
        }

        [PacketHandler(OpCodes.PlayerConnected)]
        public void OnPlayerConnected(NetConnection connection, ClientAccountData player)
        {
            Console.WriteLine($"Player - {player.Nickname} connected!");

            clients.Add(player.Token, player);

            if(gameServers.ContainsKey(connection.connectionId))
                gameServers[connection.connectionId].playersCount++;
        }

        [PacketHandler(OpCodes.UpdatePlayerData)]
        public void OnUpdatePlayerData(NetConnection connection, ClientAccountData player)
        {
            if (clients.ContainsKey(player.Token))
                clients[player.Token] = player;
        }

        [PacketHandler(OpCodes.PlayerDisconnected)]
        public void OnPlayerDisconnected(NetConnection connection, ClientAccountData player)
        {
            Console.WriteLine($"Player - {player.Nickname} disconnected!");

            if(clients.ContainsKey(player.Token))
                clients.Remove(player.Token);

            if (gameServers.ContainsKey(connection.connectionId))
                gameServers[connection.connectionId].playersCount--;
        }

        [PacketHandler(OpCodes.ASK_RequestListServer)]
        public void OnRequestListServer(NetConnection connection, ClientAccountData playerAccount)
        {
            Console.WriteLine($"Player: {playerAccount.Nickname} requesting for Servers list.");

            var serverList = new GameServerList() { gameServers = new List<GameServerData>(gameServers.Values) };

            SendMessage(connection.connectionId, OpCodes.ANS_ServersList, serverList);
        }


        [PacketHandler(OpCodes.RegisterGameServer)]
        public void RegisterGameServer(NetConnection connection, GameServerData gameServer)
        {
            gameServers.Add(connection.connectionId, gameServer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Game Server Registered: IP {gameServer.ipAddress} | Port {gameServer.port} | Name {gameServer.name}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        [PacketHandler(OpCodes.UpdateGameServerData)]
        public void OnUpdateStateServer(NetConnection connection, GameServerData gameServer)
        {
            gameServers[connection.connectionId] = gameServer;
        }

        [PacketHandler(OpCodes.RemoveGameServer)]
        public void OnRemoveGameServer(NetConnection connection, GameServerData gameServer)
        {
            gameServers.Remove(connection.connectionId);
        }
    }
}
