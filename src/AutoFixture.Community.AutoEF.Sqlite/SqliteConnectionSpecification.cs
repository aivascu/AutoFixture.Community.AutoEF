using System;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;

namespace AutoFixture.Community.AutoEF.Sqlite
{
    /// <summary>
    /// Describes a request for a <see cref="SqliteConnection"/> instance.
    /// </summary>
    public class SqliteConnectionSpecification : IRequestSpecification
    {
        /// <inheritdoc />
        public bool IsSatisfiedBy(object request)
        {
            return request is Type type
                   && !type.IsAbstract
                   && type == typeof(SqliteConnection);
        }
    }
}
