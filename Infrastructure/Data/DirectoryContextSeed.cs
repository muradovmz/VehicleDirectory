using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class DirectoryContextSeed
    {
        public static async Task SeedAsync(DirectoryContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.FuelTypes.Any())
                {
                    var fuelTypesData = File.ReadAllText("../Infrastructure/Data/SeedData/fueltypes.json");

                    var fueltypes = JsonSerializer.Deserialize<List<FuelType>>(fuelTypesData);

                    foreach (var item in fueltypes)
                    {
                        context.FuelTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Manufacturers.Any())
                {
                    var manufacturersData = File.ReadAllText("../Infrastructure/Data/SeedData/manufacturers.json");

                    var manufacturers = JsonSerializer.Deserialize<List<Manufacturer>>(manufacturersData);

                    foreach (var item in manufacturers)
                    {
                        context.Manufacturers.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Colors.Any())
                {
                    var colorsData = File.ReadAllText("../Infrastructure/Data/SeedData/colors.json");

                    var colors = JsonSerializer.Deserialize<List<Color>>(colorsData);

                    foreach (var item in colors) 
                    {
                        context.Colors.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Models.Any())
                {
                    var modelsData = File.ReadAllText("../Infrastructure/Data/SeedData/models.json");

                    var models = JsonSerializer.Deserialize<List<Model>>(modelsData);

                    foreach (var item in models)
                    {
                        context.Models.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Owners.Any())
                {
                    var ownersData = File.ReadAllText("../Infrastructure/Data/SeedData/owners.json");

                    var owners = JsonSerializer.Deserialize<List<Owner>>(ownersData);

                    foreach (var item in owners)
                    {
                        context.Owners.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Vehicles.Any())
                {
                    var vehiclesData = File.ReadAllText("../Infrastructure/Data/SeedData/vehicles.json");

                    var vehicles = JsonSerializer.Deserialize<List<VehicleSeedModel>>(vehiclesData);

                    foreach (var item in vehicles)
                    {
                        var pictureFileName = item.PictureUrl.Substring(16);
                        var vehicle = new Vehicle
                        {
                            ModelId=item.ModelId,
                            VinCode=item.VinCode,
                            StateNumberPlate=item.StateNumberPlate,
                            ManufactureDate=item.ManufactureDate,
                            ColorId=item.ColorId
                        };
                        vehicle.AddPhoto(item.PictureUrl,pictureFileName);
                        context.Vehicles.Add(vehicle);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.VehicleFuelTypes.Any())
                {
                    var vehicleFuelTypesData = File.ReadAllText("../Infrastructure/Data/SeedData/vehiclefueltypes.json");

                    var vehicleFuelTypes = JsonSerializer.Deserialize<List<VehicleFuelType>>(vehicleFuelTypesData);

                    foreach (var item in vehicleFuelTypes)
                    {
                        context.VehicleFuelTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.VehicleOwners.Any())
                {
                    var vehicleOwnersData = File.ReadAllText("../Infrastructure/Data/SeedData/vehicleowners.json");

                    var vehicleOwners = JsonSerializer.Deserialize<List<VehicleOwner>>(vehicleOwnersData);

                    foreach (var item in vehicleOwners)
                    {
                        context.VehicleOwners.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<DirectoryContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}