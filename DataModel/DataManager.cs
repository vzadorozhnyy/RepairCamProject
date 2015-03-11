using System;
using System.Collections.Generic;
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
    }
}