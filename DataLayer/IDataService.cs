using DataLayer.Models;

namespace DataLayer
{
    public interface IDataService
    {
        Title getTItle(int id);
        public List<Title> getTitles();
    }
}