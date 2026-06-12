using R3;
using UnityEngine;

namespace Clicker
{
    public interface IClickerView
    {
        public Vector3 ButtonPosition { get; }
        public Subject<Unit> ClickedCommand { get; }
        
        public void UpdateGoldText(string gold) { }

        public void UpdateEnergyText(string energy) { }

        public void UpdateEnergySlider(float energy) { }
    }
}