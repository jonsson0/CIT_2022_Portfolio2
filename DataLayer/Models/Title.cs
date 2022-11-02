using System;
namespace DataLayer.Models
{
    public class title
    {
        public char titleID { get; set; }
        public string type { get; set; }
        public string primarytitle { get; set; }
        public string originaltitle { get; set; }
        public string isadult { get; set; }
        public char startyear { get; set; }
        public char endyear { get; set; }
        public int runtimeminutes { get; set; }
        public string poster { get; set; }
        public object plot { get; set; }
        public object MyProperty { get; set; }
        public double averagerating { get; set; }
        public int numvotes { get; set; }
    }
}

