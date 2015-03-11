using System;
using System.Collections.Generic;
using DataModel.DB;
using DataModel.Objects;

namespace DataModel {
    internal class DataLoader {
        private const string ColId = "id";
        private const string ColName = "name";
        private const string ColEmail = "email";
        private const string ColPwd = "password";
        private const string ColOptions = "options";
        private const string ColLogin = "login";
        private const string ColRegDate = "registrationDate";
        private const string ColExpireDate = "expireDate";
        private const string ColUserId = "userId";
        private const string ColPhone = "phone";
        private const string ColCustomerId = "customerId";
        private const string ColDetails = "details";
        private const string ColStatus = "status";
        private const string ColVideoUrl = "videoUrl";
        private const string ColImage = "image";
        private const string ColOrderDate = "orderDate";
        private const string ColVideoRecordDate = "videoRecordDate";
        private const string ColVideoViewDate = "videoViewDate";

        private readonly LockCounter _connectionLock = new LockCounter();
        private readonly string _connectionString;

        public DataLoader(string connectionString) {
            _connectionString = connectionString;
        }

        private SimpleConnection NewConnection() {
            using (new Locker(_connectionLock))
                lock (_connectionLock) {
                    if (string.IsNullOrEmpty(_connectionString))
                        return null;
                    return new SimpleConnection(_connectionString);
                }
        }

        private TransactionConnection NewTransaction() {
            using (new Locker(_connectionLock))
                lock (_connectionLock) {
                    if (string.IsNullOrEmpty(_connectionString))
                        return null;
                    return new TransactionConnection(_connectionString);
                }
        }

        public void LoadUsers(Dictionary<Guid, User> users) {
            SimpleConnection lConnection = NewConnection();
            string lSQL = string.Format("select id, login, name, password, email, options, registrationdate, expiredate from tblUsers (nolock)");
            using (DataReader lReader = lConnection.GetData(lSQL))
                while (lReader.Read()) {
                    Guid lId = lReader[ColId].SDAsGuid();
                    if (users.ContainsKey(lId))
                        continue;
                    User lUser = new User(lId);
                    lUser.Login = lReader[ColLogin].SDAsStr();
                    lUser.Name = lReader[ColName].SDAsStr();
                    lUser.Pwd = lReader[ColPwd].SDAsStr();
                    lUser.Email = lReader[ColEmail].SDAsStr();
                    lUser.Options = lReader[ColOptions].SDAsStr();
                    lUser.RegistrationDate = lReader[ColRegDate].SDAsDate();
                    lUser.ExpireDate = lReader[ColExpireDate].SDAsDate();
                    users.Add(lId, lUser);
                }
            LoadUserCustomers(users);
        }

        private void LoadUserCustomers(Dictionary<Guid, User> users) {
            Dictionary<Guid, Customer> lDictionary = new Dictionary<Guid, Customer>();
            SimpleConnection lConnection = NewConnection();
            string lSQL = string.Format("select id, userid, name, email, options, phone from tblCustomers (nolock)");
            using (DataReader lReader = lConnection.GetData(lSQL))
                while (lReader.Read()) {
                    Guid lUserId = lReader[ColUserId].SDAsGuid();
                    if (!users.ContainsKey(lUserId))
                        continue;
                    User lUser = users[lUserId];
                    Guid lCustomerId = lReader[ColId].SDAsGuid();
                    Customer lCustomer = new Customer(lUser, lCustomerId);
                    lCustomer.Name = lReader[ColName].SDAsStr();
                    lCustomer.Email = lReader[ColEmail].SDAsStr();
                    lCustomer.Options = lReader[ColOptions].SDAsStr();
                    lCustomer.Phone = lReader[ColPhone].SDAsStr();
                    lUser.AddCustomer(lCustomer);
                    if (!lDictionary.ContainsKey(lCustomerId))
                        lDictionary.Add(lCustomerId, lCustomer);
                }
            LoadUserCustomerOrders(lDictionary);
        }

        private void LoadUserCustomerOrders(Dictionary<Guid, Customer> customers) {
            SimpleConnection lConnection = NewConnection();
            string lSQL = string.Format("select id, customerid, name, details, status, image, videoUrl, orderDate, videoRecordDatem videoViewDate from tblOrders (nolock)");
            using (DataReader lReader = lConnection.GetData(lSQL))
                while (lReader.Read()) {
                    Guid lCustomerId = lReader[ColCustomerId].SDAsGuid();
                    if (!customers.ContainsKey(lCustomerId))
                        continue;
                    Customer lCustomer = customers[lCustomerId];
                    Guid lOrderId = lReader[ColId].SDAsGuid();
                    Order lOrder = new Order(lCustomer, lOrderId);
                    lOrder.Name = lReader[ColName].SDAsStr();
                    lOrder.Details = lReader[ColDetails].SDAsStr();
                    lOrder.Status = (EOrderStatus) lReader[ColStatus].SDAsInt();
                    lOrder.ImageData = lReader[ColImage].SDAsStr();
                    lOrder.VideoUrl = lReader[ColVideoUrl].SDAsStr();
                    lOrder.OrderDate = lReader[ColOrderDate].SDAsDate();
                    lOrder.RecordDate = lReader[ColVideoRecordDate].SDAsDate();
                    lOrder.ViewDate = lReader[ColVideoViewDate].SDAsDate();
                    customers[lCustomerId].AddOrder(lOrder);
                }
        }

        public void SaveUser(User user) {
            using (TransactionConnection lConnection = NewTransaction()) {
                SqlDataWriter lWriter = new SqlDataWriter(lConnection, "tblUsers");
                lWriter.SetValue(ColId, user.Id);
                lWriter.SetValue(ColEmail, user.Email);
                lWriter.SetValue(ColExpireDate, user.ExpireDate);
                lWriter.SetValue(ColLogin, user.Login);
                lWriter.SetValue(ColOptions, user.Options);
                lWriter.SetValue(ColPwd, user.Pwd);
                lWriter.SetValue(ColRegDate, user.RegistrationDate);
                lWriter.ExecuteAuto(ColId);
            }
        }

        public void SaveCustomer(Customer customer) {
            using (TransactionConnection lConnection = NewTransaction()) {
                SqlDataWriter lWriter = new SqlDataWriter(lConnection, "tblCustomers");
                lWriter.SetValue(ColId, customer.Id);
                lWriter.SetValue(ColUserId, customer.User.Id);
                lWriter.SetValue(ColName, customer.Name);
                lWriter.SetValue(ColEmail, customer.Email);
                lWriter.SetValue(ColPhone, customer.Phone);
                lWriter.SetValue(ColOptions, customer.Options);
                lWriter.ExecuteAuto(ColId);
            }
        }

        public void SaveOrder(Order order) {
            using (TransactionConnection lConnection = NewTransaction()) {
                SqlDataWriter lWriter = new SqlDataWriter(lConnection, "tblOrders");
                lWriter.SetValue(ColId, order.Id);
                lWriter.SetValue(ColCustomerId, order.Customer.Id);
                lWriter.SetValue(ColName, order.Name);
                lWriter.SetValue(ColDetails, order.Details);
                lWriter.SetValue(ColStatus, (int) order.Status);
                lWriter.SetValue(ColImage, order.ImageData);
                lWriter.SetValue(ColVideoUrl, order.VideoUrl);
                lWriter.ExecuteAuto(ColId);
            }
        }
    }
}