using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudProject.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace CloudProject.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        static Dictionary<string,Token> ActiveLogins = new Dictionary<string, Token>();
        static List<Account> Users = new List<Account>();
        // GET api/values
        [HttpGet]
        [Route("ValidateSession/{tokenId}")]

            public async Task<Boolean> ValidateSession(string tokenId) {
            var hc = Helpers.CouchDBConnect.GetClient("users");
            var response = await hc.GetAsync("/users/"+tokenId);
            if (!response.IsSuccessStatusCode)
                return false;
            
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);

            var token = (Token) JsonConvert.DeserializeObject(json,typeof(Token));

            if (token.create.AddSeconds(token.ttl).CompareTo(DateTime.Now) > 0) {
                return true;
            }

            return false;
        }
        [HttpPost]
        public async Task<dynamic> Post([FromBody]Account a)
        {

            var hc = Helpers.CouchDBConnect.GetClient("users");
            var response = await hc.GetAsync("users/"+a._id);
            if (response.IsSuccessStatusCode) {
                Account account = (Account) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Account));
                if (account.password.Equals(a.password)) {
                    Token t = new Token();
                    t._id = a._id+":token:"+Guid.NewGuid();
                    t.create = DateTime.Now;
                    t.ttl = 600;

                    HttpContent htc = new StringContent(
                        JsonConvert.SerializeObject(t),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );

                    await hc.PostAsync("users", htc);

                    return t;
                }
            };

            return -1;

        }

            async  Task<Boolean> DoesUserExist(Account a) {
            var hc = Helpers.CouchDBConnect.GetClient("users");
            var response = await hc.GetAsync("users/"+a._id);
            if (response.IsSuccessStatusCode) {
                if(response.Content != null){
                    String x = response.Content.ReadAsStringAsync().Result;
                    int t = 4;
                }
                return true;
            }

            return false;
        }


        [HttpPost]
        //[Route("CreateUser/{_id}")]
        [Route("CreateUser")]
        public async Task<int> CreateUser([FromBody] Account a) {
            if(a._id != null){
                var doesExist = await DoesUserExist(a);
                if (doesExist) {
                    return -1;
                }
            }
            
            var hc = Helpers.CouchDBConnect.GetClient("users");
            string json = JsonConvert.SerializeObject(a);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<int> UpdateUser([FromBody] Account a) {

            var hc = Helpers.CouchDBConnect.GetClient("users");
            string json = JsonConvert.SerializeObject(a);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PutAsync("users/"+a._id,htc);
            Console.WriteLine(response);
            return 1;
        }


        [HttpDelete]
        [Route("DeleteUser/{_id}")]
        
        //[HttpDelete("DeleteUser/{_id}")]
        public async Task<int> DeleteUser([FromBody] Account a)  
        {
            var hc = Helpers.CouchDBConnect.GetClient("users");
           // string json = JsonConvert.SerializeObject(a);
            //HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.DeleteAsync("users/"+a._id);
            Console.WriteLine(response);
            return 1;
        }
    }
}
