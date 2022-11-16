﻿using System;
namespace DataLayer.Models
{
    public class Person
    {

        public string PersonId { get; set; }
        public string Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public List<Character> Characters { get; set; }
        public List<PersonProfession> PersonProfessions { get; set; }


    }
}

