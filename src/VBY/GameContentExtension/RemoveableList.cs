using System.Collections;

namespace VBY.GameContentExtension;

internal struct RemoveableList<T> : IEnumerator<T>
{
    private readonly List<T> _list;

    private int _index;
    private int _count;

    private T? _current;

    public readonly T Current => _current!;

    readonly object? IEnumerator.Current => Current;

    public RemoveableList(List<T> list)
    {
        _list = list;
        _index = 0;
        _count = list.Count;
        _current = default;
    }

    readonly void IDisposable.Dispose() { }
    public bool MoveNext()
    {
        var list = _list;
        if ((uint)_index < (uint)_count)
        {
            _current = list[_index];
            _index++;
            return true;
        }
        return MoveNextRare();
    }

    private bool MoveNextRare()
    {
        _index = _count + 1;
        _current = default;
        return false;
    }

    void IEnumerator.Reset()
    {
        _index = 0;
        _current = default;
    }
    public void RemoveCurrent()
    {
        _list.RemoveAt(_index);
        _index--;
        _count--;
    }
}
