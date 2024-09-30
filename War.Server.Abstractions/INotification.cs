using War.Server.Domain;
using War.Server.Models;
using System.Linq.Expressions;
using War.Server;
using War.Server.LanguageResources;

namespace War.Server.Notifications
{
    public interface INotification<T> where T: class
    {
        /// <summary>
        /// Bildirimin gönderileceği kişi. <c>User</c> ve <c>Employee</c> olabilir.
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Bildirim dili. Varsayılan olarak <para>Person</para> nesnesinden alınır. Sonrasında değiştirilebilir.
        /// </summary>
        Languages Language { get; set; }
        Expression<Func<Lang, L>> Message { get; }
        Expression<Func<Lang, L>> Subject { get; }
        NotificationType Type { get; }

        public string? GetUrl();

        string GenerateSubject();
        string GenerateMessage();
    }

    public enum NotificationType
    {
        Email = 1,
        InApp = 2,
        Firebase = 4
    }
}