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
            var response = await hc.GetAsync("orders/"+o.ID);
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
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }

    }
}
