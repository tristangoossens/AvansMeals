using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Implementations
{
    public class StudentService : IStudentService
    {
        public Student CreateNewStudent(int studentNr, string email, string name, DateTime birthDate, string phoneNumer)
        {
            if (!IsValidDate(birthDate))
                throw new InvalidOperationException("Je geboortedatum mag niet na vandaag liggen");

            if (!IsSixteenPlus(birthDate))
                throw new InvalidOperationException("Je moet 16 jaar oud zijn om een account te maken");


            return new Student
            {
                StudentNr = studentNr,
                Email = email,
                Name = name,
                Birthdate = birthDate,
                Phonenumber = phoneNumer
            };
        }

        public bool IsValidDate(DateTime birthDate)
        {
            return birthDate < DateTime.Now;
        }

        private bool IsSixteenPlus(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            // Check for leap year
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;

            return age >= 16;
        }
    }
}
