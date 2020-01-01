using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;

namespace BlazorDisplaySpinnerAutomatically
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SpinnerService>();
            services.AddScoped<BlazorDisplaySpinnerAutomaticallyHttpMessageHandler>();
            services.AddScoped(s =>
            {
                var blazorDisplaySpinnerAutomaticallyHttpMessageHandler = s.GetRequiredService<BlazorDisplaySpinnerAutomaticallyHttpMessageHandler>();
                var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");
                var wasmHttpMessageHandler = (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);
                var httpClient = new HttpClient(wasmHttpMessageHandler);

                blazorDisplaySpinnerAutomaticallyHttpMessageHandler.InnerHandler = wasmHttpMessageHandler;
                var uriHelper = s.GetRequiredService<NavigationManager>();
                return new HttpClient(blazorDisplaySpinnerAutomaticallyHttpMessageHandler)
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
