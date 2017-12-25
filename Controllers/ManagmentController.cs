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
    
    public class ManagmentController : Controller

    {

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

    }
}
