using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Models
{
    [Table("Logs", Schema = "dbo")]
    public class Log
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(200)]
        public string MachineName { get; set; }
        public DateTime Logged { get; set; }
        [MaxLength(6)]
        public string Level { get; set; }
        public string Message { get; set; }
        [MaxLength(400)]
        public string Logger { get; set; }
        public string Properties { get; set; }
        [MaxLength(400)]
        public string CallSite { get; set; }
        public string Exception { get; set; }
        [MaxLength(400)]
        public string UserInfo { get; set; }
        public string Url { get; set; }
    }
}