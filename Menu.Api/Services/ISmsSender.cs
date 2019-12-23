using System.Threading.Tasks;

namespace Menu.Api.Services
{
    public interface ISmsSender
    {
        Task<string> Send(string phoneNumber, string message);
    }
}