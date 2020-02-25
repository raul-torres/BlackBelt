using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace BlackBelt.Models
{
    public class Actividad
    {
        [Key]
        public int ActividadId{get;set;}


        [Required(ErrorMessage = "A title is required.")]
        public string Title{get;set;}


        public DateTime Tiempo{get;set;}


        [Column(TypeName="Date")]
        public DateTime Date{ get; set; }


        public int Duration{get;set;}
        public string DurationSpan{get;set;}


        [Required(ErrorMessage = "A description is required.")]
        public string Description{get;set;}


    /* -------------------------------------------------------------------------------- */
    // DATETIMEs
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;


    /* -------------------------------------------------------------------------------- */
    // RELATIONS

        public int UserId{get;set;}
        public User Creator{get;set;}
        


        public List<UserActividadRelation> Participants{get;set;}

    
    }
}