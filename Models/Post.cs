

using System;
using System.Collections.Generic;

namespace CloudProject.Models
{
    public class Post
    {

    public String _id {get; set;}
    public string _rev {get; set;}
    public String Title{get; set;}
    public String Writer{get; set;}
    public String WebSite{get; set;}
    public DateTime PublishDate{get; set;}
    public String Content{get; set;}
    public String cuisine{get; set;}
    public String Meals{get; set;}
    public String RFeatures{get; set;}
    public String Address{get; set;}
    public String PhoneNumber{get; set;}
    public string type { get; set; }
    public List<Comment> Comment{get; set;}
    }
}