using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Services
{
    public interface IHttpContextService
    {
        public string? Token { get; }
        public ObjectId? GameBoardId { get; }

        IHeaderDictionary? Headers { get; }
        string? GetEncodedPathAndQuery();

        string? RemoteIpAddress { get; }
    }
}
