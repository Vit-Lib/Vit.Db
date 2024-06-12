﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;


namespace Vit.Extensions.Linq_Extensions.Execute
{
    public static partial class IDbConnection_Execute_Extensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Execute(this IDbConnection conn, string sql, IDictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            // #1 setup command
            using var cmd = conn.CreateCommand();
            if (transaction != null) cmd.Transaction = transaction;
            if (commandTimeout.HasValue) cmd.CommandTimeout = commandTimeout.Value;
            cmd.Connection = conn;
            cmd.CommandText = sql;
            AddParameter(cmd, param);


            // #2 execute
            bool wasClosed = conn.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) conn.Open();
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (wasClosed) conn.Close();
            }

        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object ExecuteScalar(this IDbConnection conn, string sql, IDictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            // #1 setup command
            using var cmd = conn.CreateCommand();
            if (transaction != null) cmd.Transaction = transaction;
            if (commandTimeout.HasValue) cmd.CommandTimeout = commandTimeout.Value;
            cmd.Connection = conn;
            cmd.CommandText = sql;
            AddParameter(cmd, param);

            // #2 execute
            bool wasClosed = conn.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) conn.Open();
                return cmd.ExecuteScalar();
            }
            finally
            {
                if (wasClosed) conn.Close();
            }
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDataReader ExecuteReader(this IDbConnection conn, string sql, IDictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {

            IDbCommand cmd = null;

            bool wasClosed = conn.State == ConnectionState.Closed, disposeCommand = true;
            try
            {
                // #1 setup command
                cmd = conn.CreateCommand();
                if (transaction != null) cmd.Transaction = transaction;
                if (commandTimeout.HasValue) cmd.CommandTimeout = commandTimeout.Value;
                cmd.Connection = conn;
                cmd.CommandText = sql;
                AddParameter(cmd, param);

                // #2 execute
                var commandBehavior = wasClosed ? CommandBehavior.CloseConnection : CommandBehavior.Default;
                if (wasClosed) conn.Open();

                var reader = cmd.ExecuteReader(commandBehavior);
                wasClosed = false; // don't dispose before giving it to them!
                disposeCommand = false;
                return reader;
            }
            finally
            {
                if (wasClosed) conn.Close();
                if (disposeCommand)
                {
                    //cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }

        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddParameter(IDbCommand cmd, IDictionary<string, object> param)
        {
            if (param != null)
            {
                foreach (var entry in param)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = entry.Key;
                    p.Value = entry.Value ?? DBNull.Value;
                    cmd.Parameters.Add(p);
                }
            }
        }

    }
}
