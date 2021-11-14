using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace WNet.Core
{
    // tcp, udp, quic.... �޽����� �������̾����...
    // udp�� �Ѽ�Ĺ���� ������ ���� ���� �� ����. �������� �˾ƾ� ��.
    // id �� �޽����� �־�� �� ��?
    // �ñ������δ� �޽����� �����ؼ� ȹ���� �� �ֵ��� ����ϴ� ��
    // thread safe �ؾ� ��
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
            //�޽��� Ÿ�Կ� ���� body ó��

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