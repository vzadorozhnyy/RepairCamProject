using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;

namespace DataModel.DB {
    public class SimpleConnection {
        protected const int DefaultCommandTimeout = 30000;
        private readonly string _connectionString;

        public SimpleConnection(string connectionString) {
            _connectionString = connectionString;
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalar(string sqlString) {
            using (OleDbConnection lConnection = new OleDbConnection(_connectionString)) {
                lConnection.Open();
                DbCommand lSqlCommand = new OleDbCommand(sqlString, lConnection);
                lSqlCommand.CommandType = CommandType.Text;
                lSqlCommand.CommandTimeout = DefaultCommandTimeout;

                return lSqlCommand.ExecuteScalar();
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataReader GetData(string sqlString) {
            OleDbConnection lConnection = new OleDbConnection(_connectionString);
            lConnection.Open();
            DbCommand lSqlCommand = new OleDbCommand(sqlString, lConnection);
            lSqlCommand.CommandType = CommandType.Text;
            lSqlCommand.CommandTimeout = DefaultCommandTimeout;

            return new DataReader(lSqlCommand.ExecuteReader(), lConnection, sqlString);
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int Execute(string sqlString) {
            using (OleDbConnection lConnection = new OleDbConnection(_connectionString)) {
                lConnection.Open();
                DbCommand lSqlCommand = new OleDbCommand(sqlString, lConnection);
                lSqlCommand.CommandType = CommandType.Text;
                lSqlCommand.CommandTimeout = DefaultCommandTimeout;

                return lSqlCommand.ExecuteNonQuery();
            }
        }
    }
}