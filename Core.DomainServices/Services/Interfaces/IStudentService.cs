using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Interfaces
{
    public interface IStudentService
    {
        Student CreateNewStudent(
            int studentNr,
            string email,
            string name,
            DateTime birthDate,
            string phoneNumer
        );
    }
}
