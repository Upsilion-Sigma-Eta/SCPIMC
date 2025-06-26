using System;
using SCPIMCMain.Common.Logic;

namespace SCPIMCMain.Model.Logic;

public class TabItemModel : NotifyPropertyChanged
{
    private string _header;
    private bool _isEnabled;
    private object _content;

    public TabItemModel()
    {
        Header = "";
        IsEnabled = false;
        Content = null;
    }

    public TabItemModel(string header, bool isEnabled)
    {
        Header = header;
        IsEnabled = isEnabled;
        Content = null;
    }

    public TabItemModel(string header, object content)
    {
        Header = header;
        IsEnabled = true;
        Content = content;
    }

    public TabItemModel(string header, bool isEnabled, object content)
    {
        Header = header;
        IsEnabled = isEnabled;
        Content = content;
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
        get => _isEnabled;
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
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
