using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;

using Vit.Db.Module.Schema;
using Vit.DynamicCompile.EntityGenerate;
using Vit.Extensions.Db_Extensions;

namespace Vit.Extensions.Linq_Extensions
{


    public static partial class DbContextExtensions
    {
        #region AutoGenerateEntity

        /// <summary>
        /// 对数据库中未映射的表创建模型实体代码并映射
        /// </summary>
        /// <param name="data"></param>
        /// <param name="connInfo"></param>
        /// <param name="skipAlreadyMappedTable">跳过已经映射过的表。仅映射当前尚未映射的表（默认true）。若为false：自动生成所有表的实体模型并添加映射</param>
        /// <param name="model"></param>
        public static (List<TableSchema> schema, Type[] types) AutoGenerateEntity(this DbContext data, Vit.Db.Util.Data.ConnectionInfo connInfo, bool skipAlreadyMappedTable = true, IMutableModel model = null)
        {
            List<TableSchema> schemas;


            // #1 GetSchema
            using (var conn = Vit.Db.Util.Data.ConnectionFactory.GetConnection(connInfo))
            {
                schemas = conn.GetSchema();
            }

            // #2 GenerateEntityType
            var entityNamespace = "";
            Type[] entityTypes = schemas.Select(schema => EntityHelp.GenerateEntityBySchema(schema, entityNamespace)).ToArray();

            #region #3 add entities to ef
            if (model == null) model = (IMutableModel)data.Model;

            Type[] typeToMap = entityTypes;

            if (skipAlreadyMappedTable)
            {
                var mappedTables = model.GetEntityTypes().Select(m => m.GetEntityTableName());
                typeToMap = typeToMap.Where(type => !mappedTables.Any(tableName => tableName == type.Name)).ToArray();
            }

            foreach (var type in typeToMap)
            {
                model.AddEntityType(type);
            }
            #endregion
            return (schemas, typeToMap);
        }
        #endregion



    }
}