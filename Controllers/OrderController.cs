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
    public class OrderController : Controller
    {

        [HttpPost]
        public async Task<dynamic> Post([FromBody]Order ot)
        {
            var hc = Helpers.CouchDBConnect.GetClient("orders");
            var response = await hc.GetAsync("orders/"+ot.ID);
            if (response.IsSuccessStatusCode) {
               Order order = (Order) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Order));

                string json = JsonConvert.SerializeObject(ot);
                HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
                await hc.PostAsync("orders", htc);

                return 1;

            }
            
        return -1;

        }

        [HttpPost]
        [Route("CreateOrder/{_id}")]
        public async Task<int> CreateOrder([FromBody] Order ot) {

            var hc = Helpers.CouchDBConnect.GetClient("orders");
            string json = JsonConvert.SerializeObject(ot);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }



    
    }
}
