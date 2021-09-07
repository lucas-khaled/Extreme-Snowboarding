using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

namespace ExtremeSnowboarding
{
    public static class SerializeUtilities
    {
        public static float Byte2Float(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes, 0);
        }
        
        public static Color Byte2Color(byte[] bytes)
        {
            float r = Byte2Float(bytes.SubArray(0,4));
            float g = Byte2Float(bytes.SubArray(4,4));
            float b = Byte2Float(bytes.SubArray(8,4));

            return new Color(r, g, b);
        }

        
        public static byte[] Color2Byte(Color color)
        {
            byte[] r = BitConverter.GetBytes(color.r);
            byte[] g = BitConverter.GetBytes(color.g);
            byte[] b = BitConverter.GetBytes(color.b);

            return Combine(r, g, b);
        }

        public static string[] Byte2StringArray(byte[] bytes)
        {
            List<string> strings = new List<string>();
            List<byte> bytesList = new List<byte>(bytes);
            bytesList.RemoveAt(0);
            
            while (bytesList.Count > 1)
            {
                int nextSeparator = bytesList.FindIndex(x => Convert.ToChar(x) == '|');
                if(nextSeparator >= bytes.Length-1) break;

                string name = System.Text.Encoding.UTF8.GetString(bytesList.Take(nextSeparator).ToArray());
                strings.Add(name);
                Debug.Log("Desirialized name: "+name);
                bytesList.RemoveRange(0,nextSeparator+1);
            }

            return strings.ToArray();

        }

        public static byte[] StringArray2Byte(string[] strings)
        {
            List<byte[]> bytes = new List<byte[]>();
            byte[] separationTag = System.Text.Encoding.UTF8.GetBytes("|");
            bytes.Add(separationTag);
            foreach (var s in strings)
            {
                bytes.Add(System.Text.Encoding.UTF8.GetBytes(s));
                bytes.Add(separationTag);
            }

            return Combine(bytes.ToArray());
        }
        
        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays) {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}