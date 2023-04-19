using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesApp.Models
{
    [Table("Contractors")]
    public class Contractor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNum { get; set; }
        public string LocalNum { get; set; }
        public string PostCode { get; set; }

    }
}
