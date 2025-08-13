using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthSystemDemo
{
    // a) Generic repository
    public class Repository<T>
    {
        private readonly List<T> items = new();

        public void Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            items.Add(item);
        }

        public List<T> GetAll() => new(items);

        public T? GetById(Func<T, bool> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            var match = items.FirstOrDefault(predicate);
            if (match is null) return false;
            return items.Remove(match);
        }
    }

    // b) Patient model
    public class Patient
    {
        public int Id;
        public string Name;
        public int Age;
        public string Gender;

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString() => $"Patient(Id={Id}, Name={Name}, Age={Age}, Gender={Gender})";
    }

    // c) Prescription model
    public class Prescription
    {
        public int Id;
        public int PatientId;
        public string MedicationName;
        public DateTime DateIssued;

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString() => $"Prescription(Id={Id}, PatientId={PatientId}, Medication={MedicationName}, DateIssued={DateIssued:yyyy-MM-dd})";
    }

    // d-f) Prescription map and query helper
    public class PrescriptionIndex
    {
        private readonly Dictionary<int, List<Prescription>> _map = new();

        public void Build(IEnumerable<Prescription> prescriptions)
        {
            _map.Clear();
            foreach (var p in prescriptions)
            {
                if (!_map.TryGetValue(p.PatientId, out var list))
                {
                    list = new List<Prescription>();
                    _map[p.PatientId] = list;
                }
                list.Add(p);
            }
        }

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _map.TryGetValue(patientId, out var list) ? new List<Prescription>(list) : new List<Prescription>();
        }

        // Optional: expose the internal map as read-only if needed
        public IReadOnlyDictionary<int, List<Prescription>> Map => _map;
    }

    // g) HealthSystemApp
    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private readonly PrescriptionIndex _index = new();

        public void SeedData()
        {
            // Add patients
            _patientRepo.Add(new Patient(1, "Ama Mensah", 28, "Female"));
            _patientRepo.Add(new Patient(2, "Kwame Boateng", 35, "Male"));
            _patientRepo.Add(new Patient(3, "Efua Owusu", 42, "Female"));

            // Add prescriptions (ensure PatientId matches existing patients)
            _prescriptionRepo.Add(new Prescription(101, 1, "Amoxicillin 500mg", DateTime.Today.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(102, 1, "Paracetamol 1g", DateTime.Today.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(103, 2, "Ibuprofen 400mg", DateTime.Today.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(104, 3, "Atorvastatin 20mg", DateTime.Today.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(105, 2, "Metformin 500mg", DateTime.Today.AddDays(-1)));
        }

        public void BuildPrescriptionMap()
        {
            _index.Build(_prescriptionRepo.GetAll());
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("All Patients:");
            foreach (var p in _patientRepo.GetAll())
            {
                Console.WriteLine($"- {p}");
            }
            Console.WriteLine();
        }

        public void PrintPrescriptionsForPatient(int patientId)
        {
            var patient = _patientRepo.GetById(p => p.Id == patientId);
            if (patient is null)
            {
                Console.WriteLine($"No patient found with Id={patientId}");
                return;
            }

            Console.WriteLine($"Prescriptions for {patient.Name} (Id={patient.Id}):");
            var list = _index.GetPrescriptionsByPatientId(patientId);
            if (list.Count == 0)
            {
                Console.WriteLine("  (none)");
                return;
            }

            foreach (var rx in list.OrderBy(r => r.DateIssued))
            {
                Console.WriteLine($"- {rx}");
            }
            Console.WriteLine();
        }
    }

    // Main application flow
    public static class Program
    {
        public static void Main()
        {
            var app = new HealthSystemApp();

            app.SeedData();
            app.BuildPrescriptionMap();
            app.PrintAllPatients();

            // Choose a patient id to demonstrate
            var selectedPatientId = 2;
            app.PrintPrescriptionsForPatient(selectedPatientId);
        }
    }
}