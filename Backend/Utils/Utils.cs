
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Backend.Utils
{
    public static class Utils
    {
        public static bool IsCorrectVideoId(string videoId)
        {
            return Regex.IsMatch(videoId, @"^[a-zA-Z0-9_-]{11}$");
        }
    }
}
