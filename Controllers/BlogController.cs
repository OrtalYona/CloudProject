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
            var response = await hc.GetAsync("posts/"+p._id);
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
            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            jsonObject.Remove("_rev");
            jsonObject.Remove("_id");
            json = jsonObject.ToString();
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }

        [HttpPut]
        [Route("UpdatePost/{id}")]
        public async Task<int> UpdatePost(string id,[FromBody] Post p) {

            var hc = Helpers.CouchDBConnect.GetClient("posts");
            var getRev = await hc.GetAsync("posts/"+id);
            var post = (Post) JsonConvert.DeserializeObject(await getRev.Content.ReadAsStringAsync(),typeof(Post));
            p._rev=post._rev;
            p._id=post._id;
            string json = JsonConvert.SerializeObject(p);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PutAsync("posts/"+p._id,htc);
            Console.WriteLine(response);
            return 1;
        }

        [HttpDelete]
        [Route("DeletePost/{id}")]
        public async Task<int> DeletePost(string id)  
        {
            var hc = Helpers.CouchDBConnect.GetClient("posts");
            var response = await hc.GetAsync("posts/"+id);
            var post = (Post) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Post));
            var response1 = await hc.DeleteAsync("posts/"+post._id+"?rev="+post._rev);

            Console.WriteLine(response1);
            return 1;
        }

    }
}
