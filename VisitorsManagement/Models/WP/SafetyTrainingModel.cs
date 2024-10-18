using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisitorsManagement.Models.WP
{
    public class SafetyTrainingModel
    {
        public int? WPID { get; set; }
        [Required(ErrorMessage = "Safety Training is required")]
        public string SafetyTraining { get; set; }

        //[Required(ErrorMessage = "Trained By is required")]
        //[MaxLength(100, ErrorMessage = "Nature of Work should not be Greater than 100 character's.")]
        public string TrainedBy { get; set; }

        [Required(ErrorMessage = "Trained Date is required")]
        public string TrainedDate { get; set; }

        public string StatusNew { get; set; }
    }
}