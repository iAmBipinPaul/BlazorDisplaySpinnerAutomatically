using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorDisplaySpinnerAutomatically
{
    public class BlazorDisplaySpinnerAutomaticallyHttpMessageHandler : DelegatingHandler
    {
        private readonly SpinnerService _spinnerService;
        public BlazorDisplaySpinnerAutomaticallyHttpMessageHandler(SpinnerService spinnerService)
        {
            _spinnerService = spinnerService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _spinnerService.Show();
            await Task.Delay(1000);
            var response = await base.SendAsync(request, cancellationToken);
            _spinnerService.Hide();
            return response;
        }
    }
}
