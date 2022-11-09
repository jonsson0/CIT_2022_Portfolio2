using DataLayer;
using DataLayer.Models;

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

var something = ds.getTitlesByGenre("Drama").GetRange(0, 3);

Console.WriteLine("titles with genre drama");
foreach (var titleGenre in something)
{
    Console.WriteLine(titleGenre.Title.PrimaryTitle);
}

var similar_titles = ds.getSimilarTitles("tt0052520");
Console.WriteLine("here are similar titles:");
Console.WriteLine(similar_titles.Count);

foreach (var similarTitle in similar_titles)
{
    Console.WriteLine(similarTitle.TitleId);
}

Console.WriteLine("test create user");
var createuser = ds.createUser("testing", "1234");
Console.WriteLine(ds.getUsers());