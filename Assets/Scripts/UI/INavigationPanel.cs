using R3;

public interface INavigationPanel
{
    public Subject<Unit> ClickerCommand { get; }
    public Subject<Unit> WeatherCommand { get; }
    public Subject<Unit> DogsCommand { get; }
}