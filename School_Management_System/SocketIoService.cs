using School_View_Models;
using SocketIOSharp.Server;
using System.Text.Json;

namespace School_Management_System
{
    public class SocketIoService : BackgroundService
    {
        private SocketIOServer _ioServer;
        private readonly Dictionary<int, string> _userSocketMap = new();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ioServer = new SocketIOServer(new SocketIOServerOptions
            {
                AllowEIO3 = true
            });

            _ioServer.OnConnection(socket =>
            {
                Console.WriteLine($"Socket.IO Connected: {socket.Id}");

                // 🔹 Join
                socket.On("join", data =>
                {
                    int userId = int.Parse(data.ToString());
                    _userSocketMap[userId] = socket.Id;
                    socket.Set("userId", userId);
                });

                // 🔹 Send Message
                socket.On("sendMessage", data =>
                {
                    var msg = JsonSerializer.Deserialize<ChatViewModel>(data.ToString());
                    msg.CreatedDate = DateTime.UtcNow;

                    // PRIVATE CHAT
                    if (!msg.IsGroup && msg.RecieverId.HasValue)
                    {
                        if (_userSocketMap.TryGetValue(msg.RecieverId.Value, out var socketId))
                        {
                            _ioServer.GetSocket(socketId)?.Emit("receiveMessage", msg);
                        }
                    }

                    // GROUP CHAT
                    if (msg.IsGroup && msg.GroupId.HasValue)
                    {
                        _ioServer.EmitTo(msg.GroupId.Value.ToString(), "receiveMessage", msg);
                    }
                });

                // 🔹 Create Group
                socket.On("createGroup", data =>
                {
                    dynamic group = data;
                    int groupId = group.groupId;

                    socket.Join(groupId.ToString());
                    socket.Emit("groupCreated", group);
                });

                socket.On("joinGroup", data =>
                {
                    int groupId = int.Parse(data.ToString());
                    socket.Join(groupId.ToString());
                });

                socket.OnDisconnect(() =>
                {
                    int userId = socket.Get<int>("userId");
                    if (userId != 0)
                        _userSocketMap.Remove(userId);
                });
            });

            _ioServer.Start(5001);
            Console.WriteLine("✅ Socket.IO Server started on port 5001");

            return Task.CompletedTask;
        }
    }
}
