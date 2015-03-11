using System;
using System.Collections.Generic;

namespace DataModel.Objects {
    public class User {
        private readonly Dictionary<Guid, Customer> _customers = new Dictionary<Guid, Customer>();
        private readonly Guid _id;
        private string _email;
        private DateTime _expireDate;
        private string _login;
        private string _name;
        private string _options;
        private string _pwd;
        private DateTime _registrationDate;

        public User() : this(Guid.NewGuid()) {
        }

        public User(Guid id) {
            _id = id;
        }

        public Guid Id {
            get { return _id; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public string Login {
            get { return _login; }
            set { _login = value; }
        }

        public string Email {
            get { return _email; }
            set { _email = value; }
        }

        public string Pwd {
            get { return _pwd; }
            set { _pwd = value; }
        }

        public DateTime RegistrationDate {
            get { return _registrationDate; }
            set { _registrationDate = value; }
        }

        public DateTime ExpireDate {
            get { return _expireDate; }
            set { _expireDate = value; }
        }

        public string Options {
            get { return _options; }
            set { _options = value; }
        }

        public void AddCustomer(Customer customer) {
            if (!_customers.ContainsKey(customer.Id))
                _customers.Add(customer.Id, customer);
            else
                _customers[customer.Id] = customer;
        }

        public bool HasCustomer(Guid customerId) {
            return _customers.ContainsKey(customerId);
        }

        public Customer GetCustomer(Guid customerId) {
            return _customers.ContainsKey(customerId) ? _customers[customerId] : null;
        }

        public bool HasOrder(Guid orderId) {
            foreach (KeyValuePair<Guid, Customer> lPair in _customers)
                if (lPair.Value.HasOrder(orderId))
                    return true;
            return false;
        }

        public Order GetOrder(Guid orderId) {
            foreach (KeyValuePair<Guid, Customer> lPair in _customers)
                if (lPair.Value.HasOrder(orderId))
                    return lPair.Value.GetOrder(orderId);
            return null;
        }

        public List<Customer> GetCustomers() {
            return new List<Customer>(_customers.Values);
        }
    }
}