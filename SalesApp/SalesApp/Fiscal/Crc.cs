using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Fiscal
{
    class Crc
    {
        protected byte[] CalculateCheckSum(byte[] data)
        {
            byte[] crc_hex = new byte[2];
            string s_crc_hex;

            int crc, i;
            crc = 255;

            for (i = 0; i < data.GetLength(0); i++)
                crc ^= data[i];

            s_crc_hex = String.Format("{0:X2}", crc);

            crc_hex[0] = (byte)s_crc_hex[0];
            crc_hex[1] = (byte)s_crc_hex[1];

            return crc_hex;
        }

        public string CalcCRC(string str)
        {
            return Encoding.ASCII.GetString(CalculateCheckSum(Encoding.ASCII.GetBytes(str)));
        }     

        public string CalculateCrc(byte[] data)
        {
            int crc, i;
            crc = 255;

            for (i = 0; i < data.GetLength(0); i++)
                crc ^= data[i];

            return String.Format("{0:X2}", crc);
        }
    }
}
