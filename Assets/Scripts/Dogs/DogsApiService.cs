using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dogs;
using R3;
using Server;

public class DogsApiService : IDogsApiService
{
    private IBlockPanel _blockPanel;
    private IRequestsController _requestsController;
    private DogsData _dogsData;
    
    private Subject<List<Breed>> _onBreedsInfoGet;
    public Subject<List<Breed>> OnBreedsInfoGet => _onBreedsInfoGet;
    
    private Subject<Breed> _onBreedDescriptionGet;
    public Subject<Breed> OnBreedDescriptionGet => _onBreedDescriptionGet;
    
    private DogsRequest _currentRequest;
    private CompositeDisposable _disposables;

    public DogsApiService(IBlockPanel blockPanel, IRequestsController requestsController, DogsData dogsData)
    {
        _blockPanel = blockPanel;
        _requestsController = requestsController;
        _dogsData = dogsData;
        
        _onBreedsInfoGet = new Subject<List<Breed>>();
        _onBreedDescriptionGet = new Subject<Breed>();
        _disposables = new CompositeDisposable();
    }
    
    public void SendBreedsInfoRequest()
    {
        if (_currentRequest != null)
        {
            _requestsController.DequeueRequest(_currentRequest);
        }
        
        var url = GetBreedsInfoUrl();
        _currentRequest = new DogsRequest(url);
            
        _blockPanel.Block();
            
        _requestsController.EnqueueRequest(_currentRequest);

         _currentRequest.OnRecieveBreeds
            .Subscribe(breeds =>
            {
                _onBreedsInfoGet.OnNext(breeds);
                _blockPanel.Unblock();
                _currentRequest = null;
            })
            .AddTo(_disposables);
    }

    public void SendBreedDescriptionRequest(string id)
    {
        if (_currentRequest != null)
        {
            _requestsController.DequeueRequest(_currentRequest);
        }
        
        var url = GetBreedDescriptionUrl(id);
        _currentRequest = new DogsRequest(url);
            
        _blockPanel.Block();
            
        _requestsController.EnqueueRequest(_currentRequest);
        
        _currentRequest.OnRecieveBreedDescription
            .Subscribe(breed =>
            {
                OnBreedDescriptionGet.OnNext(breed);
                _blockPanel.Unblock();
                _currentRequest = null;
            })
            .AddTo(_disposables);
    }

    public void StopService()
    {
        _requestsController.DequeueRequest(_currentRequest);
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
        _currentRequest?.Dispose();
    }
}