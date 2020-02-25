using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace BlackBelt.Models
{
    public class UserActividadRelation
    {
        [Key]
        public int UserActividadRelationId{get;set;}


        public int UserId{get;set;}
        public User User{get;set;}


        public int ActividadId{get;set;}
        public Actividad Actividad{get;set;}

    /* -------------------------------------------------------------------------------- */
    // DATETIMEs
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}