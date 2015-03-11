using System;
using System.Web.SessionState;
using DataModel.Objects;

/// <summary>
///     Summary description for DataManagerLocal
/// </summary>
public class DataManagerLocal {
    private static readonly object SyncRoot = new Object();
    private static DataManagerLocal _inst;
    private DataManagerLocal() {}


    public static DataManagerLocal Inst {
        get {
            if (_inst == null)
                lock (SyncRoot) {
                    if (_inst == null)
                        _inst = new DataManagerLocal();
                }

            return _inst;
        }
    }

    public User LoggedUser { get; set; }

    public const int MinRequiredPasswordLength = 6;
}