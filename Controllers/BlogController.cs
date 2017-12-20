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
    public class BlogController : Controller
    {

        [HttpPost]
        [Route("CreatePost")]
        public async Task<int> CreatePost([FromBody] Post p) {

            var hc = Helpers.CouchDBConnect.GetClient("posts");
            string json = JsonConvert.SerializeObject(p);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete([FromBody] Post p)
        {
            var hc = Helpers.CouchDBConnect.GetClient("posts");
           // string json = JsonConvert.SerializeObject(a);
           // HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
           // var response = await hc.Delete("posts/"+p._id);
        }
    }
}
