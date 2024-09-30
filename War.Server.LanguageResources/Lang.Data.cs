using War.Server.LanguageResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace War.Server
{
    public partial class Lang
    {
        #region FieldNames
        public Lc FirstName = new(
            tr: "Ad",
            en: "Name"
            );

        public Lc LastName = new(
            tr: "Soyad",
            en: "Surname"
            );

        public Lc Email = new(
            tr: "E-posta",
            en: "E-mail"
            );

        public Lc Password = new(
            tr: "Parola",
            en: "Password"
            );

        public Lc ConfirmPassword = new(
            tr: "Doğrulama parolası",
            en: "Confirm Password"
            );

        public Lc ValidationKey = new (
            tr: "Doğrulama anahtarı",
            en: "Validation key"
        );

        public Lc Language = new(
            tr: "Dil",
            en: "Language"
        );

        #endregion


        #region ErrorMessages
        public L LoginFailure = new(
            tr: "Oturum açma başarısız",
            en: "Login failure"
        );

        public L DuplicateKeyError = new(
            tr: "Index çakışması hatası",
            en: "Duplicate key error"
        );

        public L StartDate = new(
            tr: "Başlangıç tarihi",
            en: "Start date"
            );

        public L EndDate = new(
            tr: "Bitiş tarihi",
            en: "End date"
            );

        /// <summary>
        /// <c>{field}</c> giriniz
        /// </summary>
        public L FieldRequired = new(
            tr: "{field} giriniz",
            en: "{field} required"
            );

        /// <summary>
        /// <c>{field}</c> geçersiz
        /// </summary>
        public L FieldInvalid = new(
            tr: "{field} geçersiz.",
            en: "Invalid {field}."
        );

        /// <summary>
        /// <c>{field}</c> en fazla <c>{maxLength}</c> uzunluğunda olabilir.
        /// </summary>
        public L FieldMaxLength = new(
            tr: "{field} en fazla {maxLength} uzunluğunda olabilir.",
            en: "{field} password."
        );

        public L PasswordsDoesntMatch = new (
            tr: "Parolalar eşleşmiyor.",
            en: "Passwords does not match."
            );

        /// <summary>
        /// Bu e-posta adresi ile kayıtlı bir kullanıcı zaten mevcut.
        /// </summary>
        public L EmailAlreadyExist = new(
            tr: "Bu e-posta adresi ile kayıtlı bir kullanıcı zaten mevcut.",
            en: "This e-mail address already exist."
        );

        public L YourPasswordMustContainAtLeastOneLetterAndOneNumber = new(
            tr: "Parolanız en az bir harf ve bir rakam içermelidir.",
            en: "Your password must contain at least one letter and one number."
        );

        public L InvalidToken = new(
            tr: "Geçersiz oturum anahtarı.",
            en: "Invalid token."
        );

        public L TokenExpired = new(
            tr: "Oturum anahtarı süresi doldu.",
            en: "Token expired."
        );

        public L UserNotAuthorizedOnThisTenant = new(
            tr: "Kullanıcı bu hesapta yetkili değil.",
            en: "The user not authorized on this game."
        );

        /// <summary>
        /// Bu doğrulama bağlantısı artık geçerli değil
        /// </summary>
        public L ThisVerificationLinkNoLongerValid = new(
            tr: "Bu doğrulama bağlantısı artık geçerli değil",
            en: "This verification link is no longer valid"
        );

        /// <summary>
        /// Varsayılan dil, erişilebilir dillerden biri olmalı
        /// </summary>
        public L DefaultLanguageMustBeFromAvailableLanguages = new(
            tr: "Varsayılan dil, erişilebilir dillerden biri olmalı.",
            en: "Default language must be from available languages."
        );

        /// <summary>
        /// {x}, {y}'den önce olamaz.
        /// </summary>
        public L XCannotBeEarlierThanY = new(
            tr: "{x}, {y}'den önce olamaz.",
            en: "{x} cannot be earlier than {y}."
            );

        public L TheTroopCannotMove = new(
            tr: "Birlik hareket edemiyor.",
            en: "The troop cannot move."
        );

        #endregion
    }
}
