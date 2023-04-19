using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Fiscal
{
    class Packet
    {
        Crc crc = new Crc();

        public byte[] CreatePacket(string packet)
        {
            string myPacket = "\x1BP" + packet + crc.CalcCRC(packet) + "\x1B\\";
            return ConvertToByteArray(myPacket);
        }

        private byte[] ConvertToByteArray(string packet)
        {
            return Encoding.GetEncoding(1250).GetBytes(packet);
        }
    }
}
