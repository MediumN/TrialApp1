using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrialApp1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWeatherService _weatherService;
        private readonly IProjectService _projectService;
        private readonly ITipService _tipService;
        private readonly ITestimonialService _testimonialService;
        private readonly IQuoteService _quoteService;

        public IndexModel(
            IWeatherService weatherService,
            IProjectService projectService,
            ITipService tipService,
            ITestimonialService testimonialService,
            IQuoteService quoteService)
        {
            _weatherService = weatherService;
            _projectService = projectService;
            _tipService = tipService;
            _testimonialService = testimonialService;
            _quoteService = quoteService;
        }

        // Welcome Section Properties
        public string WelcomeHeadline { get; set; } = "Power Your Future with Solar Energy";
        public string WelcomeSubheadline { get; set; } = "South Africa's premier renewable energy provider offering reliable solar solutions for homes and businesses.";
        public string WelcomeVideoUrl { get; set; } = "videos/solar.mp4";

        // Login Section Properties
        public string LoginSectionTitle { get; set; } = "Sign In to Your Account";
        public string LoginSectionDescription { get; set; } = "Access your solar energy dashboard, monitor your system's performance, and manage your account settings.";
        public string LoginButtonText { get; set; } = "Sign In";
        public string RegisterButtonText { get; set; } = "Create Account";
        public string ZuluText { get; set; } = "Ngena ngemvume";
        public string XhosaText { get; set; } = "Ngena kwi-akhawunti yakho";
        public string AfrikaansText { get; set; } = "Teken in";

        // Weather Section Properties
        public string WeatherTitle { get; set; } = "Solar Energy Forecast";
        public string SunlightLabel { get; set; } = "Sunlight";
        public string EnergyLabel { get; set; } = "Energy";
        public List<WeatherForecast> WeatherForecasts { get; set; }

        // Quote Section Properties
        public string QuoteSectionTitle { get; set; } = "Get Your Free Quote";
        public string QuoteSectionDescription { get; set; } = "Find out how much you can save by switching to solar energy. We offer customized solutions for homes and businesses.";
        public string HomeQuoteButtonText { get; set; } = "Home Installation";
        public string BusinessQuoteButtonText { get; set; } = "Business Solution";

        // Quote Form Properties
        [BindProperty]
        public QuoteRequest QuoteRequest { get; set; }
        public bool ShowQuoteResult { get; set; }
        public QuoteResponse QuoteResult { get; set; }

        // Projects Section Properties
        public string ProjectsSectionTitle { get; set; } = "Our Recent Projects";
        public string ProjectsSectionDescription { get; set; } = "Browse through our gallery of successfully completed solar installations across South Africa.";
        public List<Project> Projects { get; set; }

        // Tips Section Properties
        public string TipsTitle { get; set; } = "Solar Energy Tips";
        public string TipsSubtitle { get; set; } = "Maximize your solar energy system performance with these useful tips.";
        public List<EnergyTip> EnergyTips { get; set; }

        // Testimonials Section Properties
        public string TestimonialsSectionTitle { get; set; } = "What Our Customers Say";
        public string TestimonialsSectionDescription { get; set; } = "Don't just take our word for it. See what our satisfied customers have to say about our services.";
        public List<Testimonial> Testimonials { get; set; }

        // Subscription Section Properties
        public string SubscriptionTitle { get; set; } = "Stay Updated";
        public string SubscriptionDescription { get; set; } = "Subscribe to our newsletter for the latest solar energy news and special offers.";

        [BindProperty]
        public SubscriptionRequest SubscriptionRequest { get; set; }
        public string SubscriptionMessage { get; set; }
        public bool SubscriptionSuccess { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Load weather forecasts
            WeatherForecasts = await _weatherService.GetWeatherForecastsAsync();

            // Load projects
            Projects = await _projectService.GetRecentProjectsAsync(6);

            // Load energy tips
            EnergyTips = await _tipService.GetEnergyTipsAsync();

            // Load testimonials
            Testimonials = await _testimonialService.GetTestimonialsAsync(10);

            QuoteRequest = new QuoteRequest();
            SubscriptionRequest = new SubscriptionRequest();

            return Page();
        }

        public async Task<IActionResult> OnPostGetQuoteAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadPageDataAsync();
                return Page();
            }

            QuoteResult = await _quoteService.CalculateQuoteAsync(QuoteRequest);
            ShowQuoteResult = true;

            await LoadPageDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostSubscribeAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadPageDataAsync();
                return Page();
            }

            try
            {
                await _testimonialService.SubscribeToNewsletterAsync(SubscriptionRequest);
                SubscriptionSuccess = true;
                SubscriptionMessage = "Thank you for subscribing! You will receive our latest news and offers.";
            }
            catch (Exception ex)
            {
                SubscriptionSuccess = false;
                SubscriptionMessage = "Sorry, there was an error processing your subscription. Please try again.";
            }

            await LoadPageDataAsync();
            return Page();
        }

        private async Task LoadPageDataAsync()
        {
            WeatherForecasts = await _weatherService.GetWeatherForecastsAsync();
            Projects = await _projectService.GetRecentProjectsAsync(6);
            EnergyTips = await _tipService.GetEnergyTipsAsync();
            Testimonials = await _testimonialService.GetTestimonialsAsync(10);
        }
    }

    public class WeatherForecast
    {
        public string Day { get; set; }
        public string Icon { get; set; }
        public string ColorClass { get; set; }
        public double Temperature { get; set; }
        public string Description { get; set; }
        public double SunlightHours { get; set; }
        public double ExpectedEnergy { get; set; }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public double SystemSize { get; set; }
        public double AnnualProduction { get; set; }
        public double Co2Offset { get; set; }
        public string ImageUrl { get; set; }
    }

    public class EnergyTip
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Testimonial
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class QuoteRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string InstallationType { get; set; } // Home or Business
        public double RoofArea { get; set; }
        public double MonthlyElectricityBill { get; set; }
        public string Province { get; set; }
    }

    public class QuoteResponse
    {
        public double EstimatedSystemSize { get; set; }
        public double EstimatedCost { get; set; }
        public double EstimatedSavingsMonthly { get; set; }
        public double EstimatedPaybackPeriod { get; set; }
        public double EstimatedAnnualProduction { get; set; }
        public string RecommendedSystem { get; set; }
    }

    public class SubscriptionRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public bool AcceptTerms { get; set; }
    }
}