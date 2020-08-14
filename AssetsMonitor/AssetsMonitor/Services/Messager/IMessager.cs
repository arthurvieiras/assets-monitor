using AssetsMonitor.Domain.Messager;
using System.Threading.Tasks;

namespace AssetsMonitor.Services.Messager
{
    interface IMessager
    {
        public void configure(EmailConfiguration conf);
        public Task sendAsync(string message, string subject);
    }
}
