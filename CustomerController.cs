using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.Models;
using Company.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CompanyContext _context;
        public CustomerController(CompanyContext context)
        {
            _context = context;
        }
        [HttpGet]
        [ActionName("Customer/{city}")]
        public IEnumerable<Customers> Cust([FromRoute] string city)
        {
            return _context.Customers.Where(wh=>wh.City==city).ToList();
        }
        [HttpPost]
        [ActionName("select_city")]
        public async Task<IActionResult> postSelectCity([FromBody] getCity obj_city)
         {
            showCustomerData showCustomerData = new showCustomerData();
          List<Customers> obj_customer =_context.Customers.Where(wh => wh.City == obj_city.City).ToList();
            return Ok(obj_customer);
         }
        [HttpPost]
        [ActionName("selected_values")]
        public async Task<IActionResult> postSelectedValues([FromBody] showCustomerData obj_showCustomerData)
        {
            List<Customers> obj_customer= _context.Customers.Where(wh => wh.City == obj_showCustomerData.City).ToList();
            //obj_showCustomerData.Address=obj_customer.Address;
            //obj_showCustomerData.CompanyName = obj_customer.CompanyName;
            //obj_showCustomerData.ContactName = obj_customer.ContactName;
            //obj_showCustomerData.ContactTitle = obj_customer.ContactTitle;
            //obj_showCustomerData.Country = obj_customer.Country;
            //obj_showCustomerData.Phone = obj_customer.Phone;
            //obj_showCustomerData.PostalCode = obj_customer.PostalCode;
            //obj_showCustomerData.Region = obj_customer.Region;
            List<showCustomerData> custData = new List<showCustomerData>();
            foreach (Customers values in obj_customer)
            {
                showCustomerData newObj = new showCustomerData();
                newObj.Address = values.Address;
                newObj.CompanyName = values.CompanyName;
                newObj.ContactName = values.ContactName;
                newObj.ContactTitle = values.ContactTitle;
                newObj.Country = values.Country;
                newObj.Phone = values.Phone;
                newObj.PostalCode = values.PostalCode;
                newObj.Region = values.Region;
                custData.Add(newObj);
            }
            return Ok(custData);
        }
    }
}