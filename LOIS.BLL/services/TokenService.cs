using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOIS.Core.Interfaces;
using LOIS.DAL;
using LOIS.DAL.Lois;
using MoreLinq;
using Token = LOIS.Core.Models.Token;

namespace LOIS.BLL.services
{
    public class TokenService : ITokenService
    {
        public Token GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;
            double expireTime;
            try
            {
                expireTime =
                    Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]);
            }
            catch (Exception)
            {
                //8 hrs
                expireTime = 1000 * 60 * 60 * 8;
            }

            if (expireTime == 0)
                expireTime = 1000 * 60 * 60 * 8;
            DateTime expiredOn = DateTime.Now.AddMilliseconds(expireTime);

            var t = new Token()
            {
                userid = userId,
                authtoken = token,
                issuedon = issuedOn,
                expireson = expiredOn
            };

            //Insert to db
            using (var db = new loisEntities1())
            {
                var t1 = db.Tokens.FirstOrDefault(x => x.UserId == userId);

                if (t1 != null)
                {
                    t1.ExpiresOn = t.expireson;
                    db.SaveChanges();

                    t.authtoken = t1.AuthToken;
                    t.issuedon = t1.IssuedOn;
                    t.tokenid = t1.TokenId;
                }
                else
                {
                    var dbToken = new DAL.Lois.Token()
                    {
                        UserId = t.userid,
                        ExpiresOn = t.expireson,
                        IssuedOn = t.issuedon,
                        AuthToken = t.authtoken
                    };
                    db.Tokens.Add(dbToken);
                    db.SaveChanges();
                }
            }

            return t;
        }

        public bool ValidateToken(string authToken)
        {
            using (var db = new loisEntities1())
            {
                var token = db.Tokens.FirstOrDefault(x => x.AuthToken == authToken && x.ExpiresOn > DateTime.Now);

                if (token != null)
                {
                    return true;
                }

                return false;
            }
        }

        public bool Kill(string tokenId)
        {
            using (var db = new loisEntities1())
            {
                var token = db.Tokens.FirstOrDefault(x => x.AuthToken == tokenId);

                if (token != null)
                {
                    db.Tokens.Remove(token);
                    db.SaveChanges();
                }
            }

            return true;
        }

        public bool DeleteByUserId(int userId)
        {
            using (var db = new loisEntities1())
            {
                var tokens = db.Tokens.Where(x => x.UserId == userId);

                db.Tokens.RemoveRange(tokens);
                db.SaveChanges();
            }

            return true;
        }
    }
}
