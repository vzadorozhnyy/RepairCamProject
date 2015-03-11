using System;

namespace DataModel {
    public enum EOrderStatus {
        New = 1,
        Uploaded = 2,
        Sended = 3,
        Viewed = 4,
        Closed = 5
    }

    public class Order {
        private readonly Customer _customer;
        private readonly Guid _id;
        private string _details;
        private string _imageData;
        private string _name;
        private EOrderStatus _status = EOrderStatus.New;
        private string _videoUrl;

        public Order(Customer customer) : this(customer, Guid.NewGuid()) {
        }

        public Order(Customer customer, Guid id) {
            _customer = customer;
            _id = id;
        }

        public Guid Id {
            get { return _id; }
        }

        public string VideoUrl {
            get { return _videoUrl; }
            set { _videoUrl = value; }
        }

        public Customer Customer {
            get { return _customer; }
        }

        public string Details {
            get { return _details; }
            set { _details = value; }
        }

        public string ImageData {
            get { return _imageData; }
            set { _imageData = value; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public EOrderStatus Status {
            get { return _status; }
            set { _status = value; }
        }
    }
}