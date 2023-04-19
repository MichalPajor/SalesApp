using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Models
{
    [Table("Sales")]
    public class Sales
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public float Value { get; set; }

        public Sales() { }
    }

}
