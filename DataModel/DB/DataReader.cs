using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;

namespace DataModel.DB {
    public class DataReader : IDisposable {
        private readonly List<string> _columns = new List<string>();
        private readonly OleDbConnection _connection;
        private readonly List<string> _fetchedColumns = new List<string>();
        private readonly DbDataReader _reader;
        private readonly string _sqlString;
        private bool _rows;

        public DataReader(DbDataReader reader, OleDbConnection connection, string sqlString) {
            _reader = reader;
            _connection = connection;
            _sqlString = sqlString;

            for (int l = 0; l < _reader.FieldCount; l++)
                _columns.Add(_reader.GetName(l));
        }

        public object this[string colName] {
            get {
                if (!_fetchedColumns.Contains(colName.ToUpper()))
                    _fetchedColumns.Add(colName.ToUpper());
                return _reader[colName];
            }
        }

        public void Dispose() {
            if (_reader != null)
                if (!_reader.IsClosed) {
                    _rows = _reader.HasRows;
                    _reader.Close();
                }

            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
            if (_rows) {
                StringBuilder lFields = new StringBuilder();
                for (int l = 0; l < _columns.Count; l++)
                    if (!_fetchedColumns.Contains(_columns[l].ToUpper()))
                        lFields.AppendFormat("{0},", _columns[l]);
                if (lFields.Length > 0) {
                    Trace.WriteLine("-----------------");
                    Trace.WriteLine(_sqlString);
                    Trace.WriteLine("Unused Fields: " + lFields);
                    Trace.WriteLine("-----------------");
                }
            }
        }

        public void Close() {
            _rows = _reader.HasRows;
            _reader.Close();
        }

        public bool Read() {
            return _reader.Read();
        }

        public string GetSql() {
            return _sqlString;
        }

        public void GetBytes(int ordinal, byte[] buffer, int dataLength) {
            _reader.GetBytes(ordinal, 0, buffer, 0, dataLength);
        }
    }
}