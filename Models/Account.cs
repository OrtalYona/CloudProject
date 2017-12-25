
using System;

namespace CloudProject.Models
{

    public class Account2{
         public string _id {get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string UserName{get; set;}
        public string password{get; set;}
        public string Email{get; set;}
        public bool IsAdmin{get; set;}
    }

    public class Account
    {

        public string _id {get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string UserName{get; set;}
        public string password{get; set;}
        public string Email{get; set;}
        public bool IsAdmin{get; set;}

    }

    public class Token {
        public string _id {get; set;}
        public int ttl {get ;set;}
        public DateTime create {get; set;}
        public Token(){}
    }
}