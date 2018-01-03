

using System;

namespace CloudProject.Models
{
    public class Orders
    {

    public string _id {get; set;}
    public string _rev {get; set;}
     public string FirstName {get; set;}
     public string LastName {get; set;}
     public string PhoneNumber {get; set;}
    public string RestaurantName {get; set;}
    public int NumOfPlaces {get; set;}
     public int NumOfPeople {get; set;}
     public string Date {get; set;} //DateTime
     public string time {get; set;}
    public Boolean ValidateReservation{get; set;}

    }
}