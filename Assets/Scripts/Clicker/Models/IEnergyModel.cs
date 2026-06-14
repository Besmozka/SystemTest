using R3;

namespace Clicker
{
    public interface IEnergyModel
    {
        public int MaxEnergy { get; }
        public ReadOnlyReactiveProperty<int> CurrentEnergy { get; }
        
        public void AddEnergy(int energyToAdd) { }

        public bool TryRemoveEnergy(int energyToRemove)
        {
            return false;
        }
    }
}