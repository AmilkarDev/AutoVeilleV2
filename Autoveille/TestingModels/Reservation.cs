﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public class Reservation
    {
        public int Id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }

        public string color { get; set; }
        public bool allDay { get; set; } = false; 
    }
}