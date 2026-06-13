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
    private DogsData _dogsData;
    private IRequestsController _requestsController;
    private IBlockPanel _blockPanel;
    private IPopUpPanel _popUpPanel;
        
    private DogsRequest _currentRequest;

    private CancellationTokenSource _cts;
    private CompositeDisposable _disposables;

    public DogsTabController(IDogsView dogsView, DogsData dogsData, IBlockPanel blockPanel,
        IPopUpPanel popUpPanel, IRequestsController requestsController)
    {
        _dogsView = dogsView;
        _dogsData = dogsData;
        _requestsController = requestsController;
        _blockPanel = blockPanel;
        _popUpPanel = popUpPanel;
        
        _disposables = new CompositeDisposable();

        Init();
    }

    private void Init()
    {
        _dogsView.BreedInfoClick
            .Subscribe(SendBreedDescriptionRequest)
            .AddTo(_disposables);
    }

    public void ShowTab()
    {
        _dogsView.SetActive(true);
        
        SendBreedsInfoRequest();
    }

    public void HideTab()
    {
        _dogsView.SetActive(false);
        
        _requestsController.DequeueRequest(_currentRequest);
    }

    private void SendBreedsInfoRequest()
    {
        if (_currentRequest != null)
        {
            _requestsController.DequeueRequest(_currentRequest);
        }
        
        var url = GetBreedsInfoUrl();
        _currentRequest = new DogsRequest(url);

        _currentRequest.OnRecieveBreeds
            .Subscribe(breeds =>
            {
                _dogsView.UpdateBreedInfoItems(breeds);
                _blockPanel.Unblock();
                _currentRequest = null;
            })
            .AddTo(_disposables);
            
        _blockPanel.Block();
            
        _requestsController.EnqueueRequest(_currentRequest);
    }

    private void SendBreedDescriptionRequest(string id)
    {
        if (_currentRequest != null)
        {
            _requestsController.DequeueRequest(_currentRequest);
        }
        
        var url = GetBreedDescriptionUrl(id);
        _currentRequest = new DogsRequest(url);

        _currentRequest.OnRecieveBreedDescription
            .Subscribe(breed =>
            {
                _popUpPanel.SetDecriptionText(breed.Info.Description);
                _blockPanel.Unblock();
                _currentRequest = null;
            })
            .AddTo(_disposables);
            
        _blockPanel.Block();
            
        _requestsController.EnqueueRequest(_currentRequest);
    }

    private string GetBreedsInfoUrl()
    {
        var urlHelper = new UrlHelper($"{_dogsData.DogsApiUrl}/{_dogsData.DogsApiKey}");
        
        urlHelper.AddParameter("page[number]", 1);
        urlHelper.AddParameter("page[size]", 10);
        
        return urlHelper.Build();
    }

    private string GetBreedDescriptionUrl(string id)
    {
        return $"{_dogsData.DogsApiUrl}/{_dogsData.DogsApiKey}/{id}";
    }

    public void Dispose()
    {
        _cts?.Dispose();
        _currentRequest?.Dispose();
        _requestsController.Dispose();
        _disposables?.Dispose();
    }
}