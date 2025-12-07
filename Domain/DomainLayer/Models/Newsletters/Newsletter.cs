using System;
using System.ComponentModel.DataAnnotations.Schema;
using DomainLayer.Models.Users;

namespace DomainLayer.Models.Newsletters
{
    //خبرنامه
    public class Newsletter:BaseEntity
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public long CreatedBy { get; set; }
        public bool IsSend { get; set; }

        #region Relations
        [ForeignKey("CreatedBy")]
        public User User { get; set; }
        #endregion
    }
}