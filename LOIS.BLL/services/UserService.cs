using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using LOIS.Core.Models;
using LOIS.DAL.Lois;

namespace LOIS.BLL.services
{
    public class UserService : IDisposable
    {
        private IMapper mapper;
        
        public UserService()
        {
            mapper = mapperConfig.CreateConfig().CreateMapper();
        }

        public User Authenticate(string email, string password)
        {
            var user = GetUser(email);

            if (user != null)
            {
                var pHash = HashPassword(password, user.salt);

                if (pHash == user.passwordhash)
                {
                    return user;
                }
            }

            return null;
        }

        public User GetUser(string emailAddress)
        {
            using (var db = new DAL.Lois.loisEntities1())
            {
                db.Database.Log = msg => Debug.WriteLine(msg);
                try
                {
                    var user = db.Authentications.FirstOrDefault(u => u.Email == emailAddress);

                    if (user == null)
                        return null;

                    var mapped = mapper.Map<User>(user);
                    mapped.Groups = user.AuthGroupUsers.Select(x => x.AuthenticationGroup.GroupName).ToList();

                    return mapped;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

        public IEnumerable<User> GetUsers()
        {
            var output = new List<User>();
            using (var db = new loisEntities1())
            {
                var users = db.Authentications.ToList();

                foreach (var user in users)
                {
                    var mapping = mapper.Map<User>(user);
                    mapping.Groups = user.AuthGroupUsers.Select(x => x.AuthenticationGroup.GroupName).ToList();
                    output.Add(mapping);
                }
            }

            return output;
        }

        public User CreateUser(string firstName, string lastName, string email, string password, bool isAdmin = false)
        {
            using (var db = new loisEntities1())
            {
                try
                {
                    var salt = GenerateSalt();
                    var pHash = HashPassword(password, salt);

                    var user = new User()
                    {
                        firstname =  firstName,
                        lastname =  lastName,
                        email =  email,
                        passwordhash = pHash,
                        salt = salt,
                        admin = isAdmin,
                        enabled = true
                    };

                    var mapping = mapper.Map<Authentication>(user);

                    if (db.Authentications.Any(x => x.Email == user.email)) return user;

                    db.Authentications.Add(mapping);

                    if (isAdmin)
                    {
                        var adminGroup = db.AuthenticationGroups.FirstOrDefault(x => x.GroupName == "Admin");

                        db.AuthGroupUsers.Add(new AuthGroupUser()
                        {
                            Authentication = mapping,
                            AuthenticationGroup = adminGroup
                        });
                    }
                    else
                    {
                        var userGroup = db.AuthenticationGroups.FirstOrDefault(x => x.GroupName == "User");
                        db.AuthGroupUsers.Add(new AuthGroupUser()
                        {
                            Authentication = mapping,
                            AuthenticationGroup = userGroup
                        });
                    }

                    db.SaveChanges();

                    return user;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public bool ChangePassword(string email, string newPassword, string oldPassword)
        {
            var user = Authenticate(email, oldPassword);

            if (user == null)
                return false;

            var newHash = HashPassword(newPassword, user.salt);
            using (var db = new loisEntities1())
            {
                var dbUser = db.Authentications.First(x => x.Email == user.email);
                dbUser.PasswordHash = newHash;
                db.SaveChanges();
            }

            return true;
        }

        public string GenerateSalt()
        {
            var guid = Guid.NewGuid();
            return guid.ToString().Replace("-", "").Substring(0, 8);
        }

        public string HashPassword(string password, string salt)
        {
            var data = Encoding.ASCII.GetBytes(salt + password);
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1Data = sha1.ComputeHash(data);
            return Convert.ToBase64String(sha1Data);
        }

        public void Dispose()
        {
        }
    }
}
