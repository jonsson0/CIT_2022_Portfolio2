using DataLayer;

var ds = new DataService();

var list = ds.getTitles();
var PersonList = ds.getPerson();


var Stitle = ds.getTitle("tt0052520");
var TestPerson = ds.getPerson("nm0000001");


Console.WriteLine("here is a title::");
Console.WriteLine(Stitle.PrimaryTitle);

// .Date.ToString("yyyy-MM-dd")

Console.WriteLine("Heres a list of titles:");
foreach (var title in list)
{
    Console.WriteLine(title.TitleId);

}

Console.WriteLine("Heres a list of Persons:");
foreach (var persons in PersonList)
{
    Console.WriteLine(persons.Name);

}
