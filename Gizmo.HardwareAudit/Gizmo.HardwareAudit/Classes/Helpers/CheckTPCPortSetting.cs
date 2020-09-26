using System;
using System.Net.Sockets;

namespace Gizmo.HardwareAudit
{

    public class CheckTPCPortSetting
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public bool IsEnabled { set; get; }
        public int Port { set; get; }
        public string Image { set; get; }
        public string LaunchPath { set; get; }
        public string LaunchArg { set; get; }

        public CheckTPCPortSetting()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
            IsEnabled = false;
            Port = 0;
            Image = string.Empty;
            LaunchPath = string.Empty;
            LaunchArg = string.Empty;
        }

        public CheckTPCPortResult CheckAsync2(string HostName, int TimeOut)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //socket.SetSocketKeepAliveValues(2000, 1000);
                IAsyncResult result = socket.BeginConnect(HostName, Port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(TimeOut, true);
                if (socket.Connected)
                {
                    socket.EndConnect(result);
                    socket.Close();
                    return new CheckTPCPortResult() { IsOpened = true, Id = Id };
                }
                else
                {
                    socket.Close();
                    return new CheckTPCPortResult() { IsOpened = false, Id = Id };
                }
            }
            catch (SocketException)
            {
                return new CheckTPCPortResult() { IsOpened = false, Id = Id };
            }
        }

        public CheckTPCPortResult CheckAsync(string HostName, int TimeOut)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(HostName, Port);
                if (socket.Connected)
                {
                    socket.Disconnect(false);
                    return new CheckTPCPortResult() { IsOpened = true, Id = Id };
                }
                else
                {
                    socket.Close();
                    return new CheckTPCPortResult() { IsOpened = false, Id = Id };
                }
            }
            catch (SocketException)
            {
                return new CheckTPCPortResult() { IsOpened = false, Id = Id };
            }
        }
    }
}
