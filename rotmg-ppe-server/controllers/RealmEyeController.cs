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
            var pendingPlayer = _context.RealmEyeAccounts.FirstOrDefault(p => p.DiscordId == discordId && !p.Verified);
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
            var pendingPlayer = _context.RealmEyeAccounts.FirstOrDefault(p => p.DiscordId == discordId && !p.Verified);
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

            _context.RealmEyeAccounts.Remove(pendingPlayer);
            // if there is a pending request, delete it
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
                await _context.RealmEyeAccounts.FirstOrDefaultAsync(p => p.AccountName == playerName && !p.Verified);
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
                    PendingVerification = false,
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
            var player = _context.RealmEyeAccounts.FirstOrDefault(p =>
                p.DiscordId == discordId && p.AccountName == username && !p.Verified);
            if (player != null)
                return Ok(new RealmEyeVerificationMessage
                {
                    Message = "Player has already started verification.",
                    VerificationCode = player.VerificationCode,
                    Verified = false,
                    Success = false,
                    PendingVerification = true,
                });

            player = _context.RealmEyeAccounts.FirstOrDefault(r =>
                r.DiscordId == discordId && r.AccountName == username && r.Verified);
            if (player != null)
                return BadRequest(
                    // success = false, message = "Player already verified.", verified = true
                    new RealmEyeVerificationMessage
                    {
                        Success = false,
                        Message = "Player already verified.",
                        Verified = true,
                        Username = player.AccountName,
                        VerificationCode = player.VerificationCode,
                        PendingVerification = false
                    });

            var realmEyeAccount = new RealmEyeAccount();
            realmEyeAccount.VerificationCode = GenerateVerificationCode();
            realmEyeAccount.AccountName = username;
            realmEyeAccount.DiscordId = discordId;
            realmEyeAccount.Verified = false;

            await _context.RealmEyeAccounts.AddAsync(realmEyeAccount);
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
         * player/231251672486117376/ZuccheroX/verify
         */
        [HttpPost("player/{discordId}/{username}/verify")]
        public async Task<IActionResult> Verify(string discordId, string username)
        {
            var pendingRealmEyeUser =
                _context.RealmEyeAccounts.FirstOrDefault(r =>
                    r.DiscordId == discordId && r.AccountName == username);

            if (pendingRealmEyeUser == null)
                return Ok(new RealmEyeVerificationMessage
                {
                    Success = false,
                    Message = $"{username} has not been verified or started verification.",
                    Verified = false,
                    Username = username,
                });

            var existingUser = _context.RealmEyeAccounts.FirstOrDefault(
                r => r.AccountName == username && r.DiscordId == discordId && r.Verified);

            if (existingUser != null)
                return Ok(new RealmEyeVerificationMessage
                {
                    Success = false,
                    Message = $"{username} is already verified.",
                    Verified = true,
                    Username = username,
                });

            var verificationCode = pendingRealmEyeUser.VerificationCode;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");
                var url = $"https://realmeye.com/player/{username}";
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                if (html.Contains(verificationCode))
                {
                    existingUser = _context.RealmEyeAccounts.FirstOrDefault(r =>
                        r.AccountName == username && r.DiscordId == discordId && !r.Verified);
                    existingUser.AccountName = username;
                    existingUser.Verified = true;
                    existingUser.DiscordId = pendingRealmEyeUser.DiscordId;
                    existingUser.VerificationCode = pendingRealmEyeUser.VerificationCode;
                    _context.RealmEyeAccounts.Update(existingUser);
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

        [HttpPost("player/{discordId}/{username}/forceverify")]
        public async Task<IActionResult> ForceVerify(string discordId, string username)
        {
            var player = new RealmEyeAccount
            {
                VerificationCode = GenerateVerificationCode(),
                DiscordId = discordId,
                AccountName = username,
                Verified = true
            };
            await _context.RealmEyeAccounts.AddAsync(player);
            await _context.SaveChangesAsync();
            return Ok(new RealmEyeVerificationMessage
            {
                Success = true,
                Verified = true,
                PendingVerification = false,
                Message = $"{username} verified.",
                DiscordId = discordId,
                Username = username,
                VerificationCode = player.VerificationCode
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