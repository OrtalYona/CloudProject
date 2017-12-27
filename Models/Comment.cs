
using System;

namespace CloudProject.Models
{

    public class Comment
    {
    public string _id {get; set;}
    public string _rev {get; set;}
    public int relatedPost {get; set;}
    public string title {get; set;}
    public string writer {get; set;}
    public string webSite {get; set;}
    public string Content {get; set;}
    public string type { get; set; }
    
    }
}