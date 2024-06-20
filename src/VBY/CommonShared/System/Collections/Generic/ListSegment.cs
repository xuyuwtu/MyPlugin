using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;
public readonly struct ListSegment<T> : IList<T>
{
    public struct Enumerator : IEnumerator<T>, IDisposable
    {
        private readonly IList<T> _list;

        private readonly int _start;

        private readonly int _end;

        private int _current;

        public T Current
        {
            get
            {
                if (_current < _start)
                {
                    throw new InvalidOperationException("enum not started");
                }
                if (_current >= _end)
                {
                    throw new InvalidOperationException("enum not ended");
                }
                return _list[_current];
            }
        }

        object? IEnumerator.Current => Current;

        internal Enumerator(ListSegment<T> listSegment)
        {
            _list = listSegment.List;
            _start = listSegment.Offset;
            _end = listSegment.Offset + listSegment.Count;
            _current = listSegment.Offset - 1;
        }

        public bool MoveNext()
        {
            if (_current < _end)
            {
                _current++;
                return _current < _end;
            }
            return false;
        }

        void IEnumerator.Reset() => _current = _start - 1;

        public void Dispose() { }
    }

    private readonly IList<T> _list;

    private readonly int _offset;

    private readonly int _count;
    public IList<T> List => _list;

    public int Offset => _offset;

    public int Count => _count;

    public T this[int index]
    {
        get
        {
            ThrowInvalidOperationIfDefault();
            if ((uint)index >= (uint)_count)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
            return _list[_offset + index];
        }
        set
        {
            ThrowInvalidOperationIfDefault();
            if ((uint)index >= (uint)_count)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
            _list[_offset + index] = value;
        }
    }

    T IList<T>.this[int index]
    {
        get
        {
            ThrowInvalidOperationIfDefault();
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
            return _list[_offset + index];
        }
        set
        {
            ThrowInvalidOperationIfDefault();
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
            _list[_offset + index] = value;
        }
    }

    bool ICollection<T>.IsReadOnly => true;

    public ListSegment(IList<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);
        _list = list;
        _offset = 0;
        _count = list.Count;
    }

    public ListSegment(IList<T> list, int offset, int count)
    {
        ArgumentNullException.ThrowIfNull(list);
        if ((uint)offset > (uint)list.Count)
        {
            throw new Exception("offset greater than count");
        }
        if ((uint)count > (uint)(list.Count - offset))
        {
            throw new Exception("offset plus count out of range");
        }
        _list = list;
        _offset = offset;
        _count = count;
    }

    public Enumerator GetEnumerator() => new Enumerator(this);

    public override int GetHashCode()
    {
        if (_list != null)
        {
            return HashCode.Combine(_offset, _count, _list.GetHashCode());
        }
        return 0;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ListSegment<T>)
        {
            return Equals((ListSegment<T>)obj);
        }
        return false;
    }

    public bool Equals(ListSegment<T> obj)
    {
        if (obj._list == _list && obj._offset == _offset)
        {
            return obj._count == _count;
        }
        return false;
    }

    public ListSegment<T> Slice(int index)
    {
        if ((uint)index > (uint)_count)
        {
            throw new IndexOutOfRangeException(nameof(index));
        }
        return new ListSegment<T>(_list, _offset + index, _count - index);
    }

    public ListSegment<T> Slice(int index, int count)
    {
        if ((uint)index > (uint)_count || (uint)count > (uint)(_count - index))
        {
            throw new IndexOutOfRangeException(nameof(index));
        }
        return new ListSegment<T>(_list, _offset + index, count);
    }

    public T[] ToArray() => _list.ToArray();

    public static bool operator ==(ListSegment<T> a, ListSegment<T> b) => a.Equals(b);

    public static bool operator !=(ListSegment<T> a, ListSegment<T> b) => !(a == b);

    int IList<T>.IndexOf(T item) => _list.IndexOf(item);

    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    void ICollection<T>.Add(T item) => throw new NotSupportedException();

    void ICollection<T>.Clear() => throw new NotSupportedException();

    bool ICollection<T>.Contains(T item) => _list.Contains(item);

    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void ThrowInvalidOperationIfDefault()
    {
        if (_list == null)
        {
            throw new InvalidOperationException("list is null");
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (arrayIndex >= array.Length || arrayIndex < 0)
        {
            throw new IndexOutOfRangeException(nameof(array));
        }
        if (array.Length - arrayIndex < _count)
        {
            throw new ArgumentException("destination too short");
        }
        for (int i = 0; i < _count; i++)
        {
            array[i] = _list[_offset + i];
        }
    }
}