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

        /**
         * Checks whether or not there are any pending verification requests for a given Discord ID.
         */
        [HttpGet("player/pending/{discordId}")]
        public async Task<IActionResult> CheckPendingUserStatus(string discordId)
        {
            var pendingPlayer = _context.PendingRealmEyeUsers.FirstOrDefault(p => p.DiscordId == discordId);
            // if there is a pending request for the discord id
            if (pendingPlayer != null)
            {
                return Ok(new RealmEyeVerificationMessage
                {
                    Success = true,
                    Verified = false,
                    Message = $"Player {pendingPlayer.AccountName} has not finished verification.",
                    VerificationCode = pendingPlayer.VerificationCode,
                    Username = pendingPlayer.AccountName
                });
            }

            // if the discord id does not have any pending requests
            return Ok(new RealmEyeVerificationMessage
            {
                Success = false,
                Verified = false,
                Message = $"User with Discord ID {discordId} has not started any verification.",
                Username = null,
                VerificationCode = null
            });
        }

        /**
         * Deletes a pending verification request for a given Discord ID.
         */
        [HttpDelete("player/pending/{discordId}")]
        public async Task<IActionResult> DeletePendingUser(string discordId)
        {
            // check if there is a pending request for the discord id
            var pendingPlayer = _context.PendingRealmEyeUsers.FirstOrDefault(p => p.DiscordId == discordId);
            if (pendingPlayer == null)
            {
                return NotFound(new RealmEyeVerificationMessage
                {
                    Success = false,
                    Verified = false,
                    Message = $"User with Discord ID {discordId} has not started any verification.",
                    Username = null,
                    VerificationCode = null
                });
            }

            // if there is a pending request, delete it
            _context.PendingRealmEyeUsers.Remove(pendingPlayer);
            await _context.SaveChangesAsync();
            return Ok(new RealmEyeVerificationMessage
            {
                Success = true,
                Verified = false,
                Message = $"User with Discord ID {discordId} has been removed from the pending verification list.",
                Username = null,
                VerificationCode = null
            });
        }


        [HttpGet("player/{playerName}")]
        public async Task<IActionResult> GetPlayerStatus(string playerName)
        {
            var pendingPlayer =
                await _context.PendingRealmEyeUsers.FirstOrDefaultAsync(p => p.AccountName == playerName);
            if (pendingPlayer != null)
            {
                return Ok(new RealmEyeVerificationMessage
                {
                    Success = true,
                    Verified = false,
                    Message = $"{playerName} is pending verification.",
                    VerificationCode = pendingPlayer.VerificationCode,
                    DiscordId = pendingPlayer.DiscordId,
                    Username = pendingPlayer.AccountName,
                    PendingVerification = true
                });
            }

            var realmEyePlayer = await _context.RealmEyeAccounts.FirstOrDefaultAsync(r => r.AccountName == playerName);
            if (realmEyePlayer == null)
            {
                return BadRequest(new RealmEyeVerificationMessage
                {
                    Success = false,
                    Verified = false,
                    Message = $"{playerName} has not started any verification."
                });
            }

            return Ok(new RealmEyeVerificationMessage
            {
                Success = true,
                Verified = realmEyePlayer.Verified,
                Message = $"{playerName} has been verified.",
                DiscordId = realmEyePlayer.DiscordId,
                Username = realmEyePlayer.AccountName,
                VerificationCode = realmEyePlayer.VerificationCode,
                PendingVerification = false
            });
        }

        /**
         * Starts the verification process for a given player.
         */
        [HttpPost("player/discord/{discordId}/{username}")]
        public async Task<IActionResult> StartVerification(string discordId, string username)
        {
            var player = _context.PendingRealmEyeUsers.FirstOrDefault(p => p.DiscordId == discordId);
            if (player != null)
                return BadRequest(new RealmEyeVerificationMessage
                {
                    Message = "Player has already started verification.",
                    VerificationCode = player.VerificationCode,
                    Verified = false,
                    Success = false,
                    PendingVerification = true
                });
            var alreadyVerified = _context.RealmEyeAccounts.FirstOrDefault(r => r.DiscordId == discordId);
            if (alreadyVerified != null)
                return BadRequest(
                    // success = false, message = "Player already verified.", verified = true
                    new RealmEyeVerificationMessage
                    {
                        Success = true,
                        Message = "Player already verified.",
                        Verified = true,
                        Username = alreadyVerified.AccountName,
                        VerificationCode = alreadyVerified.VerificationCode,
                        PendingVerification = false
                    });

            var realmEyeAccount = new PendingRealmEyeUser();
            realmEyeAccount.VerificationCode = GenerateVerificationCode();
            realmEyeAccount.AccountName = username;
            realmEyeAccount.DiscordId = discordId;

            await _context.PendingRealmEyeUsers.AddAsync(realmEyeAccount);
            await _context.SaveChangesAsync();

            return Ok(new RealmEyeVerificationMessage
            {
                Success = true,
                Verified = false,
                Message = "Verification started.",
                VerificationCode = realmEyeAccount.VerificationCode,
                Username = realmEyeAccount.AccountName,
                PendingVerification = true,
                DiscordId = discordId
            });
        }

        /**
         * Downloads the player's RealmEye page and checks if the verification code is in the page.
         */
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
                    // return BadRequest(new
                    // {
                    //     success = false, message = $"Player {username} already verified.", verified = true,
                    //     username = username
                    // });
                    return Ok(new RealmEyeVerificationMessage
                    {
                        Success = false,
                        Message = $"{username} is already verified.",
                        Verified = true,
                        Username = username,
                        PendingVerification = false
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
                    return Ok(new RealmEyeVerificationMessage
                    {
                        Success = true,
                        Message = $"{username} verified.",
                        Verified = true,
                        Username = username,
                        PendingVerification = false,
                        DiscordId = pendingRealmEyeUser.DiscordId
                    });
                }
            }

            return Ok(new RealmEyeVerificationMessage
            {
                Success = false,
                Verified = false,
                PendingVerification = true,
                Message = $"Verification code {verificationCode} not found in RealmEye page.",
                DiscordId = pendingRealmEyeUser.DiscordId,
                Username = pendingRealmEyeUser.AccountName,
                VerificationCode = pendingRealmEyeUser.VerificationCode
            });
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
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = "";
            for (var i = 0; i < 10; i++)
                code += chars[random.Next(chars.Length)];
            return code;
        }
    }
}