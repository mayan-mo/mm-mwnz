using Mwnz.Client.Services;

namespace MM.Mwnz.Services
{
    public interface IMwnzClientService
    {
        Task<Company> GetCompany(double id);
    }

    public class MwnzClientService: IMwnzClientService
    {
        private readonly MwnzClient _mwnzClient;

        public MwnzClientService(MwnzClient mwnzClient)
        {
            _mwnzClient = mwnzClient;
        }

        public async Task<Company> GetCompany(double id)
        {
            return await _mwnzClient.CompaniesAsync(id);
        }
    }
}
