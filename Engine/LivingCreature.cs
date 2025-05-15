namespace Engine;

public class LivingCreature (int currentHitPoints, int maximumHitPoints) : INotifyPropertyChanged
{
    private int _currentHitPoints;
    public int CurrentHitPoints
    {
        get { return _currentHitPoints; }
        set
        {
            _currentHitPoints = value;
            OnPropertyChanged("CurrentHitPoints");
        }
    }
    public int MaximumHitPoints { get; set; } = maximumHitPoints;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged (string name)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged (this, new PropertyChangedEventArgs(name));
        }
    }
}
