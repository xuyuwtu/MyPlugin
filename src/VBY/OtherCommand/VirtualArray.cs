using System.Collections;

namespace VBY.OtherCommand;

public class VirtualArray<T> : IEnumerable<T>
{
    private readonly List<T[]> _list = new();
    private int _size;
    private List<int> _startIndexs = new();
    public int Count => _size;

    public bool IsReadOnly => true;

    public ref T this[int index]
    {
        get
        {
            if (index < 0 || index >= _size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            int listIndex = _startIndexs.Count - 1;
            for (; listIndex > 0; listIndex--)
            {
                if (index >= _startIndexs[listIndex])
                {
                    break;
                }
            }
            return ref _list[listIndex][index - _startIndexs[listIndex]];
        }
    }

    public VirtualArray(params T[][] arrays)
    {
        if (arrays is null || arrays.Length == 0)
        {
            throw new ArgumentException("arrays is null or empty");
        }
        foreach (var array in arrays)
        {
            if (array is null || array.Length <= 0)
            {
                throw new ArgumentException("array is null or array.Length is less than or equal zero");
            }
            Add(array);
        }

    }

    public void Add(T[] array)
    {
        if (_list.Count == 0)
        {
            _startIndexs.Add(0);
        }
        else
        {
            _startIndexs.Add(_startIndexs[^1] + _list[^1].Length);
        }
        _list.Add(array);
        _size += array.Length;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            for (int j = 0; j < _list[i].Length; j++)
            {
                yield return _list[i][j];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}