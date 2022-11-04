using System;
namespace DataLayer.Models
{
    public class SearchHistory
    {


        public char HistId { get; set; }
        public string Username { get; set; }
        public string HistData { get; set; }
        public DateTime HistTimeStamp { get; set; }
    }
}

