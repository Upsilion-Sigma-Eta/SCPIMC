namespace SCPIMCMain.Model.Logic;

public class ManagerService<TKey, TValue> : Object where TKey : notnull where TValue : class
{
    private Dictionary<TKey, TValue> _internal_managed_item = new Dictionary<TKey, TValue>();

    public IEnumerable<TKey> Func_Keys()
    {
        return _internal_managed_item.Keys.ToList();
    }

    public IEnumerable<TValue> Func_Values()
    {
        return _internal_managed_item.Values.ToList();
    }

    public TValue Func_TryGetValue(TKey __key)
    {
        try
        {
            TValue return_value = default(TValue);
            if (_internal_managed_item.TryGetValue(__key, out return_value))
            {
                return return_value;
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

    public IEnumerable<TKey> Func_TryGetKey(TValue __value)
    {
        try
        {
            List<TKey> found_key_with_values = new List<TKey>();

            foreach (TKey saved_key in _internal_managed_item.Keys)
            {
                if (_internal_managed_item[saved_key] == __value)
                {
                    found_key_with_values.Add(saved_key);
                }
            }

            return found_key_with_values;
        }
        catch (Exception ex)
        {
            throw ex;

            return null;
        }
    }

    public bool Func_AddKeyWithValue(KeyValuePair<TKey, TValue> __pair)
    {
        try
        {
            return _internal_managed_item.TryAdd(__pair.Key, __pair.Value);
        }
        catch (Exception ex)
        {
            throw ex;

            return false;
        }
    }

    public bool Func_RemoveKeyWithValue(TKey __key)
    {
        try
        {
            return _internal_managed_item.Remove(__key);
        }
        catch (Exception ex)
        {
            throw ex;

            return false;
        }
    }
}
