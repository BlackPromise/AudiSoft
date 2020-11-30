using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Services.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(IConfiguration config) : base(config) { }

        [HttpGet]
        public List<Entities.Product> Get()
        {
            try
            {
                return new Logic.Product(connection).All();
            }
            catch (Exception ex) {
                this.Log(ex);
                return new List<Entities.Product>() ;
            }
        }
    }
}
