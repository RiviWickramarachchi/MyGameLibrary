using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesLibrary.Configs
{
    public class MongoDbSettings
    {
        public string? Name { get; init; }
        public string? Host {get; init;}
        public int? Port {get; init;}

        public string ConnectionString
        {
            get
            {
                return $"mongodb://{Host}:{Port}";
            }
        }
    }
}