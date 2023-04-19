using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Fiscal
{
    public static class Mazovia
    {
        private static int[,] mazoviaInt = new int[,]{
            {'\u0104', 143}//�
            , {'\u0105', 134}//�
            , {'\u0106', 149}//�
            , {'\u0107', 141}//�
            , {'\u0118', 144}//�
            , {'\u0119', 145}//�
            , {'\u0141', 156}//�
            , {'\u0142', 146}//�
            , {'\u0143', 165}//�
            , {'\u0144', 164}//�
            , {'\u00D3', 163}//�
            , {'\u00F3', 162}//�
            , {'\u015A', 152}//�
            , {'\u015B', 158}//�
            , {'\u0179', 160}//�
            , {'\u017A', 166}//�
            , {'\u017B', 161}//�
            , {'\u017C', 167}//�
        };

        public static byte[] toMazovia(string text)
        {
            List<byte> list = new List<byte>();
            int charCode;

            if (text != null)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    charCode = text[i];
                    for (int j = 0; j < mazoviaInt.Length/2; j++)
                    {
                        if (charCode == mazoviaInt[j,0])
                        {
                            charCode = mazoviaInt[j,1];
                            //break;
                        }
                    }
                    list.Add((byte)charCode);
                }
            }
            byte[] data = new byte[list.Count];
            for(int i=0; i < list.Count; i++)
            {
                data[i] = list[i];
            }
            return data;
        }   
    }
}
