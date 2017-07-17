using UnityEngine;
using System.Collections.Generic;
using System;

namespace M4DLib
{
    public static class ListPool<T>
{

    static Stack<List<T>> _pooledObject = new Stack<List<T>>();
    static int iter = 0;

    static public List<T> Get ()
    {   
        if (_pooledObject.Count == 0)
        {
            var m = new List<T>();
            iter++;
            return m;
        } else {
            return _pooledObject.Pop();
        }

    }

    static public List<T> ConvertAll <T2> (List<T2> list, Func<T2, T> converter)
    {
        var l = Get();
        l.Expand(list);
        for (int i = 0; i < list.Count; i++)
        {
            l.Add(converter(list[i]));
        }
        return l;
    }

    static public void Release (List<T> list)
    {
        list.Clear();
        _pooledObject.Push(list);
    }
}
}
