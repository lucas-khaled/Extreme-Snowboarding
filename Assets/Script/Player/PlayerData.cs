using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Player
{
    public class PlayerData 
    {
        public string[] playerMeshes;
        public Color color1;
        public Color color2;
        
        private const int COLOR2BYTE_SIZE = 12;
        private const int STRING2BYTE_SIZE = 1;

        public PlayerData(Color color1, Color color2, string[] meshes)
        {
            this.color1 = color1;
            this.color2 = color2;
            playerMeshes = meshes;
        }
        
        public static object Deserialize(byte[] data)
        {
            List<byte> bytesList = new List<byte>(data);
            
            Color color1 = SerializeUtilities.Byte2Color(bytesList.Take(COLOR2BYTE_SIZE).ToArray());
            bytesList.RemoveRange(0,COLOR2BYTE_SIZE);

            Color color2 = SerializeUtilities.Byte2Color(bytesList.Take(COLOR2BYTE_SIZE).ToArray());
            bytesList.RemoveRange(0,COLOR2BYTE_SIZE);

            string[] names = SerializeUtilities.Byte2StringArray(bytesList.ToArray());
            
            var result = new PlayerData(color1, color2, names);
            return result;
        }

        public static byte[] Serialize(object customType)
        {
            var c = (PlayerData)customType;
            byte[] color1Byte = SerializeUtilities.Color2Byte(c.color1);
            byte[] color2Byte = SerializeUtilities.Color2Byte(c.color2);
            byte[] meshesByte = SerializeUtilities.StringArray2Byte(c.playerMeshes);

            return SerializeUtilities.Combine(color1Byte, color2Byte, meshesByte);
        }
        
    }
}
