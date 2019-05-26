using System;
using System.Collections.Generic;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Abstraction
{
    public interface IPerson
    {
        string FullName { get; set; }
        DateTime DateOfBirth { get; set; }
        string ImageUrl { get; set; }

        string Biography { get; set; }

        long? GenderId { get; set; }
        Gender Gender { get; set; }
        
        Country Country { get; set; }
    }
}
