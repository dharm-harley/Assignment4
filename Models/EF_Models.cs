using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Assignment4.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
#nullable enable
namespace Assignment4.Models
{
    public class EF_Models
    {

        public class SignUp
        {
            [Key]

            [EmailAddress]
//server side validation for the email field.
            [Display(Name = "Email Address")]
            public string? email { get; set; }
            public string? name { get; set; }

            public string? cNumber { get; set; }

            public string? university { get; set; }

            public string? major { get; set; }

        }


        public class UniversityData
        {            
            public Metadata metadata { get; set; }
            [JsonProperty]
            public Results[] results { get; set; }
        }

        public class Metadata
        {
            
            public int total { get; set; }
            [Key]
            public int page { get; set; }
            public int per_page { get; set; }
        }


        public class Resultant { 
            public Results[] ? Res { get; set; }
        }
       
        public class Results
        {
            public string? yearstart { get; set; }
            public string? locationdesc { get; set; }
            public string? race_ethnicity { get; set; }
            public string? data_value { get; set; }
            public string? sample_size { get; set; }
            public string? yearend { get; set; }
            public string? locationabbr { get; set; }
            public string? grade { get; set; }
            public string? datasource { get; set; }
            public string? classes { get; set; }

            public string? topic { get; set; }

            public string? question { get; set; }
            public string? data_value_type { get; set; }

            public string? data_value_alt { get; set; }
            public string? low_confidence_limit { get; set; }

            public string? high_confidence_limit { get; set; }


            [JsonProperty]
            public geoloction[]? geoloc { get; set; }

            public string? classid { get; set; }
            public string? topicid { get; set; }
            public string? questionid { get; set; }
            public string? datavaluetypeid { get; set; }
            public string? stratificationcategory1 { get; set; }

            public string? locationid { get; set; }
            public string? stratificationcategoryid1 { get; set; }
            public string? stratification1 { get; set; }
            public string? stratificationid1 { get; set; }
            public int? tuitionOutState { get; set; }
            public string? schoolCity { get; set; }
            public string? schoolUrl { get; set; }
            public string? accCode { get; set; }
            public string? schoolName { get; set; }
            public int? studentSize { get; set; }


            [Key]
            public int uId { get; set; }
            public int likesCount { get; set; }

        }


        }

    public class geoloction
    {
        public long latitude { get; set; }
        public long longitude { get; set; }
        public string? human_address { get; set; }
    }
}

  

