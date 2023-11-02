using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace GamesLibrary.Models
{
    [CollectionName("Roles")]
    public class ApplicationRole: MongoIdentityRole<Guid>
    {

    }
}