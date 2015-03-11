using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DataModel.DB {
    public class TransactionConnection : IDisposable {
        protected const int DefaultCommandTimeout = 30000;
        private readonly string _connectionString;
#if(DEBUG)
        private const long CacheSize = 0;
#else
        private const long CacheSize = 5000;
#endif
        private readonly StringBuilder _cache = new StringBuilder();

        private readonly OleDbConnection _connection;
        private OleDbTransaction _transaction;

        public TransactionConnection(string connectionString) {
            _connectionString = connectionString;
            _connection = new OleDbConnection(_connectionString);
        }

        public void Dispose() {
            Commit();
            if (_connection != null)
                if (_connection.State != ConnectionState.Closed)
                    _connection.Close();
        }

        public int Execute(string sqlString) {
            {
                CheckConnection();
                Cache(sqlString);
                if (ExecuteImmediatly(sqlString))
                    Flush();
                return 0;
            }
        }

        private bool ExecuteImmediatly(string sqlString) {
            if (sqlString.IndexOf("alter ", StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;
            if (sqlString.IndexOf("create ", StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;
            if (sqlString.IndexOf("drop ", StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;
            if (sqlString.IndexOf("restore ", StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;
            if (sqlString.IndexOf("backup ", StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;
            if (sqlString.IndexOf("declare ", StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;
            return false;
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalar(string sqlString) {
            CheckConnection();
            Flush();

            try {
                {
                    DbCommand lSqlCommand = new OleDbCommand(sqlString, _connection, _transaction);
                    lSqlCommand.CommandType = CommandType.Text;
                    lSqlCommand.CommandTimeout = DefaultCommandTimeout;

                    return lSqlCommand.ExecuteScalar();
                }
            } catch (Exception) {
                Rollback();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataReaderTran GetData(string sqlString) {
            CheckConnection();
            Flush();
            try {
                {
                    DbCommand lSqlCommand = new OleDbCommand(sqlString, _connection, _transaction);
                    lSqlCommand.CommandType = CommandType.Text;
                    lSqlCommand.CommandTimeout = DefaultCommandTimeout;

                    return new DataReaderTran(lSqlCommand.ExecuteReader(), sqlString);
                }
            } catch (Exception) {
                Rollback();
                throw;
            }
        }

        private void CheckConnection() {
            if (_connection == null || _connection.State != ConnectionState.Open)
                throw new Exception("Connection or transaction is closed");

            if (_transaction == null)
                Trace.WriteLine("Transaction is not initialized");
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void Flush() {
            CheckConnection();
            if (_cache.Length == 0)
                return;
            string lSql = _cache.ToString();
            try {
                {
                    DbCommand lSqlCommand = new OleDbCommand(lSql, _connection, _transaction);
                    lSqlCommand.CommandType = CommandType.Text;
                    lSqlCommand.CommandTimeout = DefaultCommandTimeout;
                    ClearCache();

                    lSqlCommand.ExecuteNonQuery();
                }
            } catch (Exception) {
                Rollback();
                throw;
            }
        }

        private void ClearCache() {
            _cache.Length = 0;
        }

        private void Cache(string sqlString) {
            _cache.AppendLine(sqlString);
            if (_cache.Length > CacheSize)
                Flush();
        }

        private void Commit() {
            CheckConnection();

            try {
                Flush();
                if (_transaction != null)
                    _transaction.Commit();
                _transaction = null;
            } catch (Exception) {
                Rollback();
                throw;
            }
        }

        private void Rollback() {
            try {
                if (_transaction != null)
                    _transaction.Rollback();
                _transaction = null;
            } catch (Exception) {
            }
        }
    }
}