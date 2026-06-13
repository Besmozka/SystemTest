using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "BackgroundDataSO")]
public class BackgroundData : ScriptableObject
{
    [Header("Backgrounds")]
    public Sprite ClickerImage;
    public Sprite WeatherImage;
    public Sprite DogsImage;
}