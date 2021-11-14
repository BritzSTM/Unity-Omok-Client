using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace WNet.Core
{
    // tcp, udp, quic.... 메시지랑 독립적이어야함...
    // udp는 한소캣으로 여러명 것을 받을 수 있음. 누구인지 알아야 함.
    // id 가 메시지에 있어야 할 듯?
    // 궁극적으로는 메시지를 조립해서 획득할 수 있도록 대기하는 것
    // thread safe 해야 함
    internal class Transporter
    {
        private byte[] buffer;
        private Socket _sock;
     
        public void Send<T>(T value)
            where T : IBinarySerializable
        {
            _sock.Send(buffer, 0, value.GetBinSerializdSize(), SocketFlags.None);
            
        }

        public Message Receive()
        {
            int totalRecv = 0;
            int sizeToRead = 16;
            byte[] hBuf = new byte[sizeToRead];

            while(sizeToRead > 0)
            {
                byte[] buf = new byte[sizeToRead];
                int recv = _sock.Receive(buf);
                if (recv == 0)
                    return null;

                buf.CopyTo(hBuf, totalRecv);
                totalRecv += recv;
                sizeToRead -= recv;
            }

            var Header = MessageHeader.CreateFrom(hBuf);
            sizeToRead = (int)Header.BodySize;

            totalRecv = 0;
            byte[] bBuf = new byte[Header.BodySize];

            while(sizeToRead > 0)
            {
                byte[] buf = new byte[sizeToRead];
                int recv = _sock.Receive(buf);
                if (recv == 0)
                    return null;

                buf.CopyTo(bBuf, totalRecv);
                totalRecv += recv;
                sizeToRead -= recv;
            }

            IBinarySerializable body;
            //메시지 타입에 따른 body 처리

            return new Message { Header = Header, Body = null };
        }

        //public void On(MessageType type, UnityAction<IBinarySerializable> action)
        //{
            
        //}

        //public void Off(MessageType type, UnityAction<IBinarySerializable> action)
        //{

        //}
    }

    internal class TCPTransporter { }
    // Who.... message....
    internal class UDPTransporter { }
    internal class QUICTransporter { }
}