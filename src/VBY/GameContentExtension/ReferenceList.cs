using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace VBY.GameContentExtension;

internal class ReferenceList<T> : IList<T> where T : class
{
    private readonly List<T> list;

    public ReferenceList()
    {
        list = new();
    }
    [NotNull]
    public T this[int index]
    {
        get => list[index];
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            if (ReferenceEquals(value, list[index]))
            {
                return;
            }
            if (Contains(value))
            {
                throw new InvalidOperationException("reference is exists");
            }
            list[index] = value;
        }
    }

    public int Count => list.Count;

    public bool IsReadOnly => false;

    public void Add([NotNull] T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        if (Contains(item))
        {
            throw new InvalidOperationException("reference is exists");
        }
        list.Add(item);
    }

    public void Clear() => list.Clear();

    public bool Contains([NotNullWhen(true)] T item)
    {
        if (item is null)
        {
            return false;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (ReferenceEquals(list[i], item))
            {
                return true;
            }
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

    public int IndexOf(T item)
    {
        if (item is null)
        {
            return -1;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (ReferenceEquals(list[i], item))
            {
                return i;
            }
        }
        return -1;
    }

    public void Insert(int index, [NotNull] T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        if (Contains(item))
        {
            throw new InvalidOperationException("reference is exists");
        }
        list.Insert(index, item);
    }

    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index == -1)
        {
            return false;
        }
        list.RemoveAt(index);
        return true;
    }

    public void RemoveAt(int index) => list.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
}
