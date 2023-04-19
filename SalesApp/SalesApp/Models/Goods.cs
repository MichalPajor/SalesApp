using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Models
{
    [Table("Goods")]
    public class Goods
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        [ForeignKey(typeof(Units))]
        public int UnitsId { get; set; }

        [ForeignKey(typeof(TaxRates))]
        public int TaxRatesId { get; set; }

        public Goods() { }
    }
}
