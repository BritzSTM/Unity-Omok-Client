namespace WNet.Core
{
    public interface IBinarySerializable
    {
        int GetBinSerializdSize();
        void GetBytes(byte[] outBytes, int offset = 0);
    }
}