using System.Diagnostics.CodeAnalysis;

namespace VBY.GameContentExtension;

internal class StringCache
{
    private Dictionary<string, string> cache = new(StringComparer.Ordinal);
    private ReferenceList<string> references = new();
    public ReferenceList<string> References { get => references; }

    public static StringCache Default = new();

    public void AddIfNotExists(string s)
    {
        var index = references.IndexOf(s);
        if (index != -1)
        {
            return;
        }
        if (cache.ContainsKey(s))
        {
            return;
        }
        cache.Add(s, s);
        references.Add(s);
    }
    public string GetOrAdd(string s)
    {
        var index = references.IndexOf(s);
        if (index != -1)
        {
            return references[index];
        }
        if (cache.TryGetValue(s, out var result))
        {
            return result;
        }
        cache.Add(s, s);
        references.Add(s);
        return s;
    }
    public bool TryGetValue(string s, [MaybeNullWhen(false)] out string value)
    {
        return cache.TryGetValue(s, out value);
    }
}
