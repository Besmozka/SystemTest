using System.Collections;
using System.Collections.Generic;
using Clicker;
using R3;
using UnityEngine;

public class EnergyModel : IEnergyModel
{
    private int _maxEnergy;
    public int MaxEnergy => _maxEnergy;
    
    private ReactiveProperty<int> _currentEnergy;
    public ReadOnlyReactiveProperty<int> CurrentEnergy => _currentEnergy;

    public EnergyModel(ClickerData clickerData)
    {
        _maxEnergy = clickerData.MaxEnergy;
        
        _currentEnergy = new ReactiveProperty<int>(_maxEnergy);
    }
    
    public void AddEnergy(int energyToAdd)
    {
        _currentEnergy.Value += energyToAdd;

        if (_currentEnergy.Value > _maxEnergy)
        {
            _currentEnergy.Value = _maxEnergy;    
        }
    }

    public void RemoveEnergy(int energyToRemove)
    {
        if (_currentEnergy.Value == 0) return;
        
        _currentEnergy.Value -= energyToRemove;
        
        if (_currentEnergy.Value < 0) _currentEnergy.Value = 0;
    }
}
