using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TalentErrorHandling
{
    internal static class SQLConn
    {
        internal static string ConnStr()
        {
            SqlConnectionStringBuilder sqlConn = new SqlConnectionStringBuilder
            {
                DataSource = Constants.GetEnvironmentVariable("dbDataSourcePort"),
                UserID = KeyVaultAuth.GetSecretFromVault("sqlUsernameUri"),
                Password = KeyVaultAuth.GetSecretFromVault("sqlPasswordUri"),
                InitialCatalog = Constants.GetEnvironmentVariable("dbName"),
                MultipleActiveResultSets = true
            };
            return sqlConn.ConnectionString;
        }
    }
}
