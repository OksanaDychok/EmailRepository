using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mailing_list_app.Models
{
    public class SendDBEmail
    {
        public SendDBEmail()
        {
            Recipients = new List<Recipient>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Date time")]
        public DateTime dateTime { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual List<Recipient> Recipients { get; set; }
    }

    public class SendEmail
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Body")]
        public string Body { get; set; }
    }
}