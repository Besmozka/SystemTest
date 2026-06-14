using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Dogs;
using R3;
using Server;

public class DogsTabController : IDisposable, ITabController
{
    private IDogsView _dogsView;
    private IDogsApiService _dogsApiService;
    private IPopUpPanel _popUpPanel;

    private CancellationTokenSource _cts;
    private CompositeDisposable _disposables;

    public DogsTabController(IDogsView dogsView, IDogsApiService dogsApiService, IPopUpPanel popUpPanel)
    {
        _dogsView = dogsView;
        _dogsApiService = dogsApiService;
        _popUpPanel = popUpPanel;
        
        _disposables = new CompositeDisposable();

        Init();
    }

    private void Init()
    {
        _dogsApiService.OnBreedsInfoGet
            .Subscribe(_dogsView.UpdateBreedInfoItems)
            .AddTo(_disposables);

        _dogsApiService.OnBreedDescriptionGet
            .Subscribe(breed => _popUpPanel.SetDecriptionText(breed.Info.Description))
            .AddTo(_disposables);
        
        _dogsView.BreedInfoClick
            .Subscribe(_dogsApiService.SendBreedDescriptionRequest)
            .AddTo(_disposables);
    }

    public void ShowTab()
    {
        _dogsView.SetActive(true);
        
        _dogsApiService.SendBreedsInfoRequest();
    }

    public void HideTab()
    {
        _dogsView.SetActive(false);
        
        _dogsApiService.StopService();
    }

    public void Dispose()
    {
        _cts?.Dispose();
        _disposables?.Dispose();
    }
}