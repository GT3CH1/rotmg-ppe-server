using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rotmg_ppe_server.data;
using rotmg_ppe_server.models;

namespace rotmg_ppe_server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealmEyeController : ControllerBase
    {
        private ApplicationDbContext _context;

        public RealmEyeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("player/{playerName}")]
        public async Task<IActionResult> GetPlayerStatus(string playerName)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == playerName);
            if (player == null)
            {
                return NotFound(new { success = false, message = $"Player {playerName} not found." });
            }

            var realmEyePlayer = _context.RealmEyeAccounts.FirstOrDefault(r => r.AccountName == player.Name);
            if (realmEyePlayer == null)
            {
                return BadRequest(new
                {
                    success = false, verified = false, message = $"Player {playerName} has not started verification."
                });
            }

            return Ok(new { success = true, verified = realmEyePlayer.Verified });
        }

        [HttpPost("player/{name}")]
        public async Task<IActionResult> StartVerification(string name)
        {
            var player = _context.RealmEyeAccounts.FirstOrDefault(p => p.AccountName == name);
            if (player != null)
                return BadRequest(new
                {
                    success = false, message = "Player already starting verification.",
                    verificationCode = player.VerificationCode
                });
            var realmEyeAccount = new RealmEyeAccount();
            realmEyeAccount.VerificationCode = GenerateVerificationCode();
            realmEyeAccount.AccountName = name;
            realmEyeAccount.Verified = false;
            await _context.RealmEyeAccounts.AddAsync(realmEyeAccount);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, verificationCode = realmEyeAccount.VerificationCode });
        }

        [HttpPost("player/{name}/verify")]
        public async Task<IActionResult> Verify(string name)
        {
            var realmEyeAccount = _context.RealmEyeAccounts.FirstOrDefault(r => r.AccountName == name);
            if (realmEyeAccount == null)
                return NotFound(new { success = false, message = $"Player {name} not found." });
            if (realmEyeAccount.Verified)
                return Ok(new { success = true, message = $"Player {name} already verified." });
            // fetch `http://realmeye.com/player/{name}` and check if the verification code is in the page
            var verificationCode = realmEyeAccount.VerificationCode;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");
                var url = $"https://realmeye.com/player/{name}";
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                if (html.Contains(verificationCode))
                {
                    realmEyeAccount.Verified = true;
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = $"Player {name} verified." });
                }
            }

            return BadRequest(new
                { success = false, message = $"Verification code {verificationCode} not found in RealmEye page." });
        }

        [HttpGet("verified")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _context.RealmEyeAccounts.Where(r => r.Verified).Select(p => p.AccountName)
                .ToListAsync();
            return Ok(players);
        }


        private static string GenerateVerificationCode()
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_+=-";
            var code = "";
            for (var i = 0; i < 10; i++)
            {
                code += chars[random.Next(chars.Length)];
            }

            return code;
        }
    }
}