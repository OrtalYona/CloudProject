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
    public class CommentController : Controller
    {

        [HttpPost]
        public async Task<dynamic> Post([FromBody]Comment c)
        {
            var hc = Helpers.CouchDBConnect.GetClient("posts");
            var response = await hc.GetAsync("posts/"+c._id);
            if (response.IsSuccessStatusCode) {
                Comment comment = (Comment) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Comment));

                string json = JsonConvert.SerializeObject(c);
                HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
                await hc.PostAsync("posts", htc);

                return 1;

            }
            
        return -1;

        }

        [HttpPost]
        [Route("CreateComment/{relatedPost}")]
        public async Task<int> CreateComment([FromBody] Comment c,string relatedPost) {

            var hc = Helpers.CouchDBConnect.GetClient("posts");
            string json = JsonConvert.SerializeObject(c);
            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            jsonObject.Remove("_rev");
          //  jsonObject.Remove("_id");
            jsonObject.Property("_id").Value = "post:"+relatedPost+":comment:"+Guid.NewGuid().ToString();
            json = jsonObject.ToString();
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }

        [HttpPut]
        [Route("UpdateComment/{id}")]
        public async Task<int> UpdateComment(string id,[FromBody] Comment c) {

            var hc = Helpers.CouchDBConnect.GetClient("posts");
            var getRev = await hc.GetAsync("posts/"+id);
            var comment = (Comment) JsonConvert.DeserializeObject(await getRev.Content.ReadAsStringAsync(),typeof(Comment));
            c._rev=comment._rev;
            c._id=comment._id;
            string json = JsonConvert.SerializeObject(c);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PutAsync("posts/"+c._id,htc);
            Console.WriteLine(response);
            return 1;
        }

        [HttpDelete]
        [Route("DeleteComment/{id}")]
        public async Task<int> DeleteComment(string id)  
        {
            var hc = Helpers.CouchDBConnect.GetClient("posts");
            var response = await hc.GetAsync("users/"+id);
            var comment = (Comment) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Comment));
            var response1 = await hc.DeleteAsync("users/"+comment._id+"?rev="+comment._rev);

            Console.WriteLine(response1);
            return 1;
        }

    }
}
