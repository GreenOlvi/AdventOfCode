namespace AOC2021.Day16
{
    public static class BitExtenstions
    {
        public static int ToInt(this IEnumerable<byte> bits)
        {
            var num = 0;
            foreach (var bit in bits)
            {
                num = num * 2 + bit;
            }
            return num;
        }

        public static byte ToByte(this IEnumerable<byte> bits)
        {
            byte num = 0;
            foreach (var bit in bits)
            {
                num = (byte)(num * 2 + bit);
            }
            return num;
        }

        public static long ToLong(this IEnumerable<byte> bits)
        {
            var num = 0L;
            foreach (var bit in bits)
            {
                num = num * 2 + bit;
            }
            return num;
        }
    }
}
