using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Tokens
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken(int length)
        {
            Random random = new Random();
            string charPool = "abcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder rs = new StringBuilder(charPool.Length);

            while (length > 0)
            {
                rs.Append(charPool[random.Next(charPool.Length)]);
                length--;
            }
            return rs.ToString();
        }
    }
}
