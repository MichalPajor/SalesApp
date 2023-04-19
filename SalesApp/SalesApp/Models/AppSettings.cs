using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Models
{
    [Table("AppSettings")]
    public class AppSettings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public AppSettings() { }
    }
}
