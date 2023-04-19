using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Models
{
    [Table("TaxRates")]
    public class TaxRates
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; } //101 - wyłączona, 100 - zwolniona
        [OneToMany]
        public List<Goods> Goods { get; set; }

        public TaxRates() { }
    }
}
