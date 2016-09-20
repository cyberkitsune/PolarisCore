namespace Polaris.Lib.Utility
{
	public static class HalfPrecision
    {
        public static unsafe float UIntToFloat(uint input)
        {
            float *fp = (float *)(&input);

            return *fp;
        }

        public static unsafe uint FloatToUInt(float input)
        {
            uint *ip = (uint *)(&input);

            return *ip;
        }

        public static float FloatFromHalfPrecision(ushort value)
        {
            if ((value & 0x7FFF) != 0)
            {
                uint sign = (uint)((value & 0x8000) << 16);
                uint exponent = (uint)(((value & 0x7C00) >> 10) + 0x70) << 23;
                uint mantissa = (uint)((value & 0x3FF) << 13);

                return UIntToFloat(sign | exponent | mantissa);
            }

            return 0;
        }

        public static ushort FloatToHalfPrecision(float value)
        {
            uint ivalue = FloatToUInt(value);

            if ((ivalue & 0x7FFFFFFF) != 0)
            {
                ushort sign = (ushort)((ivalue >> 16) & 0x8000);
                ushort exponent = (ushort)(((ivalue & 0x7F800000) >> 23) - 0x70);

                if ((exponent & 0xFFFFFFE0) != 0)
                    return (ushort)((exponent >> 17) ^ 0x7FFF | sign);

                ushort a = (ushort)((ivalue & 0x7FFFFF) >> 13);
                ushort b = (ushort)(exponent << 10);

                return (ushort)(a | b | sign);
            }

            return (ushort)(ivalue >> 16);
        }
    }
}
