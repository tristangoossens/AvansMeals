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
    public class ReservationService : IReservationService
    {
        // Repositories
        private readonly IStudentRepository _studentRepository;
        private readonly IMealboxRepository _mealboxRepository;

        // Services
        private readonly IMealboxService _mealboxService;

        public ReservationService(
            IStudentRepository studentRepository, 
            IMealboxRepository mealboxRepository,
            IMealboxService mealboxService
        )
        {
            _studentRepository = studentRepository;
            _mealboxRepository = mealboxRepository;
            _mealboxService = mealboxService;
        }

        public async Task<Reservation> CreateReservation(int mealboxId, int studentId)
        {
            Student student = await GetRelatedStudent(studentId);
            Mealbox mealbox = await GetRelatedMealbox(mealboxId);

            if(await _mealboxService.IsAgeBound(mealboxId) && !ValidateStudentAgeForReservation(mealbox, student))
                throw new ArgumentException("Je moet 18 jaar of ouder zijn om dit pakket te reserveren");

            if (IsDuplicatePickupDateForReservations(mealbox, student))
                throw new ArgumentException("Je mag maar een pakket per ophaaldatum reserveren");

            if (await _mealboxService.IsReserved(mealboxId))
                throw new ArgumentException("Dit pakket is reeds door een andere gebruiker gereserveerd");


            return new Reservation
            {
                IsPickedUp = false, 
                Student = student,
                Mealbox = mealbox
            };
        }

        private static bool ValidateStudentAgeForReservation(Mealbox mealbox, Student student)
        {
            // Check if the student is 18 years or older (from pickup date)
            return (mealbox.PickupFrom.Year - student.Birthdate.Year) >= 18;
        }

        private static bool IsDuplicatePickupDateForReservations(Mealbox mealbox, Student student)
        {
            // Check if a student has made a reservation on the planned pickup date
            return student.Reservations.Any(r => r.Mealbox.PickupFrom.Date == mealbox.PickupFrom.Date);
        }

        private async Task<Student> GetRelatedStudent(int id)
        {
            return await _studentRepository.GetSingle(id);
        }

        private async Task<Mealbox> GetRelatedMealbox(int id)
        {
            return await _mealboxRepository.GetSingle(id);
        }
    }
}
