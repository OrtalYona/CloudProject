

using System;

namespace CloudProject.Models
{
    public class Orders
    {

    public string _id {get; set;}
    public string _rev {get; set;}

     public String FirstName {get; set;}
     public String LastName {get; set;}
     public String PhoneNumber {get; set;}
    public String RestaurantName {get; set;}
     public int NumOfPeople {get; set;}
     public DateTime Date {get; set;}
     public String time {get; set;}
    }
}