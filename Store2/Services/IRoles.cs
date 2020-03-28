using System.Threading.Tasks;

namespace Store2.Services
{
    public interface IRoles
    {
        Task GenerateRolesFromPagesAsync();

        Task AddToRoles(string applicationUserId);
    }
}
