using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudProject.Helpers;
using CloudProject.Models;
using Newtonsoft.Json;
using System.Net.Http;



namespace CloudProject.Controllers
{
    [Route("api/[controller]")]
    public class OrderTableController : Controller
    {
        [HttpPost]
        [Route("Create")]
        public async Task<int> CreateOrder([FromBody] OrderTable ot) {

            var hc = Helpers.CouchDBConnect.GetClient("users");
            string json = JsonConvert.SerializeObject(ot);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PostAsync("",htc);
            
            Console.WriteLine(response);
            return 1;
        }

        // [HttpGet]
        // [Route("Order/{id}")]
        //  public OrderTable GetOrder(string id) {
             
        //      return 
        //  }
    
    }
}
