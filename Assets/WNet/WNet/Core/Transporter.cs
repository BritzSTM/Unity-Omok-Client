using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

namespace WNet
{
    public class Transporter : SingletonMonoBehaviour<Transporter>
    {
        static public float SyncTickRate = 60; // Hz... sleep time -> 1000/SyncTickRate
        static public int BufferSize = 4096 * 2;

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

        public void On(MessageType type, UnityAction<IBinarySerializable> action)
        {
            
        }

        public void Off(MessageType type, UnityAction<IBinarySerializable> action)
        {

        }
    }
}