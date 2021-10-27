using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace WNet.Core
{
    internal struct RawPacketInfo
    {
        public int Offset;
        public int Size;
    };

    internal class PacketQueue
    {
        private MemoryStream _streamBuffer;
        private List<RawPacketInfo> _rawPacketInfos;
        private int _offset;

        public PacketQueue()
        {
            _streamBuffer = new MemoryStream();
            _rawPacketInfos = new List<RawPacketInfo>();

            _offset = 0;
        }

        public int Enqueue(byte[] data, int size)
        {
            RawPacketInfo info = new RawPacketInfo { Offset = _offset, Size = size };
            _rawPacketInfos.Add(info);

            _streamBuffer.Position = _offset;
            _streamBuffer.Write(data, 0, size);
            _streamBuffer.Flush();
            _offset += size;

            return size;
        }

        public int Dequeue(ref byte[] buffer, int size)
        {
            if(_rawPacketInfos.Count <= 0)
            {
                return -1;
            }

            var info = _rawPacketInfos[0];
            int dataSize = Math.Min(size, info.Size);
            _streamBuffer.Position = info.Offset;

            int recvSize = _streamBuffer.Read(buffer, 0, dataSize);
            if (recvSize > 0)
                _rawPacketInfos.RemoveAt(0);

            if(_rawPacketInfos.Count == 0)
            {
                Clear();
                _offset = 0;
            }

            return recvSize;
        }

        public void Clear()
        {
            byte[] buffer = _streamBuffer.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);

            _streamBuffer.Position = 0;
            _streamBuffer.SetLength(0);
        }
    }
}