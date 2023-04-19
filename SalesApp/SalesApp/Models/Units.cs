using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Models
{
    [Table("Units")]
    public class Units
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortCut { get; set; }
        [OneToMany]
        public List<Goods> Goods { get; set; }

        public Units() { }
    }
}
