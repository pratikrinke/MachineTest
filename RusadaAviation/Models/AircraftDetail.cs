//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RusadaAviation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class AircraftDetail
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter Make"), MaxLength(122)]
        public string Make { get; set; }

        [Required(ErrorMessage = "Please enter Model"), MaxLength(122)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Please enter Registration")]
        [RegularExpression(@"[A-Za-z]{1,2}[-][A-Za-z]{1,5}", ErrorMessage = "Please enter a valid Registration")]
        public string Registration { get; set; }

        [Required(ErrorMessage = "Please enter Location"), MaxLength(255)]
        public string Location { get; set; }

        [DisplayName("upload image")]
        [Column(TypeName = "binary")]
        public byte[] ModelImage { get; set; }

        [NotMapped]
        public HttpPostedFileBase ModelImageFile { get; set; }

        [Required(ErrorMessage = "Please select Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }

    }
}
