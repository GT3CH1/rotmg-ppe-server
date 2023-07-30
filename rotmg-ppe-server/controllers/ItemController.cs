using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Newtonsoft.Json;
using rotmg_ppe_server.data;
using rotmg_ppe_server.models;

namespace rotmg_ppe_server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Item
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return _context.Items.ToList();
        }

        // GET: api/Item/ring-of-unbound-health
        [HttpGet("{name}")]
        public string Get(string name)
        {
            var item = FindItemByName(name);
            var json = JsonConvert.SerializeObject(item);
            return json;
        }

        // POST: api/Item
        [HttpPost]
        public void Post([FromBody] Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        // PUT: api/Item/5
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] Item item)
        {
            var foundItem = FindItemByName(name);
            if (foundItem != null && ItemIsValid(item))
            {
                foundItem.Name = item.Name;
                foundItem.Worth = item.Worth;
                foundItem.Soulbound = item.Soulbound;
                _context.Items.Update(foundItem);
                _context.SaveChanges();
            }
        }

        private bool ItemIsValid(Item i) => i.Name != null && i.Worth != null;

        private Item? FindItemByName(string name)
        {
            return _context.Items.Where(i => i.Name == name.ToString()).FirstOrDefault();
        }

        // DELETE: api/Item/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            var item = FindItemByName(name);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}