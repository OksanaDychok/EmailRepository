using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mailing_list_app.Models
{
    public class Recipient
    {
        public Recipient() { }
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public virtual List<SendDBEmail> SendDBEmails { get; set; }

        public virtual List<UserProfile> UserProfiles { get; set; }
    }
}