using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudProject.Models;
using System.Net.Http;
using Newtonsoft.Json;
using RawRabbit;
using StackExchange.Redis;
using  CloudProject.Helpers;

namespace CloudProject.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
    
        
        static Dictionary<string,Token> ActiveLogins = new Dictionary<string, Token>();
        static List<Account> Users = new List<Account>();

//--------------------------------------------------------------------------------------------
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
//------------------------------------------------------------------------------------------
        
        [HttpPost]
        public async Task<dynamic> Post([FromBody]Account a)
        {
            //LogIn
            var hc = Helpers.CouchDBConnect.GetClient("users");
            var response = await hc.GetAsync("users/"+a._id);
            if (response.IsSuccessStatusCode) {
                Account account = (Account) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Account));
                if (account.password.Equals(a.password)) {
                    Token t = new Token();
                
                    t._id = a._id+":token:"+Guid.NewGuid();
                    t.create = DateTime.Now;
                    t.ttl = 600;

                    //write the token to cache
                    cachingDB.StringSet(t._id.ToString(),JsonConvert.SerializeObject(t));
                    
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
                }
                return true;
            }

            return false;
        }


        [HttpPost]
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
            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            jsonObject.Remove("_rev");
            jsonObject.Remove("_id");
            json = jsonObject.ToString();
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return 1;
        }

        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<int> UpdateUser(string id,[FromBody] Account u) {

            var hc = Helpers.CouchDBConnect.GetClient("users");
            var getRev = await hc.GetAsync("users/"+id);
            var user = (Account) JsonConvert.DeserializeObject(await getRev.Content.ReadAsStringAsync(),typeof(Account));
            u._rev=user._rev;
            u._id=user._id;
            string json = JsonConvert.SerializeObject(u);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PutAsync("users/"+id,htc);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return 1;
        }


        [HttpDelete]
        [Route("DeleteUser/{id}")]        
        public async Task<int> DeleteUser(string id)  
        {
            var hc = Helpers.CouchDBConnect.GetClient("users");
            var response = await hc.GetAsync("users/"+id);
            var user = (Account) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Account));
            var response1 = await hc.DeleteAsync("users/"+user._id+"?rev="+user._rev);
            Console.WriteLine(response1);
            return 1;
        }

        
        IDatabase cachingDB;

        public AccountController(IRedisConnectionFactory caching) {
            cachingDB = caching.Connection().GetDatabase();
        }

        [HttpPost]
        [Route("/WriteToCache/{_id}")]
        public async Task<IEnumerable<String>> WriteToCache(string _id,[FromBody] Token a) {
            var hc = Helpers.CouchDBConnect.GetClient("users");
            var getID = await hc.GetAsync("users/"+_id);
            var user = (Account) JsonConvert.DeserializeObject(await getID.Content.ReadAsStringAsync(),typeof(Account));
            a._id=user._id;

            cachingDB.StringSet(a._id.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(a));

            return new List<string>{"ok"};
        }

        [HttpGet]
        [Route("/ReadFromCache/{id}")]
        public Account ReadFromCache(string id) {
            Account p = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(cachingDB.StringGet(id.ToString()));
            return p;
        }
    }
}
