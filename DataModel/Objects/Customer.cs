using System;
using System.Collections.Generic;

namespace DataModel.Objects {
    public class Customer {
        private readonly Guid _id;
        private readonly Dictionary<Guid, Order> _orders = new Dictionary<Guid, Order>();
        private readonly User _user;
        private string _email;
        private string _name;
        private string _options;
        private string _phone;

        public Customer(User user) : this(user, Guid.NewGuid()) {
        }

        public Customer(User user, Guid id) {
            _user = user;
            _id = id;
        }

        public User User {
            get { return _user; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public string Email {
            get { return _email; }
            set { _email = value; }
        }

        public string Phone {
            get { return _phone; }
            set { _phone = value; }
        }

        public string Options {
            get { return _options; }
            set { _options = value; }
        }

        public Guid Id {
            get { return _id; }
        }

        public void AddOrder(Order order) {
            if (!_orders.ContainsKey(order.Id))
                _orders.Add(order.Id, order);
            else
                _orders[order.Id] = order;
        }
    }
}