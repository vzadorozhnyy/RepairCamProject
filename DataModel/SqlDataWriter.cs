using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DataModel {
    public class SqlDataWriter {
        public const string Comma = ", ";
        private readonly TransactionConnection _connection;
        private readonly MsSqlDataConverter _converter;
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>(10);
        private readonly Dictionary<string, string> _dataAlt = new Dictionary<string, string>(10);
        private readonly string _tableName;

        public SqlDataWriter(TransactionConnection connection, string tableName) {
            Debug.Assert(!ReferenceEquals(null, connection) && !string.IsNullOrEmpty(tableName));

            _connection = connection;
            _converter = new MsSqlDataConverter();
            _tableName = tableName;
        }

        public void NewSave() {
            _data.Clear();
            _dataAlt.Clear();
        }

        public void SetValue(string fieldName, string value) {
            _dataAlt[fieldName] = _data[fieldName] = _converter.ConvertString(value);
        }

        public void SetValue(string fieldName, double value) {
            _dataAlt[fieldName] = _data[fieldName] = _converter.ConvertDouble(value);
        }

        public void SetValue(string fieldName, int value) {
            _dataAlt[fieldName] = _data[fieldName] = _converter.ConvertInt(value);
        }

        public void SetValue(string fieldName, bool value) {
            _dataAlt[fieldName] = _data[fieldName] = _converter.ConvertBool(value);
        }

        public void SetValue(string fieldName, DateTime value) {
            _data[fieldName] = _converter.ConvertDateTime(value);
            _dataAlt[fieldName] = _converter.ConvertDateTimeAlt(value);
        }

        public void SetValue(string fieldName, Guid value) {
            _dataAlt[fieldName] = _data[fieldName] = _converter.ConvertGuid(value);
        }

        public void SetNullValue(string fieldName) {
            _dataAlt[fieldName] = _data[fieldName] = _converter.GetNullValue();
        }

        #region Execute

        public int ExecuteInsert() {
            string lSql = string.Format("insert into {0} with (ROWLOCK) ({1}) values ({2})", _tableName, GetFieldNames(Comma), GetFieldValues(Comma));
            if (_connection == null)
                return 0;
            return _connection.Execute(lSql);
        }

        public int ExecuteUpdate(string whereClause) {
            string lSql = string.Format("update {0} with (rowlock)set {1} ", _tableName, GetNameValuePairs());
            if (!string.IsNullOrEmpty(whereClause))
                lSql += string.Format(" where {0}", whereClause);
            if (_connection == null)
                return 0;
            return _connection.Execute(lSql);
        }

        public int ExecuteAuto(params string[] keys) {
            string lKeys = ListToString(keys);
            string lWhere = GetNameValuePairsForWhere(keys);

            string lSql = string.Format("if exists(select {0} from {1} (nolock) where {2}) " +
                                        " update {1} with (ROWLOCK) set {3} where {2} " +
                                        " else insert into {1} with (ROWLOCK) ({4}) values ({5}) ",
                lKeys, _tableName, lWhere, GetNameValuePairs(), GetFieldNames(Comma), GetFieldValues(Comma));
            if (_connection == null)
                return 0;
            return _connection.Execute(lSql);
        }

        public int ExecuteSave(params string[] keys) {
            StringBuilder lSql = new StringBuilder();
            lSql.AppendFormat("exec sp{0}_Save ", _tableName);
            List<string> lKeys = new List<string>(_dataAlt.Keys);

            for (int l = 0; l < lKeys.Count; l++) {
                if (l > 0)
                    lSql.Append(",");
                lSql.AppendFormat("@{0}={1}", lKeys[l], _dataAlt[lKeys[l]]);
            }

            return _connection.Execute(lSql.ToString());
        }

        public int ExecuteDeleteBeforeInsert(params string[] keys) {
            string lWhere = GetNameValuePairsForWhere(keys);

            string lSql = string.Format("delete from {0} with (ROWLOCK) where {1} " +
                                        " insert into {0} with (ROWLOCK)({2}) values ({3}) ",
                _tableName, lWhere, GetFieldNames(Comma), GetFieldValues(Comma));
            if (_connection == null)
                return 0;
            return _connection.Execute(lSql);
        }

        #endregion

        #region Helper stuff

        private string GetFieldValues(string delimiter) {
            StringBuilder lBuilder = new StringBuilder();
            List<string> lValues = new List<string>(_data.Values);
            for (int l = 0; l < lValues.Count; l++) {
                if (lBuilder.Length != 0)
                    lBuilder.Append(delimiter);
                lBuilder.Append(lValues[l]);
            }
            return lBuilder.ToString();
        }

        private string GetFieldNames(string delimiter) {
            StringBuilder lBuilder = new StringBuilder();
            List<string> lKeys = new List<string>(_data.Keys);

            for (int l = 0; l < lKeys.Count; l++) {
                if (lBuilder.Length != 0)
                    lBuilder.Append(delimiter);
                lBuilder.Append(lKeys[l]);
            }

            return lBuilder.ToString();
        }

        private string GetNameValuePairs() {
            StringBuilder lBuilder = new StringBuilder();
            List<string> lKeys = new List<string>(_data.Keys);

            for (int l = 0; l < lKeys.Count; l++) {
                if (lBuilder.Length != 0)
                    lBuilder.Append(Comma);
                lBuilder.AppendFormat("{0}={1}", lKeys[l], _data[lKeys[l]]);
            }
            return lBuilder.ToString();
        }

        private string GetNameValuePairsForWhere(params string[] fields) {
            StringBuilder lBuilder = new StringBuilder();
            List<string> lKeys = new List<string>(_data.Keys);
            List<string> lFields = new List<string>(fields);

            for (int l = 0; l < lKeys.Count; l++) {
                string lKey = lKeys[l];
                if (lFields.Contains(lKey)) {
                    if (lBuilder.Length != 0)
                        lBuilder.Append(" and ");
                    string lVal = _data[lKey];
                    lBuilder.AppendFormat(lVal == _converter.GetNullValue() ? "{0} is {1}" : "{0}={1}", lKey, lVal);
                }
            }
            return lBuilder.ToString();
        }


        private static string ListToString(params string[] list) {
            StringBuilder lBuilder = new StringBuilder();

            for (int l = 0; l < list.Length; l++) {
                if (lBuilder.Length != 0)
                    lBuilder.Append(Comma);
                lBuilder.Append(list[l]);
            }
            return lBuilder.ToString();
        }

        #endregion
    }
}