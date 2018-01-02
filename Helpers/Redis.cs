using Microsoft.Extensions.Options;
using System;
using StackExchange.Redis;
using CloudProject;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using CloudProject.Models;


namespace CloudProject.Helpers {
    public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }

 
    public class RedisConnectionFactory : IRedisConnectionFactory
    {

        private readonly Lazy<ConnectionMultiplexer> _connection;
        

        private readonly IOptions<ConfigurationOptions> redis;

        public RedisConnectionFactory(IOptions<ConfigurationOptions> redis)
        {
            this._connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("sl-eu-lon-2-portal.8.dblayer.com:22611,password=FVNLJPTDWWVABABD"));
        }

        public ConnectionMultiplexer Connection()
        {
            return this._connection.Value;
        }
    
    }
}
    
