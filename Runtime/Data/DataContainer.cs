using System;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer<T> where T : BaseData
{
    private readonly Dictionary<int, T> _dataDic = new();
    
    public int Count => _dataDic.Count;

    public DataContainer(int capacity = 0)
    {
        _dataDic = capacity > 0
            ? new Dictionary<int, T>(capacity)
            : new Dictionary<int, T>();
    }
    
    public void Add(T data)
    {
        _dataDic[data.Id] = data;
    }

    public T Get(int id)
    {
        if (_dataDic.TryGetValue(id, out var data))
        {
            return data;
        }
        
        return null;
    }

    public IReadOnlyList<T> GetAll()
    {
        return new List<T>(_dataDic.Values);
    }
}
