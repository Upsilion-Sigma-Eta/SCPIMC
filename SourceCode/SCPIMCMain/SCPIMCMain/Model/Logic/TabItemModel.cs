using SCPIMCMain.Common.Logic;

namespace SCPIMCMain.Model.Logic;

public class TabItemModel : NotifyPropertyChanged
{
    private string _header;
    private bool _is_enabled;
    private object _content;

    public TabItemModel()
    {
        Header = "";
        IsEnabled = false;
        Content = null;
    }

    public TabItemModel(string __header, bool __isEnabled)
    {
        Header = __header;
        IsEnabled = __isEnabled;
        Content = null;
    }

    public TabItemModel(string __header, object __content)
    {
        Header = __header;
        IsEnabled = true;
        Content = __content;
    }

    public TabItemModel(string __header, bool __isEnabled, object __content)
    {
        Header = __header;
        IsEnabled = __isEnabled;
        Content = __content;
    }

    public string Header
    {
        get => _header;
        set
        {
            if (_header != value)
            {
                _header = value;
                OnPropertyChangedEventHandler(this, nameof(Header));
            }
        }
    }

    public bool IsEnabled
    {
        get => _is_enabled;
        set
        {
            if (_is_enabled != value)
            {
                _is_enabled = value;
                OnPropertyChangedEventHandler(this, nameof(IsEnabled));
            }
        }
    }

    public object Content
    {
        get => _content;
        set
        {
            if (_content != value)
            {
                _content = value;
                OnPropertyChangedEventHandler(this, nameof(Content));
            }
        }
    }
}
