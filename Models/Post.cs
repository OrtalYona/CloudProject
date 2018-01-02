

using System;
using System.Collections.Generic;

namespace CloudProject.Models
{
    public class Post
    {
    public string _id {get; set;}
    public string _rev {get; set;}
    public string Title{get; set;}
    public string Writer{get; set;}
    public string WebSite{get; set;}
    public DateTime PublishDate{get; set;}
    public string Content{get; set;}
    public string cuisine{get; set;}
    public string Meals{get; set;}
    public string RFeatures{get; set;}
    public string Address{get; set;}
    public string PhoneNumber{get; set;}
    public string type { get; set; }
    public List<Comment> Comment{get; set;}
    }
}