using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressSearch.Context;
using AddressSearch.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;

namespace AddressSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly DataContext context;

        public AddressController(DataContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult<AddressEntity>> AddAddress(AddressEntity address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await this.context.Addresses.AddAsync(address);
            await this.context.SaveChangesAsync();
            return address;
        }
        [HttpGet]
        public async Task<ActionResult<SearchAddress>> SearchAddress([FromQuery] string address, string sortby = "alphabetical")
        {
            List<AddressEntity> addressList = new List<AddressEntity>();
            List<SearchAddress> addressSearchResult = new List<SearchAddress>();
            try
            {
                if (address.Length >= 3)
                {
                    var query = context.Addresses.Where(x =>
                     (x.Address.Contains(address) || x.City.Contains(address) || x.State.Contains(address)));
                    addressList = await query.ToListAsync();
                    if (addressList.Count > 0)
                    {
                        foreach (var adrs in addressList)
                        {
                            var totalfrequency = 0;
                            totalfrequency = frequency(address, adrs.Address) + frequency(address, adrs.City) * 2 + frequency(address, adrs.State) * 3;
                            var obj = new SearchAddress
                            {
                                Address = adrs.Address,
                                City = adrs.City,
                                State = adrs.State,
                                frequency = totalfrequency
                            };
                            addressSearchResult.Add(obj);


                        }
                        if (!string.IsNullOrEmpty(sortby))
                        {
                            if (sortby == "alphabetical")
                            {
                                return Ok(addressSearchResult.OrderBy(x => x.Address));
                            }
                            if (sortby == "frequency")
                            {

                                return Ok(addressSearchResult.OrderByDescending(x => x.frequency));
                            }

                        }
                    }
                    else
                    {
                        return NotFound();
                    }

                }
            }
            catch (Exception e)
            {
                return Ok(addressSearchResult);
            }

            return Ok(addressSearchResult);
        }
        private static int frequency(string text, string pattern)
        {
            int c = 0, j = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (pattern.IndexOf(text[i]) >= 0)
                {
                    c += 1;
                }
            }
            return c;

        }
    }
}
