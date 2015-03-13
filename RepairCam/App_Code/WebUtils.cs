using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class WebUtils {
    private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

    public static string Serialize(object obj) {
        lock (Serializer) {
            return Serializer.Serialize(obj);
        }
    }

    public static IDictionary<string, object> Deserialize(string str) {
        IDictionary<string, object> lDeserialize = null;
        try {
            lock (Serializer) {
                lDeserialize = Serializer.Deserialize<IDictionary<string, object>>(str);
            }
        } catch (Exception ex) {}
        return lDeserialize;
    }
}