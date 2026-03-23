using System;
using System.Collections.Generic;
using System.Linq;



var people = new List<Person>
{
    new("Mia",     24, "Berlin",     true,  new List<string>{ "skiing", "gaming", "coffee" }),
    new("Lucas",   31, "Lisbon",     false, new List<string>{ "surfing", "guitar" }),
    new("Aisha",   19, "Toronto",    true,  new List<string>{ "photography", "hiking", "gaming" }),
    new("noah",    42, "Austin",     true,  new List<string>{ "bbq", "motorcycles", "coffee", "gaming" }),
    new("Sofia",   28, "Barcelona",  false, new List<string>{ "dancing", "yoga" }),
    new("Kai",     22, "Seoul",      true,  new List<string>{ "gaming", "street food", "photography", "sleeping" })
};

Console.WriteLine("Data ready. Start playing with LINQ!\n");

var max = people.Max(x => x.Hobbies.Count);
Console.WriteLine($"max = {max}");

var result = people.Where(x => x.Hobbies.Count == max);

foreach (var person in result)
{
  Console.WriteLine(person.Name);
}

class Person
{
  public string Name { get; set; }
  public int Age { get; set; }
  public string City { get; set; }
  public bool LikesCoffee { get; set; }
  public List<string> Hobbies { get; set; } = new List<string>();

  public Person(string name, int age, string city, bool likesCoffee, List<string> hobbies)
  {
    Name = name;
    Age = age;
    City = city;
    LikesCoffee = likesCoffee;
    Hobbies = hobbies;
  }
}

