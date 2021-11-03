using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace WNet.Core
{
    public enum MessageType : ushort
    {
        Login,
        JoinLobby,
        JoinedLobby,
        SendChat
    }

    public class MessageHeader : IBinarySerializable
    {
        public uint BodySize;
        public MessageType Type;
        
        public MessageHeader() { }
        public MessageHeader(uint bodySize, MessageType type)
        {
            BodySize = bodySize;
            Type = type;
        }
        public int GetBinSerializdSize() => 16;

        public void GetBytes(byte[] outBytes, int offset = 0)
        {
            Debug.Assert(outBytes != null && (outBytes.Length - offset) >= GetBinSerializdSize());

            BitConverterExtend.ToBytes(BodySize, outBytes, 0 + offset);
            BitConverterExtend.ToBytes((ushort)Type, outBytes, 4 + offset);
        }

        public static MessageHeader CreateFrom(byte[] srcBytes, int offset = 0, MessageHeader inplaceHeader = null)
        {
            Debug.Assert(srcBytes != null);

            uint bodySize = BitConverter.ToUInt32(srcBytes, 0 + offset);
            MessageType type = (MessageType)BitConverter.ToUInt16(srcBytes, 4 + offset);

            if (!Enum.IsDefined(typeof(MessageType), type))
                throw new InvalidCastException("[MessageHeader] Message type is invalid.");

            if (inplaceHeader == null)
                return new MessageHeader(bodySize, type);

            inplaceHeader.BodySize = bodySize;
            inplaceHeader.Type = type;

            return inplaceHeader;
        }
    }

    public class Message : IBinarySerializable
    {
        public MessageHeader Header { get; set; }
        public IBinarySerializable Body { get; set; }

        public int GetBinSerializdSize() => Header.GetBinSerializdSize() + Body.GetBinSerializdSize();

        public void GetBytes(byte[] outBytes, int offset = 0)
        {
            Debug.Assert(outBytes != null && (outBytes.Length - offset) >= GetBinSerializdSize());

            Header.GetBytes(outBytes, 0 + offset);
            Body.GetBytes(outBytes, Header.GetBinSerializdSize() + offset);
        }
    }
}