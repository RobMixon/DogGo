using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        List<Dog> GetAllDogs();
        List<Dog> GetDogsByOwnerId(int ownerId);
        void AddDog(Dog newDog);
        Dog GetDogById(int id);
        void DeleteDog(int id);
        void UpdateDog(Dog dog);
    }
}