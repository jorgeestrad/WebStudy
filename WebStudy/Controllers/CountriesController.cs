﻿namespace WebStudy.Controllers
{
    using Data.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using WebStudy.Data.Repositories;

    public class CountriesController : Controller
    {
        private readonly ICountryRepository countryRepository;
        private readonly ILogger<CountriesController> logger;

        public CountriesController(ICountryRepository countryRepository, ILogger<CountriesController> logger)
        {
            this.countryRepository = countryRepository;
            this.logger = logger;
        }

        #region Countries
        public IActionResult Index()
        {
            //logger.LogError("Error al Ingresar al Listado de países");
            return View(this.countryRepository.GetCountriesWithCities());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await this.countryRepository.GetCountryWithCitiesAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                await this.countryRepository.CreateAsync(country);
                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await this.countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                await this.countryRepository.UpdateAsync(country);
                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await this.countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            try
            {
                await this.countryRepository.DeleteAsync(country);

            }
            catch { }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Cities
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await this.countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            var model = new CityViewModel { CountryId = country.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.countryRepository.AddCityAsync(model);
                return this.RedirectToAction("Details", new { id = model.CountryId });
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await this.countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            var countryId = await this.countryRepository.DeleteCityAsync(city);
            return this.RedirectToAction("Details", new { id = countryId });
        }

        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await this.countryRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(City city)
        {

            var countryId = await this.countryRepository.UpdateCityAsync(city);
            if (countryId != 0)
            {
                return this.RedirectToAction("Details", new { id = countryId });
            }

            return this.View(city);
        }
        #endregion
    }
}
