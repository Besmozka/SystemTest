using R3;

namespace Clicker
{
    public interface IGoldModel
    {
        public ReadOnlyReactiveProperty<int> CurrentGold { get; }
    
        public void AddGold(int amount) { }
    }
}