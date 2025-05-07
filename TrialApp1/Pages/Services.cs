using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TrialApp1.Pages;

namespace TrialApp1
{
    // Weather Service
    public interface IWeatherService
    {
        Task<List<WeatherForecast>> GetWeatherForecastsAsync();
    }

    public class WeatherService : IWeatherService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "WeatherForecasts";

        public WeatherService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<List<WeatherForecast>> GetWeatherForecastsAsync()
        {
            if (_cache.TryGetValue(CacheKey, out List<WeatherForecast> forecasts))
            {
                return forecasts;
            }

            // In a real scenario, this would call an external weather API
            // For now, we'll create some sample data
            var result = new List<WeatherForecast>
            {
                new WeatherForecast
                {
                    Day = "Today",
                    Icon = "sun",
                    ColorClass = "warning",
                    Temperature = 28,
                    Description = "Sunny",
                    SunlightHours = 9.5,
                    ExpectedEnergy = 24.8
                },
                new WeatherForecast
                {
                    Day = "Tomorrow",
                    Icon = "cloud-sun",
                    ColorClass = "primary",
                    Temperature = 26,
                    Description = "Partly Cloudy",
                    SunlightHours = 7.5,
                    ExpectedEnergy = 19.2
                },
                new WeatherForecast
                {
                    Day = DayAfterTomorrow(),
                    Icon = "sun",
                    ColorClass = "warning",
                    Temperature = 29,
                    Description = "Sunny",
                    SunlightHours = 9.8,
                    ExpectedEnergy = 25.3
                },
                new WeatherForecast
                {
                    Day = DayAfterTomorrowPlus1(),
                    Icon = "cloud",
                    ColorClass = "secondary",
                    Temperature = 24,
                    Description = "Cloudy",
                    SunlightHours = 5.2,
                    ExpectedEnergy = 13.1
                }
            };

            // Cache the forecasts for 1 hour
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            _cache.Set(CacheKey, result, cacheOptions);

            return result;
        }

        private string DayAfterTomorrow()
        {
            return DateTime.Now.AddDays(2).ToString("dddd");
        }

        private string DayAfterTomorrowPlus1()
        {
            return DateTime.Now.AddDays(3).ToString("dddd");
        }
    }

    // Project Service
    public interface IProjectService
    {
        Task<List<Project>> GetRecentProjectsAsync(int count);
    }

    public class ProjectService : IProjectService
    {
        public async Task<List<Project>> GetRecentProjectsAsync(int count)
        {
            // In a real scenario, this would query a database
            var projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Title = "Cape Town Villa Solar Installation",
                    Type = "Residential",
                    Location = "Cape Town, Western Cape",
                    Description = "A high-efficiency solar system installed on a luxury villa in Cape Town, providing 90% of the home's energy needs.",
                    SystemSize = 12.5,
                    AnnualProduction = 18500,
                    Co2Offset = 12.8,
                    ImageUrl = "/images/projects/cape-town-villa.jpg"
                },
                new Project
                {
                    Id = 2,
                    Title = "Johannesburg Office Complex",
                    Type = "Commercial",
                    Location = "Sandton, Gauteng",
                    Description = "Large-scale commercial installation providing significant energy savings for a multi-story office complex.",
                    SystemSize = 75.0,
                    AnnualProduction = 112000,
                    Co2Offset = 78.4,
                    ImageUrl = "/images/projects/joburg-office.jpg"
                },
                new Project
                {
                    Id = 3,
                    Title = "Durban Beachfront Hotel",
                    Type = "Commercial",
                    Location = "Umhlanga, KwaZulu-Natal",
                    Description = "Comprehensive solar solution for a beachfront hotel, reducing electricity costs by over 60%.",
                    SystemSize = 45.2,
                    AnnualProduction = 67800,
                    Co2Offset = 47.5,
                    ImageUrl = "/images/projects/durban-hotel.jpg"
                },
                new Project
                {
                    Id = 4,
                    Title = "Pretoria Family Home",
                    Type = "Residential",
                    Location = "Centurion, Gauteng",
                    Description = "Complete off-grid solution for a family home, including battery storage system for 24/7 power availability.",
                    SystemSize = 8.5,
                    AnnualProduction = 12750,
                    Co2Offset = 8.9,
                    ImageUrl = "/images/projects/pretoria-home.jpg"
                },
                new Project
                {
                    Id = 5,
                    Title = "Bloemfontein Shopping Mall",
                    Type = "Commercial",
                    Location = "Bloemfontein, Free State",
                    Description = "Extensive rooftop solar installation for a major shopping mall, significantly reducing operational costs.",
                    SystemSize = 120.0,
                    AnnualProduction = 180000,
                    Co2Offset = 126.0,
                    ImageUrl = "/images/projects/bloem-mall.jpg"
                },
                new Project
                {
                    Id = 6,
                    Title = "Port Elizabeth School",
                    Type = "Commercial",
                    Location = "Gqeberha, Eastern Cape",
                    Description = "Solar installation for an educational institution, providing clean energy and educational opportunities.",
                    SystemSize = 35.0,
                    AnnualProduction = 52500,
                    Co2Offset = 36.8,
                    ImageUrl = "/images/projects/pe-school.jpg"
                }
            };

            return projects.Take(count).ToList();
        }
    }

    // Tips Service
    public interface ITipService
    {
        Task<List<EnergyTip>> GetEnergyTipsAsync();
    }

    public class TipService : ITipService
    {
        public async Task<List<EnergyTip>> GetEnergyTipsAsync()
        {
            return new List<EnergyTip>
            {
                new EnergyTip
                {
                    Icon = "sun",
                    Title = "Panel Placement",
                    Description = "Position your solar panels where they can receive maximum sunlight throughout the day, typically facing north in South Africa."
                },
                new EnergyTip
                {
                    Icon = "droplet",
                    Title = "Regular Cleaning",
                    Description = "Clean your solar panels every 3-6 months to remove dust and debris that can reduce efficiency by up to 25%."
                },
                new EnergyTip
                {
                    Icon = "tree",
                    Title = "Trim Nearby Trees",
                    Description = "Keep trees trimmed to prevent shadows on your panels, as even partial shading can significantly reduce energy production."
                },
                new EnergyTip
                {
                    Icon = "battery-charging",
                    Title = "Battery Maintenance",
                    Description = "If you have a battery system, follow manufacturer guidelines for maintenance to maximize lifespan and efficiency."
                }
            };
        }
    }

    // Testimonial Service
    public interface ITestimonialService
    {
        Task<List<Testimonial>> GetTestimonialsAsync(int count);
        Task SubscribeToNewsletterAsync(SubscriptionRequest request);
    }

    public class TestimonialService : ITestimonialService
    {
        public async Task<List<Testimonial>> GetTestimonialsAsync(int count)
        {
            var testimonials = new List<Testimonial>
            {
                new Testimonial
                {
                    Id = 1,
                    CustomerName = "Thabo Mbeki",
                    Location = "Johannesburg",
                    Comment = "Since installing solar panels, my electricity bill has decreased by 75%. The team was professional and completed the installation in just two days.",
                    Rating = 5,
                    PhotoUrl = "/images/testimonials/thabo.jpg"
                },
                new Testimonial
                {
                    Id = 2,
                    CustomerName = "Sarah Johnson",
                    Location = "Cape Town",
                    Comment = "I was skeptical at first, but now I'm a believer! Our business has saved thousands on electricity costs, and the system paid for itself in just 18 months.",
                    Rating = 5,
                    PhotoUrl = "/images/testimonials/sarah.jpg"
                },
                new Testimonial
                {
                    Id = 3,
                    CustomerName = "Kagiso Rabada",
                    Location = "Pretoria",
                    Comment = "The best investment I've made for my home. The customer service was excellent, and they provided clear information throughout the entire process.",
                    Rating = 4,
                    PhotoUrl = "/images/testimonials/kagiso.jpg"
                },
                new Testimonial
                {
                    Id = 4,
                    CustomerName = "Anele Mdoda",
                    Location = "Durban",
                    Comment = "As a business owner, going solar has been a game-changer. Not only are we saving money, but our customers appreciate our commitment to sustainability.",
                    Rating = 5,
                    PhotoUrl = "/images/testimonials/anele.jpg"
                },
                new Testimonial
                {
                    Id = 5,
                    CustomerName = "Pieter van der Merwe",
                    Location = "Stellenbosch",
                    Comment = "From the initial consultation to the final installation, the team was professional and knowledgeable. Highly recommend their services!",
                    Rating = 4,
                    PhotoUrl = "/images/testimonials/pieter.jpg"
                },
                new Testimonial
                {
                    Id = 6,
                    CustomerName = "Nomzamo Mbatha",
                    Location = "Port Elizabeth",
                    Comment = "The battery backup system has been a lifesaver during load shedding. Now we have power when our neighbors don't!",
                    Rating = 5,
                    PhotoUrl = "/images/testimonials/nomzamo.jpg"
                },
                new Testimonial
                {
                    Id = 7,
                    CustomerName = "David Smith",
                    Location = "Bloemfontein",
                    Comment = "We're thrilled with our solar installation. The energy production has exceeded our expectations, and the system looks great on our roof.",
                    Rating = 5,
                    PhotoUrl = "/images/testimonials/david.jpg"
                },
                new Testimonial
                {
                    Id = 8,
                    CustomerName = "Precious Moloi",
                    Location = "Kimberley",
                    Comment = "Excellent service from start to finish. The team was respectful of our property and completed the work on schedule.",
                    Rating = 4,
                    PhotoUrl = "/images/testimonials/precious.jpg"
                },
                new Testimonial
                {
                    Id = 9,
                    CustomerName = "Trevor Noah",
                    Location = "Cape Town",
                    Comment = "I installed solar panels on my family home in South Africa and it's been an incredible investment. The sunny climate makes it perfect for solar energy.",
                    Rating = 5,
                    PhotoUrl = "/images/testimonials/trevor.jpg"
                },
                new Testimonial
                {
                    Id = 10,
                    CustomerName = "Zanele Dlamini",
                    Location = "Nelspruit",
                    Comment = "The monitoring app is fantastic - I can track our energy production in real-time. It's satisfying to see how much we're saving every day.",
                    Rating = 5,
                    PhotoUrl = "/images/testimonials/zanele.jpg"
                }
            };

            return testimonials.Take(count).ToList();
        }

        public async Task SubscribeToNewsletterAsync(SubscriptionRequest request)
        {
            // In a real scenario, this would add the email to a newsletter database or service
            // For now, we'll just simulate success
            await Task.Delay(500); // Simulate processing time

            // In a real implementation, you might throw exceptions for invalid emails, etc.
            if (string.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentException("Email is required");
            }
        }
    }

    // Quote Service
    public interface IQuoteService
    {
        Task<QuoteResponse> CalculateQuoteAsync(QuoteRequest request);
    }

    public class QuoteService : IQuoteService
    {
        public async Task<QuoteResponse> CalculateQuoteAsync(QuoteRequest request)
        {
            // In a real scenario, this would calculate a quote based on various factors
            // For now, we'll create a simple calculation

            double systemSize;
            double costPerKw;

            // Determine system size based on monthly bill
            if (request.InstallationType == "Home")
            {
                systemSize = Math.Round(request.MonthlyElectricityBill / 500 * 3, 1); // Rough estimate
                costPerKw = 18000; // ZAR per kW for residential
            }
            else // Business
            {
                systemSize = Math.Round(request.MonthlyElectricityBill / 1000 * 5, 1); // Businesses typically need larger systems
                costPerKw = 16000; // ZAR per kW for commercial (economies of scale)
            }

            // Adjust based on roof area (basic implementation)
            double roofAreaNeeded = systemSize * 7; // About 7 sq meters per kW
            if (roofAreaNeeded > request.RoofArea)
            {
                systemSize = Math.Round(request.RoofArea / 7, 1); // Limited by roof space
            }

            double totalCost = systemSize * costPerKw;
            double annualProduction = systemSize * 1600; // Approximately 1600 kWh per kW in South Africa
            double monthlySavings = annualProduction / 12 * 2.2; // Approx 2.2 ZAR per kWh
            double paybackPeriod = totalCost / (monthlySavings * 12);

            string recommendedSystem;
            if (systemSize <= 5)
            {
                recommendedSystem = "EcoHome Basic";
            }
            else if (systemSize <= 10)
            {
                recommendedSystem = "EcoHome Premium";
            }
            else if (systemSize <= 25)
            {
                recommendedSystem = "Business Efficiency";
            }
            else
            {
                recommendedSystem = "Enterprise Power";
            }

            return new QuoteResponse
            {
                EstimatedSystemSize = systemSize,
                EstimatedCost = totalCost,
                EstimatedSavingsMonthly = monthlySavings,
                EstimatedPaybackPeriod = Math.Round(paybackPeriod, 1),
                EstimatedAnnualProduction = annualProduction,
                RecommendedSystem = recommendedSystem
            };
        }
    }
}