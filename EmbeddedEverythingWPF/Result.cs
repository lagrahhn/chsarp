using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EmbeddedEverythingWPF;

public class Result : INotifyPropertyChanged
{
    private string filename;
    private long size;
    private string path;
    private DateTime dateModified;


    public long Size
    {
        get { return size; }
        set
        {
            if (size != value)
            {
                size = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime DateModified
    {
        get { return dateModified; }
        set
        {
            if (dateModified != value)
            {
                dateModified = value;
                OnPropertyChanged();
            }
        }
    }

    public string Filename
    {
        get { return filename; }
        set
        {
            if (filename != value)
            {
                filename = value;
                OnPropertyChanged();
            }
        }
    }

    public string Path
    {
        get { return path; }
        set
        {
            if (path != value)
            {
                path = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}