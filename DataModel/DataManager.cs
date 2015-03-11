using System;
using System.Collections.Generic;
using DataModel.DB;
using DataModel.Objects;

namespace DataModel {
    public class DataManager {
        private static readonly object SyncRoot = new Object();
        private static DataManager _inst;
        private readonly Dictionary<Guid, User> _users = new Dictionary<Guid, User>();
        private DataLoader _dataLoader;

        private DataManager() {
        }

        public static DataManager Inst {
            get {
                if (_inst == null)
                    lock (SyncRoot) {
                        if (_inst == null)
                            _inst = new DataManager();
                    }

                return _inst;
            }
        }

        public void Init(string connectionString) {
            _dataLoader = new DataLoader(connectionString);
            Load();
        }

        private void Load() {
            _dataLoader.LoadUsers(_users);
        }

        public bool CanUserLogin(string login, string pwd) {
            User lUser = GetUserByLogin(login);
            if (lUser == null)
                return false;
            return lUser.Pwd == pwd;
        }

        public User GetUserByLogin(string login) {
            foreach (KeyValuePair<Guid, User> lPair in _users)
                if (lPair.Value.Login == login)
                    return lPair.Value;
            return null;
        }

        public void SaveUser(User user) {
            lock (_users)
                if (!_users.ContainsKey(user.Id))
                    _users.Add(user.Id, user);
                else
                    _users[user.Id] = user;
            _dataLoader.SaveUser(user);
        }

        public void SaveCustomer(Customer customer) {
            lock (_users) {
                if (!_users.ContainsKey(customer.User.Id))
                    return;
                User lUser = _users[customer.User.Id];
                lUser.AddCustomer(customer);
            }
            _dataLoader.SaveCustomer(customer);
        }

        public void SaveOrder(Order order) {
            lock (_users) {
                if (!_users.ContainsKey(order.Customer.User.Id))
                    return;
                order.Customer.AddOrder(order);
            }
            _dataLoader.SaveOrder(order);
        }

        public User GetUser(Guid id) {
            return _users.ContainsKey(id) ? _users[id] : null;
        }

        public Customer GetCustomer(Guid customerId) {
            foreach (KeyValuePair<Guid, User> lPair in _users)
                if (lPair.Value.HasCustomer(customerId))
                    return lPair.Value.GetCustomer(customerId);
            return null;
        }

        public Order GetOrder(Guid orderId) {
            foreach (KeyValuePair<Guid, User> lPair in _users)
                if (lPair.Value.HasOrder(orderId))
                    return lPair.Value.GetOrder(orderId);
            return null;
        }

        public List<Order> GetOrders(User user, EOrderStatus status) {
            List<Order> lOrders = GetOrders(user);
            List<Order> lFiltered = new List<Order>();
            for (int l = 0; l < lOrders.Count; l++)
                if (lOrders[l].Status == status)
                    lFiltered.Add(lOrders[l]);
            return lFiltered;
        }

        List<Order> GetOrders(User user) {
            List<Order> lOrders = new List<Order>();
            List<Customer> lCustomers = user.GetCustomers();
            for (int l = 0; l < lCustomers.Capacity; l++)
                lOrders.AddRange(lCustomers[l].GetOrders());
            return lOrders;
        }
    }
}