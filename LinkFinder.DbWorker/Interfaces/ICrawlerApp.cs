using LinkFinder.DbWorker.Models;
using System.Threading.Tasks;

namespace LinkFinder.DbWorker.Interfaces
{
    public interface ICrawlerApp
    {
        Task<Test> StartWork(string url);
    }
}
