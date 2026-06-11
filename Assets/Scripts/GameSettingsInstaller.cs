using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private ClickerData clickerData;
    
    public override void InstallBindings()
    {
        Container.BindInstance(clickerData).AsSingle();
    }
}