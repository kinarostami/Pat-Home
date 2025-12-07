using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Articles;

public class Article : BaseEntity
{
    public long UserId { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string MetaDescription { get; set; }
    public string Body { get; set; }
    public string Tags { get; set; }
    public string ImageName { get; set; }
    public long GroupId { get; set; }
    public long? ParentGroupId { get; set; }
    public string ShortLink { get; set; }
    public bool IsSpecial { get; set; }
    public bool IsShow { get; set; }
    public long Visit { get; set; }
    public DateTime DateReals { get; set; }

    public User User { get; set; }
}
