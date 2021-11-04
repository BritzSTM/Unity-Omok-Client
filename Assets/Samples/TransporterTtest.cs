using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class TransporterTtest : MonoBehaviour
{
    Socket s;
    byte[] buf = new byte[16];
    // Start is called before the first frame update
    void Start()
    {
        s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        s.Connect("127.0.0.1", 7001);
        var msg = new WNet.Core.MessageHeader();
        msg.BodySize = 0;
        msg.Type = WNet.Core.MessageType.JoinedLobby;
        msg.GetBytes(buf);

        s.Send(buf);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
