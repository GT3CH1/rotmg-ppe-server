using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using rotmg_ppe_server.data;
using rotmg_ppe_server.models;

namespace rotmg_ppe_server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Item
        [HttpGet]
        public IActionResult Index()
        {
            var list = _context.Items.ToList();
            return View(list);
        }

        [HttpGet("{name}")]
        public IActionResult GetItem(string name)
        {
            var item = FindItemByName(name);
            if (item == null)
            {
                return NotFound(new { success = false, message = "Item not found" });
            }

            return View(item);
        }


        // GET: api/Item/ring-of-unbound-health
        [HttpGet("{name}/info")]
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
            // check if item name is unique
            var foundItem = FindItemByName(item.Name);
            if (foundItem != null)
            {
                return;
            }

            _context.Items.Add(item);
            _context.SaveChanges();
        }

        // PUT: api/Item/5
        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] Item item)
        {
            var foundItem = FindItemByName(name);
            if (foundItem == null)
                return NotFound(new { success = false });
            foundItem.Worth = item.Worth;
            foundItem.ItemType = item.ItemType;
            if (item.Name != null)
                foundItem.Name = item.Name;
            _context.Items.Update(foundItem);
            _context.SaveChanges();
            return Ok(new { success = true });
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