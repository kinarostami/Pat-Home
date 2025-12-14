using System.Threading.Tasks;

namespace Eshop.Infrastructure.Recaptcha
{
    public interface IGoogleRecaptcha
    {
        Task<bool> IsSatisfy();
    }
}