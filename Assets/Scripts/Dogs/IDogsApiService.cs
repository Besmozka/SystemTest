
using System.Collections.Generic;
using R3;

public interface IDogsApiService
{
    public Subject<List<Breed>> OnBreedsInfoGet { get; }
    public Subject<Breed> OnBreedDescriptionGet { get; }
    public void SendBreedsInfoRequest();
    public void SendBreedDescriptionRequest(string id);
    public void StopService();
}