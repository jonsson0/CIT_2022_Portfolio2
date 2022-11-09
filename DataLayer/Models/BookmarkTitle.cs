﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class BookmarkTitle
    {
        public User User { get; set; }
        public User Username { get; set; }
        public Title Title { get; set; }  
        public Title Primarytitle { get; set; }
        public DateTime Timestamp { get; set; }

    }
}