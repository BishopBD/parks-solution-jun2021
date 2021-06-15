using OutingsApi.Controllers;
using System.Threading.Tasks;

namespace OutingsApi.Services
{
    public interface IWriteOutingsForProcessing
    {
        Task SendOuting(PostOutingCreateRequest request);
    }
}