﻿using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Utility.ExtensionMethods
{
    public static class HttpRequestExtensionMethods
    {
        /// <summary>
        /// HTTP Headers'tan işlem yapılan hesap kimlik numarasını alır
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static ObjectId? GetGameBoardId(this HttpRequest httpRequest)
        {
            if (ObjectId.TryParse(httpRequest.Headers["game"].ToString(), out ObjectId tenantId))
                return tenantId;

            return null;
        }

        public static string? GetToken(this HttpRequest httpRequest)
        {
            return httpRequest.Headers["token"].ToString();
        }

    }
}
