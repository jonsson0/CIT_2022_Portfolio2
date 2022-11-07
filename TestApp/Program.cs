using DataLayer;

var ds = new DataService();

var list = ds.getTitles();

var Stitle = ds.getTItle("tt0052520");

Console.WriteLine("here is a title::");
Console.WriteLine(Stitle.PrimaryTitle);

// .Date.ToString("yyyy-MM-dd")

Console.WriteLine("Heres a list of titles:");
foreach (var title in list)
{
    Console.WriteLine(title.TitleId);

}