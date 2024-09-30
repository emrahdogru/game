using War.Server.Utility.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using MongoDB.Bson;

namespace War.Server.Domain.Services
{
    public class HttpContextService : IHttpContextService
    {
        readonly HttpContext? httpContext;

        public HttpContextService(IHttpContextAccessor accessor)
        {
            httpContext = accessor.HttpContext;
        }

        public string? Token => httpContext?.Request.GetToken();

        public ObjectId? GameBoardId => httpContext?.Request.GetGameBoardId();

        public IHeaderDictionary? Headers => httpContext?.Request.Headers;

        public string? GetEncodedPathAndQuery()
        {
            return httpContext?.Request.GetEncodedPathAndQuery();
        }

        public string? Method
        {
            get
            {
                return httpContext?.Request.Method;
            }
        }

        public string? RemoteIpAddress => httpContext?.Connection?.RemoteIpAddress?.ToString();
    }
}
