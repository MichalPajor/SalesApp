using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Fiscal
{
    public class Packets
    {
        Crc crcMethods = new Crc();
        Packet packet = new Packet();
        public byte[] DailyReport(DateTime date)
        {
            return packet.CreatePacket($"1;{date.Year.ToString().Substring(2, 2)};{date.Month};{date.Day}#r\x0D\x0D");
        }

        public byte[] MonthlyReport(bool isFull, string day, string month, string year)
        {
            string version = isFull ? "6" : "7";
            return packet.CreatePacket($"{year.Substring(2, 2)};{month};{day};{year.Substring(2, 2)};{month};{day};{version}#o\x0D\x0D");
        }

        public byte[] PeroidReport(bool isFull, DateTime dateFrom, DateTime dateTo)
        {           
            string version = isFull ? "0" : "1";

            string yearFrom = dateFrom.Year.ToString().Substring(2,2);
            string monthFrom = dateFrom.Month.ToString();
            string dayFrom = dateFrom.Day.ToString();

            string yearTo = dateTo.Year.ToString().Substring(2, 2);
            string monthTo = dateTo.Month.ToString();
            string dayTo = dateTo.Day.ToString();

            return packet.CreatePacket($"{yearFrom};{monthFrom};{dayFrom};{yearTo};{monthTo};{dayTo};{version}#o\x0D\x0D");
        }


        public byte[] SetTaxRates(double A, double B, double C, double D, double E, double F, double G)
        {
            string a = convertTax(A);
            string b = convertTax(B);
            string c = convertTax(C);
            string d = convertTax(D);
            string e = convertTax(E);
            string f = convertTax(F);
            string g = convertTax(G);

            return packet.CreatePacket($"7$p\x0D\x0D{a}/{b}/{c}/{d}/{e}/{f}/{g}");
        }

        public byte[] StartReceipt(int positions)
        {
            return packet.CreatePacket(positions.ToString()+"$h");
        }
        public byte[] LastError()
        {
            return packet.CreatePacket("0#n");
        }

        public byte[] StartInvoice(int positions, string paymentType, int invoiceNum, int nrsys, string nip, string nabywca, bool copy, string l1, string l2, string l3 )
        {
            //string l1 = "Linia1";//
            //string l2 = "Linia2";//
            //string l3 = "Linia3";//
            //string nip = "1234567890123";//
            string wystawiajacy = "";
            string odbiorca = "";
            //string nabywca = "NabywcaLinia";//
            int isCopy = copy ? 0 : 255;
            
            List<byte> beginPacket = new List<byte>();
            List<byte> packet = new List<byte>();
            List<byte> contentPacket = new List<byte>();
            List<byte> endPacket = new List<byte>();

            beginPacket.Add(27);//ESC
            beginPacket.Add(80);//P

            endPacket.Add(27);//ESC
            endPacket.Add(92);// /

            contentPacket.AddRange(Mazovia.toMazovia($"{positions};3;1;2;1;0;{isCopy};0;0;0;0$h{invoiceNum}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{l1}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{l2}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{l3}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{nip}"));
            contentPacket.Add(13);//CR
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{paymentType}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{odbiorca}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{wystawiajacy}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{nrsys}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{nabywca}"));
            contentPacket.Add(13);//CR

            packet.AddRange(beginPacket);
            packet.AddRange(contentPacket);
            packet.AddRange(Mazovia.toMazovia(crcMethods.CalculateCrc(contentPacket.ToArray())));
            packet.AddRange(endPacket);

            return packet.ToArray();
        }

        public byte[] ReceiptLine(int lineNum, int discountType, string name, string quantity, string PTU, string value, string brutto, double discount)
        {
            List<byte> beginPacket = new List<byte>();
            List<byte> packet = new List<byte>();
            List<byte> contentPacket = new List<byte>();
            List<byte> endPacket = new List<byte>();

            beginPacket.Add(27);//ESC
            beginPacket.Add(80);//P

            endPacket.Add(27);//ESC
            endPacket.Add(92);// /

            contentPacket.AddRange(Mazovia.toMazovia($"{lineNum};{discountType}$l{name}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia(quantity));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{PTU}/{value}/{brutto}/{discount.ToString().Replace(",", ".")}/"));

            packet.AddRange(beginPacket);
            packet.AddRange(contentPacket);
            packet.AddRange(Mazovia.toMazovia(crcMethods.CalculateCrc(contentPacket.ToArray())));
            packet.AddRange(endPacket);

            return packet.ToArray();
        }

        public byte[] InvoiceEnd(double payIn, double sum)
        {
            string kupujacy = "";
            string sprzedajacy = "";

            List<byte> beginPacket = new List<byte>();
            List<byte> packet = new List<byte>();
            List<byte> contentPacket = new List<byte>();
            List<byte> endPacket = new List<byte>();

            beginPacket.Add(27);//ESC
            beginPacket.Add(80);//P

            endPacket.Add(27);//ESC
            endPacket.Add(92);// /

            contentPacket.AddRange(Mazovia.toMazovia($"1;0;0;0;0;1;0;0;0$eAPP"));
            contentPacket.Add(13);//CR
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{kupujacy}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{sprzedajacy}"));
            contentPacket.Add(13);//CR
            contentPacket.AddRange(Mazovia.toMazovia($"{payIn.ToString().Replace(",", ".")}/{sum.ToString().Replace(",", ".")}/0/"));

            packet.AddRange(beginPacket);
            packet.AddRange(contentPacket);
            packet.AddRange(Mazovia.toMazovia(crcMethods.CalculateCrc(contentPacket.ToArray())));
            packet.AddRange(endPacket);

            return packet.ToArray();
        }

        public byte[] ReceiptEnd(double payIn, double sum)
        {

            return packet.CreatePacket($"1$e\x0D{payIn.ToString().Replace(",", ".")}/{sum.ToString().Replace(",", ".")}/");
        }
        public byte[] ReceiptCancel()
        {
            return packet.CreatePacket("0$e\x0D\x0D");
        }

        public byte[] EndReceiptForm2(double sum)
        {
            return packet.CreatePacket($"0;0;1;0;0;0;0;0;1;0;0;1;0$y\x0D\x0D\x0D\x0D{sum.ToString().Replace(",", ".")}/{sum.ToString().Replace(", ", ".")}/10/2/{sum.ToString().Replace(", ", ".")}/0/");
        }

        private string convertTax(double tax)
        {
            string taxConverted = tax.ToString();
            taxConverted = taxConverted.Replace(",", ".");
            return taxConverted;
        }       
    }
}
