using DataLayer.Models;

namespace DataLayer
{
    public interface IDataService
    {
        Title getTItle(string id);
        public List<Title> getTitles();
    }
}