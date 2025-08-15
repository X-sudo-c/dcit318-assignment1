using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthSystemDemo
{
   
    public class Repository<T> where T : class
    {
        private readonly List<T> items = new List<T>();

        public void Add(T item) => items.Add(item);

     
        public List<T> GetAll() => new List<T>(items);

       
        public T? GetById(Func<T, bool> predicate) => items.FirstOrDefault(predicate);

        public bool Remove(Func<T, bool> predicate)
        {
            var toRemove = items.Where(predicate).ToList();
            foreach (var x in toRemove) items.Remove(x);
            return toRemove.Count > 0;
        }
    }

    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString() => $"Patient {{ Id={Id}, Name={Name}, Age={Age}, Gender={Gender} }}";
    }

    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString() => $"Prescription {{ Id={Id}, PatientId={PatientId}, Medication={MedicationName}, DateIssued={DateIssued:yyyy-MM-dd} }}";
    }

    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new Repository<Patient>();
        private readonly Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();

        private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "Alice Mensah", 34, "Female"));
            _patientRepo.Add(new Patient(2, "Kwame Owusu", 46, "Male"));
            _patientRepo.Add(new Patient(3, "Ama Boateng", 29, "Female"));

            _prescriptionRepo.Add(new Prescription(101, 1, "Amoxicillin 500mg", DateTime.Today.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(102, 2, "Lisinopril 10mg", DateTime.Today.AddDays(-30)));
            _prescriptionRepo.Add(new Prescription(103, 1, "Ibuprofen 200mg", DateTime.Today.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(104, 3, "Metformin 500mg", DateTime.Today.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(105, 2, "Atorvastatin 20mg", DateTime.Today.AddDays(-15)));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap = _prescriptionRepo
                .GetAll()
                .GroupBy(p => p.PatientId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.TryGetValue(patientId, out var list) ? list : new List<Prescription>();
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("=== All Patients ===");
            foreach (var p in _patientRepo.GetAll())
            {
                Console.WriteLine(p);
            }
            Console.WriteLine();
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            Console.WriteLine($"=== Prescriptions for PatientId={id} ===");

            var patient = _patientRepo.GetById(p => p.Id == id);
            if (patient == null)
            {
                Console.WriteLine("Patient not found.\n");
                return;
            }

            var prescriptions = GetPrescriptionsByPatientId(id);
            if (prescriptions.Count == 0)
            {
                Console.WriteLine("No prescriptions found for this patient.\n");
                return;
            }

            foreach (var rx in prescriptions.OrderByDescending(r => r.DateIssued))
            {
                Console.WriteLine(rx);
            }
            Console.WriteLine();
        }
    }

    public class Program
    {
        public static void Main()
        {
            var app = new HealthSystemApp(); 
            app.SeedData();                   
            app.BuildPrescriptionMap();       

            app.PrintPrescriptionsForPatient(2);
        }
    }
}
