using DataLayer;

var ds = new DataService();

var list = ds.getTitles();

var Stitle = ds.getTItle(0);

Console.WriteLine("here:");
Console.WriteLine(list.First());

// .Date.ToString("yyyy-MM-dd")

foreach (var title in list)
{
    Console.WriteLine(title.PrimaryTitle);

}