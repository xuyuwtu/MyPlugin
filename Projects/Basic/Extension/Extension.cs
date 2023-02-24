using System.Data;
using System.Linq.Expressions;

namespace VBY.Basic.Extension;
public static class VBYDbExt
{
    public static int GetInt32(this IDataReader reader, string name) => reader.GetInt32(reader.GetOrdinal(name));
    public static string GetString(this IDataReader reader, string name) => reader.GetString(reader.GetOrdinal(name));
    public static DateTime GetDateTime(this IDataReader reader, string name) => reader.GetDateTime(reader.GetOrdinal(name));
    public static void ForEach(this IDataReader args, Action<IDataReader> action) { while (args.Read()) action(args); }
    public static void DoForEach(this IDataReader args,Action<IDataReader> action) { action(args); while (args.Read()) action(args); }
    public static void DoForEach<T>(this IDataReader args, Action<DBHelper<T>> action) 
    {
        var helper = new DBHelper<T>(args);
        action(helper);
        while (args.Read()) 
            action(helper); 
    }
}
public class DBHelper<T>
{
    public IDataReader Reader;
    public DBHelper(IDataReader reader)
    {
        Reader = reader;
    }
    public TResult Get<TResult>(Expression<Func<T, TResult>> expression) => TShockAPI.DB.DbExt.Get<TResult>(Reader, ((MemberExpression)expression.Body).Member.Name);
}