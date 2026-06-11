using R3;

namespace Clicker.UI
{
    public interface IClickerView
    {
        public Subject<Unit> ClickedCommand { get; }
        
        public void UpdateGoldText(string gold) { }

        public void UpdateEnergyText(string energy) { }

        public void UpdateEnergySlider(float energy) { }
    }
}