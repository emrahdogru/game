using FluentValidation;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Models;
using Newtonsoft.Json;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Repositories;
using War.Server.Database;

namespace War.Server.Domain
{
    /// <summary>
    /// Kullanıcı. Bir kullanıcı birden fazla <see cref="GameBoard"/> üzerinde <see cref="Player"/> olarak yer alabilir. Oturum süreçleri
    /// <see cref="User"/> üzerinden takip edilirken, kullanıcının <see cref="GameBoard"/> üzerindeki aksiyonları <see cref="Player"/> üzerinden
    /// takip edilir.
    /// </summary>
    [Db("User")]
    public class User : Entity, IUser
    {
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;


        const string passwordSalt = "6↔36Ì6-}66¦Ş×(×§}³Í£║T♫æ£ìm ´~╚Ôsdf55636";
        private IEnumerable<PasswordHistoryItem>? passwordHistory = null;
        private static readonly CultureInfo cultureEn = System.Globalization.CultureInfo.GetCultureInfo("en-us");



        public User(Languages language = Languages.Turkish)
        {
            this.Language = language;
        }

        public override string ToString()
        {
            return this.FullName;
        }


        [BsonElement]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                var parts = (value ?? "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (this.Language > 0)
                {
                    parts = parts.Select(x => x.ToLower(GetCultureInfo()))
                        .Select(x =>
                        {
                            char firstChar = x[0].ToString(GetCultureInfo()).ToUpper(GetCultureInfo())[0];
                            return $"{firstChar}{x[1..]}";
                        }).ToArray();
                }

                firstName = string.Join(" ", parts);
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (this.Language > 0 && value != null)
                    value = value.ToUpper(GetCultureInfo());

                lastName = string.Join(" ", (value ?? "").Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        public string Email
        {
            get
            {
                return (email ?? "").ToLower(cultureEn);
            }
            set
            {
                email = (value ?? "").ToLower(cultureEn);
            }
        }

        /// <summary>
        /// Kullanıcı e-posta adresi doğrulanmış mı?
        /// </summary>
        [BsonElement]
        public bool IsApproved
        {
            get
            {
                // Eğer kullanıcı gelen maildeki linke tıklayarak parolasını belirledi ise e-posta adresi onaylanmıştır.
                return !string.IsNullOrWhiteSpace(Password);
            }
        }

        /// <summary>
        /// Kullanıcı aktif mi?
        /// </summary>
        public bool IsActive { get; set; } = true;

        public Languages Language { get; set; } = Languages.Turkish;

        [BsonElement]
        private Dictionary<ObjectId, IEnumerable<Authority>> TenantAuthorizations { get; set; } = new Dictionary<ObjectId, IEnumerable<Authority>>();

        public System.Globalization.CultureInfo GetCultureInfo()
        {
            return System.Globalization.CultureInfo.GetCultureInfo(Convert.ToInt32(this.Language));
        }

        [DataType(DataType.Password)]
        [JsonIgnore]
        [BsonElement]
        private string? Password { get; set; }

        private string GetHashedPassword(string password)
        {
            return Utility.Tools.Sha256($"{password}{passwordSalt}{Id}");
        }

        public IQueryable<Player> GetPlayers()
        {
            return PlayerRepository.GetAll().Where(x => x.UserId == this.Id);
        }

        public void SetPassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentOutOfRangeException(nameof(newPassword));

            if (newPassword.Length < 8)
                throw new ArgumentOutOfRangeException(nameof(newPassword), "Parola en az 8 karakter olmalıdır.");

            if (!newPassword.Any(c => Char.IsDigit(c)) || !newPassword.Any(c => Char.IsLetter(c)))
                throw new UserException("Your password must contain least one letter and number");

            DateTime? lastPasswordChangeDate = PasswordHistory.Skip(1).DefaultIfEmpty().Max(x => x?.CreateDate);

            string hashedPassword = GetHashedPassword(newPassword);

            if (this.PasswordHistory.OrderByDescending(x => x.CreateDate).Take(3).Any(x => x.Password == hashedPassword))
                throw new UserException("Yeni parolanız, son üç parolanızdan farklı olmalı.");

            this.Password = hashedPassword;
            this.PasswordHistory = this.PasswordHistory.Append(new PasswordHistoryItem()
            {
                Password = this.Password,
                CreateDate = DateTime.UtcNow
            });
        }

        public bool IsValidPassword(string password)
        {
            return this.Password != null && this.GetHashedPassword(password).Equals(this.Password, StringComparison.Ordinal);
        }


        public string GenerateValidationKey()
        {
            return Utility.Tools.Sha256($"{this.Id}╙û♥Ωabº∩)◙-L>(╧┤◄┴╕→bè╕↔∩I♫{this.Email}");
        }

        [BsonElement]
        protected IEnumerable<PasswordHistoryItem> PasswordHistory
        {
            get
            {
                if (passwordHistory == null && this.Password != null)
                {
                    passwordHistory = new PasswordHistoryItem[] {
                        new PasswordHistoryItem(){ Password = this.Password, CreateDate = this.CreateDate }
                    };
                }

                return passwordHistory ?? [];
            }
            private set
            {
                passwordHistory = value;
            }
        }

        protected sealed class PasswordHistoryItem
        {
            [BsonElement]
            internal string Password { get; set; } = string.Empty;

            [BsonElement]
            internal DateTime CreateDate { get; set; } = DateTime.UtcNow;
        }

        public enum Authority
        {
            Player,
            Admin
        }
    }
}
