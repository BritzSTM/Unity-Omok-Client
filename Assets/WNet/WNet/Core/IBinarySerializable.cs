namespace WNet
{
    public interface IBinarySerializable
    {
        int GetBinSerializdSize();
        void GetBytes(byte[] outBytes, int offset = 0);
    }
}