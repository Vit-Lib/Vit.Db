﻿using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

using Vit.Extensions.Serialize_Extensions;


namespace Vit.Extensions.Db_Extensions
{
    public static partial class IDbConnection_ExecuteScalar_Extensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ExecuteScalar<T>(this IDbConnection conn, string sql, IDictionary<string, object> param = null, int? commandTimeout = null)
        {
            return conn.ExecuteScalar(sql, param: param, commandTimeout: commandTimeout).Convert<T>();
        }
    }
}
