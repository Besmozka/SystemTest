using System.Collections.Generic;
using R3;

namespace Dogs
{
    public interface IDogsView
    {
        public void UpdateBreedInfoItems(List<Breed> breedsInfo);
        public void SetActive(bool active);
        
        public Subject<string> BreedInfoClick { get; }
    }
}
