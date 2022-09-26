using Microsoft.Extensions.DependencyInjection;
using WpfPaging.Services;
using WpfPaging.ViewModels;

namespace WpfPaging
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public static void Init()
        {
            var services = new ServiceCollection();
            //db context
            services.AddScoped<BankContext>();

            //pages
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainPageViewModel>();
            services.AddSingleton<BanksViewModel>();
            services.AddSingleton<BankEditViewModel>();
            services.AddSingleton<ScoreItemEditViewModel>();
            services.AddSingleton<PlansViewModel>();
            services.AddSingleton<PlanEditViewModel>();

            services.AddScoped<LogPageViewModel>();

            //services
            services.AddSingleton<PageService>();
            services.AddSingleton<MessageBus>();
            services.AddScoped<CreditPlanSelectorService>();

            _provider = services.BuildServiceProvider();

            //check up services on start up
            foreach (var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }
        // easy binding 
        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();
        public LogPageViewModel LogPageViewModel => _provider.GetRequiredService<LogPageViewModel>();
        public MainPageViewModel MainPageViewModel => _provider.GetRequiredService<MainPageViewModel>();
        public PlansViewModel PlansViewModel=> _provider.GetRequiredService<PlansViewModel>();
        public BanksViewModel BanksViewModel => _provider.GetRequiredService<BanksViewModel>();
        public BankEditViewModel BankEditViewModel => _provider.GetRequiredService<BankEditViewModel>();
        public ScoreItemEditViewModel ScoreItemEditViewModel => _provider.GetRequiredService<ScoreItemEditViewModel>();
        public PlanEditViewModel PlanEditViewModel => _provider.GetRequiredService<PlanEditViewModel>();
    }
}
