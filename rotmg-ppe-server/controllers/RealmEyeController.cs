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

        [HttpGet("player/discord/{discordId}")]
        public async Task<IActionResult> GetUserStatus(string discordId)
        {
            var pendingPlayer = _context.PendingRealmEyeUsers.FirstOrDefault(p => p.DiscordId == discordId);
            if (pendingPlayer != null)
            {
                var verifiedPlayer = _context.RealmEyeAccounts.FirstOrDefault(r => r.DiscordId == discordId);
                if (verifiedPlayer != null)
                {
                    return Ok(new
                    {
                        success = true, verified = true, message = $"Player {verifiedPlayer.AccountName} is verified.",
                        username = verifiedPlayer.AccountName
                    });
                }
                return Ok(new
                {
                    success = true, verified = false,
                    message = $"Player {pendingPlayer.AccountName} has not finished verification.",
                    verificationCode = pendingPlayer.VerificationCode, username = pendingPlayer.AccountName
                });
            }


            return Ok(new { success = false, verified = false, message = "User has not started verification." });
        }

        [HttpGet("player/{playerName}")]
        public async Task<IActionResult> GetPlayerStatus(string playerName)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == playerName);
            if (player == null)
            {
                return NotFound(new { success = false, message = $"Player {playerName} not found.", verified = false });
            }

            var pendingPlayer = _context.PendingRealmEyeUsers.FirstOrDefault(p => p.AccountName == player.Name);
            if (pendingPlayer != null)
            {
                return Ok(new
                {
                    success = false, verified = false, message = $"Player {playerName} has not finished verification.",
                    verificationCode = pendingPlayer.VerificationCode, discordId = pendingPlayer.DiscordId
                });
            }

            var realmEyePlayer = _context.RealmEyeAccounts.FirstOrDefault(r => r.AccountName == player.Name);
            if (realmEyePlayer == null)
            {
                return BadRequest(new
                {
                    success = false, verified = false, message = $"Player {playerName} has not started verification."
                });
            }

            return Ok(new
            {
                success = true, verified = realmEyePlayer.Verified, discordId = realmEyePlayer.DiscordId,
                username = realmEyePlayer.AccountName
            });
        }

        [HttpPost("player/discord/{discordId}/{username}")]
        public async Task<IActionResult> StartVerification(string discordId, string username)
        {
            var player = _context.PendingRealmEyeUsers.FirstOrDefault(p => p.DiscordId == discordId);
            if (player != null)
                return BadRequest(new
                {
                    success = false, message = "Player has already started verification.",
                    verificationCode = player.VerificationCode,
                    verified = false
                });
            var alreadyVerified = _context.RealmEyeAccounts.FirstOrDefault(r => r.DiscordId == discordId);
            if (alreadyVerified != null)
                return BadRequest(new
                {
                    success = false, message = "Player already verified.", verified = true
                });

            var realmEyeAccount = new PendingRealmEyeUser();
            realmEyeAccount.VerificationCode = GenerateVerificationCode();
            realmEyeAccount.AccountName = username;
            realmEyeAccount.DiscordId = discordId;
            await _context.PendingRealmEyeUsers.AddAsync(realmEyeAccount);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, verificationCode = realmEyeAccount.VerificationCode, verified = false });
        }

        [HttpPost("player/{discordId}/{username}/verify")]
        public async Task<IActionResult> Verify(string discordId, string username)
        {
            var pendingRealmEyeUser = _context.PendingRealmEyeUsers.FirstOrDefault(r => r.DiscordId == discordId);
            if (pendingRealmEyeUser == null)
                return NotFound(new { success = false, message = $"Player {discordId} not found.", verified = false });
            else
            {
                var existingUser = _context.RealmEyeAccounts.FirstOrDefault(r => r.AccountName == username);
                if (existingUser != null)
                    return BadRequest(new
                    {
                        success = false, message = $"Player {username} already verified.", verified = true,
                        username = username
                    });
            }

            // fetch `http://realmeye.com/player/{name}` and check if the verification code is in the page
            var verificationCode = pendingRealmEyeUser.VerificationCode;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");
                var url = $"https://realmeye.com/player/{username}";
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                if (html.Contains(verificationCode))
                {
                    var realmEyeUser = new RealmEyeAccount();
                    realmEyeUser.AccountName = username;
                    realmEyeUser.Verified = true;
                    realmEyeUser.DiscordId = pendingRealmEyeUser.DiscordId;
                    realmEyeUser.VerificationCode = pendingRealmEyeUser.VerificationCode;
                    await _context.RealmEyeAccounts.AddAsync(realmEyeUser);
                    _context.PendingRealmEyeUsers.Remove(pendingRealmEyeUser);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = $"Player {username} verified.", username = username });
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