using System;
using System.IO;
using System.Text;

namespace ArchiveUnpacker.Utils
{
    public static class Extensions
    {
        public static string ToCString(this byte[] bytes, Encoding enc = null) => (enc ?? Encoding.UTF8).GetString(bytes).TrimEnd('\0');

        public static string ReadCString(this BinaryReader br, Encoding enc = null)
        {
            if (enc is null)
                enc = Encoding.UTF8;

            var multiByte = !enc.IsSingleByte;

            long startIdx = br.BaseStream.Position;
            for (int i = 0; br.BaseStream.Position < br.BaseStream.Length; i++) {
                if (br.ReadByte() == 0 && (multiByte && i % 2 == 0))
                    break;
            }
            long endIdx = br.BaseStream.Position;

            br.BaseStream.Position = startIdx;
            string name = enc.GetString(br.ReadBytes((int)(endIdx - startIdx - 1)));
            ++br.BaseStream.Position;

            if (multiByte)
                ++br.BaseStream.Position;

            return name;
        }

        public static int ReadInt32BE(this BinaryReader br)
        {
            var data = br.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
        
        public static uint ReadUInt32BE(this BinaryReader br)
        {
            var data = br.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }
        
        public static short ReadInt16BE(this BinaryReader br)
        {
            var data = br.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }
        
        public static ushort ReadUInt16BE(this BinaryReader br)
        {
            var data = br.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }
        
        public static long ReadInt64BE(this BinaryReader br)
        {
            var data = br.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }
        
        public static float ReadSingleBE(this BinaryReader br)
        {
            var data = br.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }
    }
}
