﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBhandlingWebApp.Models
{
    public class Maxims
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contents { get; set; }
        public DateTime RegDate { get; set; }
    }
}