using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using WNet;
using log4net;

public class TransporterTest : MonoBehaviour
{
    static readonly ILog log = LogManager.GetLogger(typeof(TransporterTest));
    Socket s;
    byte[] buf = new byte[16];
    // Start is called before the first frame update
    void Start()
    {
        log.Warn("start transporter");

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
