﻿using War.Server.Domain;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Models.Forms
{
    public interface IEntityForm<TEntity> where TEntity : class, IEntity
    {
        ObjectId Id { get; set; }

        /// <summary>
        /// Formdan gelen verileri entity'e yazar
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="UserException"></exception>
        Task Bind(IEntityContext context, IUser user, TEntity entity);
    }

    public interface ITenantEntityForm<TTenantEntity> where TTenantEntity : class, IGameBoardEntity
    {
        ObjectId Id { get; set; }

        /// <summary>
        /// Formdan gelen verileri entity'e yazar
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <exception cref="UserException"></exception>
        Task Bind(IEntityContext context, IUser user, TTenantEntity entity);
    }
}