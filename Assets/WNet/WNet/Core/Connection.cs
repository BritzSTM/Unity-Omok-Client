using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace WNet.Core
{
    public class Connection
    {
        public Connection(Socket sock) { }
        public void SendMessage() { }
        public void ReciveMessage() { }
        public void On(MessageType type, Action doAction) { }
    }
}