using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    [Authorize]
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepository;
        private readonly IOwnerRepository _ownerRepository;

        public DogsController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepository = dogRepository;
            _ownerRepository = ownerRepository;
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: DogsController
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            List<Dog> dogs = _dogRepository.GetDogsByOwnerId(ownerId);
            return View(dogs);
        }
        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }


        // LOOK AT THIS
        public ActionResult Create()
        {
            // We use a view model because we need the list of Owners in the Create view
            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = new Dog(),
                Owners = _ownerRepository.GetAllOwners(),
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Dog dog)
        {
            try
            {
                // LOOK AT THIS
                //  Let's save a new dog
                //  This new dog may or may not have Notes and/or an ImageUrl
                dog.OwnerId = GetCurrentUserId();
                _dogRepository.AddDog(dog);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // LOOK AT THIS
                //  When something goes wrong we return to the view
                //  BUT our view expects a DogFormViewModel object...so we'd better give it one
                DogFormViewModel vm = new DogFormViewModel()
                {
                    Dog = dog,
                    Owners = _ownerRepository.GetAllOwners(),
                };

                return View(vm);
            }
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }


        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepository.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);

            return View(dog);
        }

        // POST: Owners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepository.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }
        // helper method to get current users id

    }
}
