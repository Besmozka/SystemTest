using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ClickerData", menuName = "ClickerDataSO")]
public class ClickerData : ScriptableObject
{
    [Header("Gold data")]
    public int ClickGoldCost;
    public float AutoClickTime;
    
    
    [Header("Energy data")]
    public int MaxEnergy;
    public int ClickEnergyCost;
    public float EnergyIncreaseTime;
    [FormerlySerializedAs("EnergyIncreaseValue")] public int EnergyChargeValue;
}
