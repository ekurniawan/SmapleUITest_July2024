﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCards.MyUITest
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "The values are available indirectly on the base class.")]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is designed as an extensibility point.")]
    public class SqlServerDataAttribute : OleDbDataAttribute
    {
        const string sqlWithTrust =
            "Provider=SQLOLEDB; Data Source={0}; Initial Catalog={1}; Integrated Security=SSPI;";

        //"Provider=SQLOLEDB;Data Source=ACTUAL;Integrated Security=SSPI;Initial Catalog=TestDatabase"

        const string sqlWithUser =
            "Provider=SQLOLEDB; Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};";


        /// <summary>
        /// Creates a new instance of <see cref="SqlServerDataAttribute"/>, using a trusted connection.
        /// </summary>
        /// <param name="serverName">The server name of the Microsoft SQL Server</param>
        /// <param name="databaseName">The database name</param>
        /// <param name="selectStatement">The SQL SELECT statement to return the data for the data theory</param>
        public SqlServerDataAttribute(string serverName,
            string databaseName,
            string selectStatement)
            : base(String.Format(CultureInfo.InvariantCulture, sqlWithTrust, serverName, databaseName), selectStatement)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlServerDataAttribute"/>, using the provided username and password.
        /// </summary>
        /// <param name="serverName">The server name of the Microsoft SQL Server</param>
        /// <param name="databaseName">The database name</param>
        /// <param name="userName">The username for the server</param>
        /// <param name="password">The password for the server</param>
        /// <param name="selectStatement">The SQL SELECT statement to return the data for the data theory</param>
        public SqlServerDataAttribute(string serverName,
                                      string databaseName,
                                      string userName,
                                      string password,
                                      string selectStatement)
            : base(String.Format(CultureInfo.InvariantCulture, sqlWithUser, serverName, databaseName, userName, password),
                   selectStatement)
        { }
    }
}
