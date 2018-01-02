using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudProject.Models;
using System.Net.Http;
using Newtonsoft.Json;
using RawRabbit.Enrichers.MessageContext.Context;
using RawRabbit;


namespace CloudProject.Controllers
{
    [Route("api/[controller]")]
    
    public class OrdersController : Controller

    {
        IBusClient client;
    public OrdersController(IBusClient _client) 
    {
        client = _client;
        client.SubscribeAsync<Orders,MessageContext>(
            (order,ctx) => {
               if(order.NumOfPlaces>=order.NumOfPeople){//the order received
                   order.ValidateReservation=true;
                   
               }
               else
               {
                   order.ValidateReservation=false;
               }
                int result=order.NumOfPlaces-order.NumOfPeople;
                order.NumOfPlaces=result;  //update 

                var hc = Helpers.CouchDBConnect.GetClient("orders");
                string json = JsonConvert.SerializeObject(order);
                var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                jsonObject.Remove("_rev");
                jsonObject.Remove("_id");
                json = jsonObject.ToString();
                HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
                hc.PostAsync("",htc);

                Console.WriteLine("order FirstName: {0} LastName: {1} PhoneNumber: {2}, RestaurantName: {3}, NumOfPeople: {4},time: {5}",order.FirstName, order.LastName, order.PhoneNumber,order.RestaurantName,order.NumOfPeople,order.time);
                return Task.FromResult(0);
         }
         );     
            
    }
        // [HttpGet]
        // [Route("/init")]
        // public async void init() {
        //     await client.PublishAsync(new Orders {
        //         FirstName = "Ortal",
        //         LastName = "Yona",
        //         PhoneNumber = "0528962830",
        //         RestaurantName = "Cafe",
        //         NumOfPeople = 2,
        //         time = "20:30"

        //     });
        // }  

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
        [Route("CreateOrder")]
        public async Task<int> CreateOrder([FromBody] Orders o) {

            var hc = Helpers.CouchDBConnect.GetClient("orders");
            await client.PublishAsync(o);

            //string json = JsonConvert.SerializeObject(o);
           // var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            //jsonObject.Remove("_rev");
          //  jsonObject.Remove("_id");
         //   json = jsonObject.ToString();
           // HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
           // var response = await hc.PostAsync("",htc);
            
         //   Console.WriteLine(response);
            return 1;
        }
        
        [HttpPut]
        [Route("UpdateOrder/{id}")]
        public async Task<int> UpdateOrder(string id,[FromBody] Orders o) {

            var hc = Helpers.CouchDBConnect.GetClient("orders");
            var getRev = await hc.GetAsync("orders/"+id);
            var order = (Orders) JsonConvert.DeserializeObject(await getRev.Content.ReadAsStringAsync(),typeof(Orders));
            o._rev=order._rev;
            o._id=order._id;
            string json = JsonConvert.SerializeObject(o);
            HttpContent htc = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await hc.PutAsync("orders/"+id,htc);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return 1;
        }


        [HttpDelete]
        [Route("DeleteOrders/{id}")]  
        public async Task<int> DeleteOrders(string id)  
        {
            var hc = Helpers.CouchDBConnect.GetClient("orders");
            var response = await hc.GetAsync("orders/"+id);
            var order = (Orders) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(Orders));
            var response1 = await hc.DeleteAsync("orders/"+order._id+"?rev="+order._rev);
            Console.WriteLine(response1);
            return 1;
        }

    }
}
