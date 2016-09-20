using System.Runtime.InteropServices;

namespace Polaris.Lib.Utility
{
	public static class Structure
    {
        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            handle.Free();

            return stuff;
        }
    }
}
