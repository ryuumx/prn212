using System;
using Newtonsoft.Json;

namespace NuGetExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person
            {
                Name = "John Doe",
                Age = 30,
                IsStudent = false
            };

            string json = JsonConvert.SerializeObject(person, Formatting.Indented);
            Console.WriteLine(json);

            string jsonInput = @"{""Name"":""Jane Smith"",""Age"":25,""IsStudent"":true}";
            Person deserializedPerson = JsonConvert.DeserializeObject<Person>(jsonInput);
            Console.WriteLine($"Deserialized: {deserializedPerson.Name}, {deserializedPerson.Age}, {deserializedPerson.IsStudent}");
        }
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsStudent { get; set; }
    }
}