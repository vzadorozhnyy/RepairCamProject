using System;
using System.Diagnostics;
using System.Threading;

namespace DataModel {
    public sealed class Locker : IDisposable {
        private LockCounter _item;

        public Locker(LockCounter item) {
            _item = item;
            _item.Lock();
            if (_item.LockCount > 1)
                Trace.WriteLine("Lock Count > 1");
        }

        public void Dispose() {
            _item.Unlock();
            _item = null;
        }

        ~Locker() {
            if (_item != null)
                _item.Unlock();
        }
    }

    public class LockCounter {
        private int _count;

        public int LockCount {
            get { return _count; }
        }

        public void Lock() {
            Interlocked.Increment(ref _count);
        }

        public void Unlock() {
            Interlocked.Decrement(ref _count);
        }
    }
}