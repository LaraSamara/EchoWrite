﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Reason {  get; set; }
        public bool IsHandled { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReporterId { get; set; }


        // Navigation properties
        [ForeignKey("ReporterId")]
        public ApplicationUser Reporter { get; set; }

    }
}