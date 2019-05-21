using MasterServer.Packets;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MasterServer.Sources
{
    public class PacketHandler
    {
        public static Dictionary<OpCodes, IPacketHandler> handlers;

        public static void RegisterHandlers(object targetClass)
        {
            handlers = new Dictionary<OpCodes, IPacketHandler>();

            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    PacketHandlerAttribute attribute = (PacketHandlerAttribute)method.GetCustomAttributes(typeof(PacketHandlerAttribute), false).SingleOrDefault();
                    if (attribute != null)
                    {
                        OpCodes opCode = attribute.OpCode;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Registered opCode handler: " + opCode.ToString());
                        Console.ForegroundColor = ConsoleColor.White;

                        
                        Type packetType = method.GetParameters()[1].ParameterType;
                        Delegate del = Delegate.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(NetConnection), packetType), targetClass, method);
                        IPacketHandler handler = (IPacketHandler)Activator.CreateInstance(typeof(PacketHandler<>).MakeGenericType(packetType), del);

                        handlers.Add(opCode, handler);
                    }
                }
            }
        }
    }

    public interface IPacketHandler
    {
        void Invoke(NetConnection connection, byte[] data);
    }

    public class PacketHandler<T> : IPacketHandler where T : class
    {
        public Action<NetConnection, T> Handler;

        public PacketHandler(Action<NetConnection, T> handler)
        {
            this.Handler = handler;
        }

        public void Invoke(NetConnection client, byte[] data)
        {
            T obj = MessagePackSerializer.Deserialize<T>(data);

            Handler.Invoke(client, obj);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PacketHandlerAttribute : Attribute
    {
        public OpCodes OpCode;

        public PacketHandlerAttribute(OpCodes OpCode)
        {
            this.OpCode = OpCode;
        }
    }

}
