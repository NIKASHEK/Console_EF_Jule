using Jule24.Data;
using Jule24.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Jule24
{
    internal class Program
    {
        private static PersonContext _context = new PersonContext{};
        static void Main(string[] args)
        {
            //GetPersons("Before add");
            //AddPerson("Nika", "598994480");  
            //AddVariousType();
            //GetPersons("After Add");
            //AddPersonAndQuotesAndVehicleWithRelation();
            //QueryFilter();
            //RetrieveAndUpdate();
            //RetrieveAndDelete("random");
            //InsertPersonWithRelatedData();
            //AddVehicleToExistingPerson();
            //AddExistingQuoteToExistingPerson();
            //GetDataAboutPersonWithEverything();
            //FilterDataByAdditionalInfo();
            //QueryUsingSQLInterpolated(1);
            //GetOnlyAuthorNameFromPerson();
            SomeRandomMethods();
        }
        private static void GetPersons(string text)
        {
            Console.WriteLine(text + ": ");
            var people = _context.People;
            foreach (var item in people)
            {
                Console.WriteLine(item.Id + " " + item.Name + " " + item.Phone);
            }
        }
        private static void AddPerson(string name, string phone)
        {
            var person = new Person { Name = name, Phone = phone};
            _context.People.Add(person);
            _context.SaveChanges();
        }
        private static void AddVariousType()
        {
            var person = new Person { Name = "Mate", Phone = "557111111" };
            var vehicle = new Vehicle { Model = "Lexus" };
            var Quotes = new List<Quote> { new Quote { Text = "Keep moving forward!" }, new Quote { Text = "Stay Focused!" } };
            _context.AddRange(person,vehicle);
            _context.Quotes.AddRange(Quotes);
            _context.SaveChanges();
        }
        private static void AddPersonAndQuotesAndVehicleWithRelation()
        {
            var person = new Person { Name = "Tamta", Phone = "557434343" };
            var vehicle = new Vehicle { Model = "Mazda" };
            var quote1 = new Quote { Text = "It's my life" };
            var quote2 = new Quote { Text = "All my own!" };
            person.Vehicle = vehicle;
            person.Quotes.Add(quote1);
            person.Quotes.Add(quote2);
            _context.People.Add(person);
            _context.SaveChanges();
        }
        private static void QueryFilter()
        {
            var people = _context.People.Where(p => EF.Functions.Like(p.Name, "Ni%")).ToList();
            var quote = _context.Quotes.Where(p => p.Text.Contains("Hello")).ToList();
            foreach (var item in people)
            {
                Console.WriteLine(item.Id + " " + item.Name + " " + item.Phone);
            }
            foreach (var item in quote)
            {
                Console.WriteLine(item.Id + " " + item.Text + " "  + item.PersonId);
            }
        }
        private static void RetrieveAndUpdate()
        {
            var quote = _context.Quotes.Skip(1).FirstOrDefault();
            quote.Text += "<<*>>";
            Console.WriteLine(quote.Id + " " + quote.Text + " " + quote.PersonId);

        }
        private static void RetrieveAndDelete(string name)
        {
            var person = _context.People.Where(p => p.Name == name).FirstOrDefault();
            if (person != null)
            {
                _context.People.Remove(person);
                _context.SaveChanges();
            }
        }
        private static void InsertPersonWithRelatedData()
        {
            var apartments = new List<Apartment> { new Apartment {Name = "Red Castle"}, new Apartment {Name = "King Castle"} };
            var person = new Person
            {
                Name = "Shota",
                Phone = "555554544",
                Vehicle = new Vehicle { Model = "Subaru" },
                Quotes = new List<Quote> { new Quote { Text = "Stay strong" }, new Quote {Text = "Try again and again"} },
                Apartments = apartments
            };
            _context.Add(person);
            _context.SaveChanges();
        }
        private static void AddVehicleToExistingPerson()
        {
            var person = _context.People.Where(p => p.Name.ToLower() == "tamta").Include(p => p.Vehicle).FirstOrDefault();
            var mercedes = new Vehicle { Model = "Mercedes" };
            if (person.Vehicle != null)
            {
                person.Vehicle = null;
               // _context.Vehicles.Remove(person.Vehicle);
               //_context.SaveChanges();
            }
            person.Vehicle = mercedes;
            _context.SaveChanges();
        }
        private static void AddExistingQuoteToExistingPerson()
        {
            var existingQuote = _context.Quotes.FirstOrDefault();
            var existingPerson = _context.People.FirstOrDefault();
            existingPerson.Quotes.Add(existingQuote);
            _context.SaveChanges();
        }
        private static void GetDataAboutPersonWithEverything()
        {
            var persons = _context.People
                .Include(p => p.Apartments)
                .Include(p => p.Quotes)
                    .ThenInclude(q => q.Author)
                .Include(p => p.Vehicle)
                .ToList();
            foreach (var person in persons)
            {
                Console.WriteLine(person.Id + " " + person.Name + " " + person.Phone);
                Console.WriteLine("Apartments: ");
                foreach (var apartment in person.Apartments)
                {
                    Console.WriteLine(apartment.Id + " " + apartment.Name);
                }

                Console.WriteLine("Quotes: ");
                foreach (var quote in person.Quotes)
                {
                    Console.WriteLine(quote.Id + " " + quote.Text + " " + quote.PersonId );

                    if (quote.Author != null)//
                    {
                        Console.WriteLine("Author: " + quote.Author.Name);
                    }
                }

                if (person.Vehicle != null)//
                {
                    Console.WriteLine("Vehicle: " + person.Vehicle.Model);
                }
                Console.WriteLine(new string('-', 50));
            }
        }
        private static void FilterDataByAdditionalInfo()
        {
            var personWithSpecificApartment = _context.People.Where(p => p.Apartments.Any(q => EF.Functions.Like(q.Name, "Red%"))).FirstOrDefault();

            if (personWithSpecificApartment != null)
            {
                Console.WriteLine(personWithSpecificApartment.Id + " " + personWithSpecificApartment.Name);
            }

            Console.WriteLine(new string('-', 50));

            var personsWithSpecificQuote = _context.People.Where(p => p.Quotes.Any(q => q.Text.Contains("Hello") || q.Author != null && q.Author.Name == "John")).ToList();
            foreach (var person in personsWithSpecificQuote)
            {
                Console.WriteLine(person.Id + " " + person.Name + " " + person.Phone);
            }

            Console.WriteLine(new string('-', 50));

            var personsWithSpecificVehicle = _context.People.Where(p => /*p.Vehicle != null &&*/ EF.Functions.Like(p.Vehicle.Model, "M%")).ToList();
            foreach (var person in personsWithSpecificVehicle)
            {
                Console.WriteLine(person.Name);
            }
        }
        private static void QueryUsingSQLInterpolated(int id)
        {
            var authors = _context.Authors.FromSqlInterpolated($"Select * from Authors where Id in ({id})").ToList();
            foreach (var author in authors)
            {
                Console.WriteLine(author.Id + " " + author.Name);
            }
        }
        private static void GetOnlyAuthorNameFromPerson()
        {
            var authorName = _context.People
                .Where(p => p.Name == "Shota")
                .SelectMany(p => p.Quotes)
                .Select(q => q.Author.Name).FirstOrDefault();

            Console.WriteLine(authorName);
        }
        private static void SomeRandomMethods()
        {
            //1.
            //var person = new Person { Id = Guid.Parse("4F0B7FC5-7C02-482D-E861-08DEEB0C12EE")};
            //_context.People.Attach(person);
            //person.Name = "Nika";
            //_context.SaveChanges();


            ////2.
            ////--------------------------------------------------------------------------------

            //var zaza = new Person { Name = "Zaza", Phone = "5474747474" };
            //_context.Entry<Person>(zaza).State = EntityState.Added;
            //_context.SaveChanges();


            //3.
            //----------------------------------------------------------------------------------
            //var zaza = new Person { Id = Guid.Parse("844EB046-C6F1-4B74-0E23-08DEEBFB8B0D") };
            //_context.Entry<Person>(zaza).State = EntityState.Deleted;
            //_context.SaveChanges();


            //4.
            //----------------------------------------------------------------------------------
            //var quote = new Quote { Id = 2, Text = "Hello World!!!!!!!!!!", PersonId = Guid.Parse("6D868589-1697-4AA7-0818-08DEEB0D7CEB"), AuthorId = null};
            //_context.Entry<Quote>(quote).State = EntityState.Modified;
            //_context.SaveChanges();


            //5.
            //----------------------------------------------------------------------------------
            //var quote = new Quote { Id = 5, AuthorId = 1};
            //_context.Entry<Quote>(quote).Property(q => q.AuthorId).IsModified = true;
            //_context.SaveChanges();


            //6.
            //----------------------------------------------------------------------------------
            //var newPersons = _context.People.Select(p => new List<object> 
            //{
            //    p.Name, p.Phone
            //}).ToList();
            //foreach (var person in newPersons)
            //{
            //    Console.WriteLine(person[0] + " " + person[1]);
            //}


            //7.
            //-----------------------------------------------------------------------------------
            //make some inner Join
            //
            //var persons = _context.People.Where(p => p.Quotes.Any(q => q.Author.Id != null))
            //    .Include(p => p.Quotes)
            //        .ThenInclude(q => q.Author)
            //        .ToList();
            //foreach (var person in persons)
            //{
            //    Console.WriteLine(person.Name + ": ");
            //    foreach (var quote in person.Quotes)
            //    {
            //        if(quote.Author != null)
            //            Console.WriteLine(quote.Author.Name);
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine(new string('-',50));
            //}
        }
    }
}
