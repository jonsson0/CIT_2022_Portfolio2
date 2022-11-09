using System.Runtime.InteropServices;
using System.Threading.Channels;
using DataLayer;
using DataLayer.Models;

var ds = new DataService();

/*var list = ds.getTitles();
var PersonList = ds.getPerson();


var Stitle = ds.getTitle("tt0052520");
var TestPerson = ds.getPerson("nm9993711");

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


Console.WriteLine("this is the titleonmainpage ids:");

var titles = ds.getTitles();
foreach (var title in titles)
{
    Console.WriteLine(title.TitleId);
}

Console.WriteLine("");
*/

//ds.createPerson("nm9993711", "Tom", "1991", null);
ds.updatePerson("nm9993710 ", "steen", "1991", "2012");
//ds.deletePerson("nm9993711");
//Console.WriteLine(TestPerson.PersonId);
