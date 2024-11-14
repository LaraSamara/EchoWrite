using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class UserReport:Report
    {
        public string ReportedId { get; set; }

        [ForeignKey("ReportedId")]
        public ApplicationUser Reported { get; set; }

    }
}
