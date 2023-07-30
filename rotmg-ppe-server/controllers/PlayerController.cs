using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rotmg_ppe_server.data;
using rotmg_ppe_server.models;

namespace rotmg_ppe_server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public List<Player> Get()
        {
            var players = _context.Players.Include(p => p.Items).ToList();
            return players;
        }

        // GET: api/Player/5
        [HttpGet("{name}")]
        public async Task<Player> Get(string name)
        {
            return GetPlayer(name);
        }

        [HttpGet("TopLivingPlayers")]
        public List<Player> GetTopLivingPlayers()
        {
            var deadPlayers = _context.Players.Include(p => p.Items).Where(p => !p.IsDead.Value).ToList();
            // sort dead players by worth
            var players = deadPlayers.OrderByDescending(p => p.GetWorth()).Take(10).ToList();
            return players;
        }

        [HttpGet("TopDeadPlayers")]
        public List<Player> GetTopDeadPlayers()
        {
            var deadPlayers = _context.Players.Include(p => p.Items).Where(p => p.IsDead.Value).ToList();
            // sort dead players by worth
            var players = deadPlayers.OrderByDescending(p => p.GetWorth()).Take(10).ToList();
            return players;
        }

        // POST: api/Player
        [HttpPost]
        public async void Post([FromBody] Player p)
        {
            if (p.Name == null)
                return;
            if (p.CharacterClass == null)
                return;

            // var newPlayer = new Player(p.Name, p.CharacterClass, null, false);
            var player = new Player
            {
                Name = p.Name,
                CharacterClass = p.CharacterClass,
                Items = new List<Item>(),
                IsDead = false,
                IsUpe = p.IsUpe.GetValueOrDefault(false)
            };
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }

        // PUT: api/Player/5
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] Player p)
        {
            var player = GetPlayer(name);
            if (player == null)
                return;
            if (p.IsDead != null)
                player.IsDead = p.IsDead;
            if (player.Items == null)
                player.Items = new List<Item>();
            if (p.Items != null)
            {
                player.Items.Clear();
                foreach (var item in p.Items)
                {
                    var lookedUpItem = _context.Items.Where(i => i.Name == item.Name).FirstOrDefault();
                    if (lookedUpItem == null)
                        continue;
                    if (player.IsUpe.GetValueOrDefault() && !lookedUpItem.Soulbound.GetValueOrDefault())
                        continue;
                    player.Items.Add(lookedUpItem);
                }
            }

            _context.Update(player);
            _context.SaveChanges();
        }

        // DELETE: api/Player/5
        [HttpDelete("{name}")]
        public async void Delete(string name)
        {
            var player = GetPlayer(name);
            if (player == null)
                return;
            _context.Remove(player);
            await _context.SaveChangesAsync();
        }

        private Player GetPlayer(string name)
        {
            // check if player exists
            if (!(_context.Players.Any(w => w.Name == name)))
                return null;
            return _context.Players.Where(p => p.Name == name && !p.IsDead.Value).FirstOrDefault();
        }
    }
}