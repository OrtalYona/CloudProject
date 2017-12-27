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
    
    public class OrdersController : Controller

    {

        [HttpPost]
        public async Task<dynamic> Post([FromBody]Orders o)
        {
            var hc = Helpers.CouchDBConnect.GetClient("orders");
            var response = await hc.GetAsync("orders/"+o._id);
            if (response.IsSuccessStatusCode) {
                Orders order = (Orders) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Orders));

                string json = JsonConvert.SerializeObject(o);
                HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
                await hc.PostAsync("orders", htc);

                return 1;

            }
            
        return -1;

        }

        [HttpPost]
       // [Route("CreateOrder/{_id}")]
        [Route("CreateOrder")]
        public async Task<int> CreateOrder([FromBody] Orders o) {

            var hc = Helpers.CouchDBConnect.GetClient("orders");
            string json = JsonConvert.SerializeObject(o);
            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            jsonObject.Remove("_rev");
            json = jsonObject.ToString();
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }
        
        [HttpPut]
        [Route("UpdateOrder/{id}")]
        public async Task<int> UpdateOrder(string id,[FromBody] Orders o) {

            var hc = Helpers.CouchDBConnect.GetClient("orders");
            var getRev = await hc.GetAsync("orders/"+id);
            var order = (Orders) JsonConvert.DeserializeObject(await getRev.Content.ReadAsStringAsync(),typeof(Orders));
            o._rev=order._rev;
            string json = JsonConvert.SerializeObject(o);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PutAsync("orders/"+id,htc);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return 1;
        }


        [HttpDelete]
        [Route("DeleteOrders/{id}")]
        
        //[HttpDelete("DeleteUser/{_id}")]
        public async Task<int> DeleteOrders(string id)  
        {
            var hc = Helpers.CouchDBConnect.GetClient("orders");
            var response = await hc.GetAsync("orders/"+id);
            var order = (Orders) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Orders));
          //  string json = JsonConvert.SerializeObject(a);
           // HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response1 = await hc.DeleteAsync("users/"+order._id+"?rev="+order._rev);
            Console.WriteLine(response1);
            return 1;
        }

    }
}
