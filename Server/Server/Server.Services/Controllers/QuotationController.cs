using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : BaseController
    {
        public QuotationController(IConfiguration config) : base(config) { }

        // GET: api/<QuotationController>
        [HttpGet]
        public Entities.Quotation Get()
        {
            try
            {
                return new Logic.Quotation(connection).All(this.SessionValue);
            }
            catch (Exception ex)
            {
                this.Log(ex);
                return new Entities.Quotation();
            }
        }

        // POST api/<QuotationController>
        [HttpPost]
        public void Post([FromBody] Entities.Quotation value)
        {
            try
            {
                value.SessionValue = this.SessionValue;
                new Logic.Quotation(connection).InsertOrUpdate(value);
            }
            catch (Exception ex)
            {
                this.Log(ex);
            }
        }

        // DELETE api/<QuotationController>/5
        [HttpDelete]
        public void Delete()
        {
            try
            {
                new Logic.Quotation(connection).Delete(this.SessionValue);
            }
            catch (Exception ex)
            {
                this.Log(ex);
            }
        }
    }
}
