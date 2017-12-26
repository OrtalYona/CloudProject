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
        public async Task<dynamic> Post([FromBody]Post p)
        {
            var hc = Helpers.CouchDBConnect.GetClient("posts");
            var response = await hc.GetAsync("posts/"+p.ID);
            if (response.IsSuccessStatusCode) {
                Post posts = (Post) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Post));

                string json = JsonConvert.SerializeObject(p);
                HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
                await hc.PostAsync("posts", htc);

                return 1;

            }
            
        return -1;

        }

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

        [HttpPut]
        [Route("UpdatePost/{_id}")]
        public async Task<int> UpdatePost([FromBody] Post p) {

            var hc = Helpers.CouchDBConnect.GetClient("posts");
            string json = JsonConvert.SerializeObject(p);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PutAsync("posts/"+p.ID,htc);
            Console.WriteLine(response);
            return 1;
        }

        [HttpDelete]
        [Route("DeletePost/{_id}")]
        public async Task<int> DeleteUser(string _id)  
        {
            var hc = Helpers.CouchDBConnect.GetClient("posts");
            var response = await hc.DeleteAsync("posts/"+_id);
            Console.WriteLine(response);
            return 1;
        }

    }
}
