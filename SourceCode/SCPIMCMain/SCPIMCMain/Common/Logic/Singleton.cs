namespace SCPIMCMain.Common.Logic;

public class Singleton<T> : Object where T : new()
{
    private static T _instance = new T();

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                _instance = new T();

                return _instance;
            }
        }
    }
}
