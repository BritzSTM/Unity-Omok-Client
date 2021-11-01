namespace WNet
{
    public static class BitConverterExtend
    {
        public static unsafe void ToBytes<T>(T value, byte[] array, int offset)
            where T : unmanaged
        {
            fixed (byte* ptr = &array[offset])
                *(T*)ptr = value;
        }
    }
}