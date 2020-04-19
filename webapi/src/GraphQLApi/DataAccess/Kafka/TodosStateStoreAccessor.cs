using GraphQLApi.Schema.Models;
using System;
using System.Collections.Generic;

namespace GraphQLApi.DataAccess.Kafka
{
    // TODO: we should really return state store instead of using own dictionary.
    // ufortunately there is no .net library fully supporting kafka streams and 
    // kafka-stream-net (https://github.com/LGouellec/kafka-stream-net/) which we use  doesn't support it yet
    public class TodosStateStoreAccessor
    {
        public Dictionary<Guid, Todo> TodosDictionary { get; } = new Dictionary<Guid, Todo>();
    }
}
