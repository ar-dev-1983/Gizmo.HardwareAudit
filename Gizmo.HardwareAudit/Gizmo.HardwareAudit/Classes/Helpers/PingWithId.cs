using System;
using System.Net.NetworkInformation;
using System.Text;

namespace Gizmo.HardwareAudit
{
    public class PingWithId : Ping
    {
        public Guid Id { set; get; }
        public int Timeout { set; get; }
        public string NetworkIp { set; get; }
        public PingWithId()
        {
            Id = new Guid();
            Timeout = 5000;
            NetworkIp = string.Empty;
        }

        public void SendPingAsync()
        {
            this.SendAsync(NetworkIp, Timeout, Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
        }

        public IPStatus SendPing()
        {
            var result = IPStatus.Unknown;
            try { result = this.Send(NetworkIp).Status; } catch (Exception) { }
            return result;
        }
    }

}
