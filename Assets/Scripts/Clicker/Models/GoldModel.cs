using Clicker;
using R3;

public class GoldModel : IGoldModel
{
    private ReactiveProperty<int> _currentGold;
    public ReadOnlyReactiveProperty<int> CurrentGold => _currentGold;

    public GoldModel()
    {
        _currentGold = new ReactiveProperty<int>(0);
    }
    
    public void AddGold(int amount)
    {
        _currentGold.Value += amount;
    }
}
