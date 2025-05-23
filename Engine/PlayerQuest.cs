﻿namespace Engine;

public class PlayerQuest(Quest details) : INotifyPropertyChanged
{
    private Quest _details = details;
    private bool _isCompleted = false;

    public Quest Details
    {
        get { return _details; }
        set
        {
            _details = value;
            OnPropertyChanged("Details");
        }
    }

    public bool IsCompleted
    {
        get { return _isCompleted; }
        set
        {
            _isCompleted = value;
            OnPropertyChanged("IsCompleted");
            OnPropertyChanged("Name");
        }
    }

    public string Name
    {
        get { return Details.Name; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged (string name)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
