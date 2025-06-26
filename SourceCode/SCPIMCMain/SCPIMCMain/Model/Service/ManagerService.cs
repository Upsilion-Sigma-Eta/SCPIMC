using System;
using System.Windows;

namespace SCPIMCMain.Model.Logic;

public class ManagerService<TKey, TValue> : Object where TKey : notnull where TValue : class
{
    private Dictionary<TKey, TValue> _internalManagedItem = new Dictionary<TKey, TValue>();

    public IEnumerable<TKey> Keys()
    {
        return _internalManagedItem.Keys.ToList();
    }

    public IEnumerable<TValue> Values()
    {
        return _internalManagedItem.Values.ToList();
    }

    public TValue TryGetValue(TKey key)
    {
        try
        {
            TValue returnValue = default(TValue);
            if (_internalManagedItem.TryGetValue(key, out returnValue))
            {
                return returnValue;
            }
            else
            {
                throw new KeyNotFoundException("There is no key.");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public IEnumerable<TKey> TryGetKey(TValue value)
    {
        try
        {
            List<TKey> foundKeyWithValues = new List<TKey>();

            foreach (TKey savedKey in _internalManagedItem.Keys)
            {
                if (_internalManagedItem[savedKey] == value)
                {
                    foundKeyWithValues.Add(savedKey);
                }
            }

            return foundKeyWithValues;
        }
        catch (Exception ex)
        {
            throw ex;

            return null;
        }
    }

    public bool AddKeyWithValue(KeyValuePair<TKey, TValue> pair)
    {
        try
        {
            return _internalManagedItem.TryAdd(pair.Key, pair.Value);
        }
        catch (Exception ex)
        {
            throw ex;

            return false;
        }
    }

    public bool RemoveKeyWithValue(TKey key)
    {
        try
        {
            return _internalManagedItem.Remove(key);
        }
        catch (Exception ex)
        {
            throw ex;

            return false;
        }
    }
}
