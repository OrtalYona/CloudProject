
using System;

namespace Project.Models
{
public class Maneger
{
    public Boleean HasPassword{get; set;}
    public IList<UserLogInInfo> Login{get; set;}
    public string PhoneNumber{get; set;}
    public Boleean ToFactor{get; set;}
    public Boleean BrowserRemembered{get; set;}
    

}
}