
using System;

namespace Project.Models
{

    public class Account
    {

    public int ID {get; set;}
    public string FirstName{get;,set;}
    public string LastName{get; set;}
    public string UserName{get; set;}
    public string password{get; set;}
    public string Email{get; set:}

    }

    public class Token {
        public string _id {get; set;}
        public int ttl {get ;set;}
        
        public DateTime create {get; set;}

        public Token(){}
    }
}