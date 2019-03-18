using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{

    [Table(name: "x4_usermeta_pivot", Schema = "x4ever")]
    public class UserMetaPivot
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }
        [Column(name: "user_id")] public Int64 user_id { get; set; }
        [Column(name: "address")] public string Address { get; set; }

        [Column(name: "age")] public string Age { get; set; }

        [Column(name: "city")] public string City { get; set; }

        [Column(name: "country")] public string Country { get; set; }
        [Column(name: "postalcode")] public string PostalCode { get; set; }
        [Column(name: "state")] public string State { get; set; }
        [Column(name: "weightlossgoal")] public string WeightLossGoal { get; set; }
        [Column(name: "gender")] public string Gender { get; set; }
        [Column(name: "profilephoto")] public string ProfilePhoto { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
    }
}