using System;
using ClubApi.Models.Configurations;
using Nest;

namespace ClubApi.Data
{
    public class ElasticClientFactory
    {
        public static ElasticClient CreateElasticClient(ElasticSetting setting)
        {
            var esUri = new Uri($"http://{setting.Host}:{setting.Port}");
            var settings = new ConnectionSettings(esUri);
            return new ElasticClient(settings);
        }
    }
}
