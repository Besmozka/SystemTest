using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private WeatherData weatherData;
    [SerializeField] private ClickerData clickerData;
    [SerializeField] private DogsData dogsData;
    
    public override void InstallBindings()
    {
        Container.BindInstance(weatherData).AsCached();
        Container.BindInstance(clickerData).AsCached();
        Container.BindInstance(dogsData).AsCached();
    }
}