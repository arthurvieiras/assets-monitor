using AssetsMonitor.Domain.Messager;

namespace AssetsMonitor.Services.Messager
{
    interface IMessager
    {
        public void configure(Config conf);
        public void send(string message);
    }
}
