namespace VBY.MiniGame;
public abstract class MainGame
{
    public event EventHandler? PreStart, PostStart;
    public event EventHandler? PreStop, PostStop;
    private bool _Started;
    public bool Started
    {
        get => _Started;
        internal set
        {
            if(value)
            {
                PreStart?.Invoke(this, EventArgs.Empty);
                _Started = true;
                Start();
                PostStart?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                PreStop?.Invoke(this, EventArgs.Empty);
                _Started = false;
                Stop();
                PostStop?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public void Start()
    {
        Started = true;
    }
    public void Stop()
    {
        Started = false;
    }
}