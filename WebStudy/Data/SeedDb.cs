namespace WebStudy.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
  
    using Microsoft.AspNetCore.Identity;
    using WebStudy.Helpers;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {

            var userType = context.Find<UserType>(1);

            await this.context.Database.EnsureCreatedAsync();

            await this.CheckRoles();

            await this.CountriesAndCities();

            await this.Masters();

            await this.CheckUser("jorgeestradacorrea@hotmail.com", "Jorge Enrique", "Estrada Correa", "Customer", "3057896547", userType);
            var user = await this.CheckUser("jorgeestradacorrea@gmail.com", "Gallery", "Admin", "Admin", "3057896547", userType);

        }

        private async Task<User> CheckUser(string userName, string firstName, string lastName, string role, string telephone, UserType userType)
        {
            //Add user
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                user = await this.AddUser(userName, firstName, lastName, role, telephone, userType);
            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task<User> AddUser(string userName, string firstName, string lastName, string role, string telephone, UserType userType)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = userName,
                UserName = userName,
                Telephone = telephone,
                UserType = userType,
                CityId = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()
            };

            var result = await this.userHelper.AddUserAsync(user, "Admin123");
            if (result != IdentityResult.Success)
            {
                throw new InvalidOperationException("Could not create the user in seeder");
            }

            await this.userHelper.AddUserToRoleAsync(user, role);
            var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            await this.userHelper.ConfirmEmailAsync(user, token);

            return user;

        }

        private async Task Masters()
        {
            if (!this.context.UserTypes.Any())
            {
                this.AddUserType("Administrator");
                this.AddUserType("Operator");
                await this.context.SaveChangesAsync();
            }
        }

        private async Task CountriesAndCities()
        {
            if (!this.context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Medellín" });
                cities.Add(new City { Name = "Bogotá" });
                cities.Add(new City { Name = "Cali" });
                cities.Add(new City { Name = "Barranquilla" });
                cities.Add(new City { Name = "Pereira" });
                cities.Add(new City { Name = "Cartagena de Indias" });
                cities.Add(new City { Name = "Bucaramanga" });
                cities.Add(new City { Name = "Cúcuta" });
                cities.Add(new City { Name = "Soacha" });
                cities.Add(new City { Name = "Ibagué" });
                cities.Add(new City { Name = "Villavicencio" });
                cities.Add(new City { Name = "Santa Marta" });
                cities.Add(new City { Name = "Bello" });
                cities.Add(new City { Name = "Valledupar" });
                cities.Add(new City { Name = "Buenaventura" });
                cities.Add(new City { Name = "Pasto" });
                cities.Add(new City { Name = "Manizales" });
                cities.Add(new City { Name = "Monteria" });
                cities.Add(new City { Name = "Neiva" });

                this.context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Colombia"
                });

                var citiesArgentina = new List<City>();
                citiesArgentina.Add(new City { Name = "Córdoba" });
                citiesArgentina.Add(new City { Name = "Buenos Aires" });
                citiesArgentina.Add(new City { Name = "Rosario" });
                citiesArgentina.Add(new City { Name = "Tandil" });
                citiesArgentina.Add(new City { Name = "Salta" });
                citiesArgentina.Add(new City { Name = "Mendoza" });
                citiesArgentina.Add(new City { Name = "San Luis" });

                this.context.Countries.Add(new Country
                {
                    Cities = citiesArgentina,
                    Name = "Argentina"
                });

                var citiesVenezuela = new List<City>();
                citiesVenezuela.Add(new City { Name = "Caracas" });
                citiesVenezuela.Add(new City { Name = "Valencia" });
                citiesVenezuela.Add(new City { Name = "Maracaibo" });
                citiesVenezuela.Add(new City { Name = "Ciudad Bolivar" });
                citiesVenezuela.Add(new City { Name = "Maracay" });
                citiesVenezuela.Add(new City { Name = "Barquisimeto" });

                this.context.Countries.Add(new Country
                {
                    Cities = citiesVenezuela,
                    Name = "Venezuela"
                });

                await this.context.SaveChangesAsync();
            }
        }

        private async Task CheckRoles()
        {
            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");
        }


        private void AddUserType(string name)
        {
            this.context.UserTypes.Add(new UserType
            {
                Name = name,
            });
        }

    }
}

