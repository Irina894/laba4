using System;
using System.Collections.Generic;

public abstract class Person
{
    public string Name { get; set; }
    public Person(string name)
    {
        Name = name;
    }
    public abstract string DisplayInfo();
}

public class Patient : Person
{
    public Patient(string name) : base(name) { }
    public override string DisplayInfo()
    {
       return "Patient:"+ Name ;
    }
}

public class Doctor : Person
{
    public Doctor(string name) : base(name)
    {
    }

    public List<Patient> Patients { get; set; } = new List<Patient>();

    public void AddPatient(Patient patient)
    { Patients.Add(patient); }


    public override string DisplayInfo()
    {
        List<Patient> sortedPatients = new List<Patient>(Patients);

        sortedPatients.Sort((p1, p2) => string.Compare(p1.Name, p2.Name, StringComparison.Ordinal));

        List<string> names = new List<string>();

        foreach (Patient p in sortedPatients)
        {
            names.Add(p.Name);
        }

        string patientsList = string.Join(", ", names);

        return "Doctor:" + Name + ", Patients:" + patientsList;
    }

}

public abstract class HospitalUnit
{
    public string Name { get; set; }
    public List<List<Patient>> Rooms { get; set; } = new List<List<Patient>>();
    public HospitalUnit (string name)
    {
        Name=name;
        Rooms=new List<List<Patient>>();
        for (int i = 0; i < 20; i++)
        {
            Rooms.Add(new List<Patient>());
        }
    }

    public  bool AddPatientToRoom(Patient patient)
    {
        foreach (var room in Rooms)
        {
            if (room.Count < 3)
            {
                room.Add(patient);
                return true;
            }
        }
        return false;
    }

    public abstract void DisplayInfo();
}

public class Department : HospitalUnit
{
    public Department(string name) : base(name) { }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Patients in {Name}:");
        foreach (var room in Rooms)
        {
            foreach (var patient in room)
            {
                Console.WriteLine(patient.Name);
            }
        }
    }
    public void DisplayRoomPatients(int roomNumber)
    {
        if (roomNumber >= 0 && roomNumber < Rooms.Count)
        {
            var room = Rooms[roomNumber];
            if (room.Count > 0)
            {
                List<Patient> sortedPatients = new List<Patient>(room);

                sortedPatients.Sort((p1, p2) => string.Compare(p1.Name, p2.Name, StringComparison.Ordinal));

                Console.WriteLine($"Patients in {Name} room {roomNumber}:");
                foreach (var patient in sortedPatients)
                {
                    Console.WriteLine(patient.Name);
                }
            }
            else
            {
                Console.WriteLine($"Room {roomNumber} in {Name} is empty.");
            }
        }
        else
        {
            Console.WriteLine($"Invalid room number for {Name}.");
        }
    }
}

public class Program
{
    static void Main()
    {
        var departments = new List<Department>();
        var doctors = new List<Doctor>();

        Console.WriteLine("Enter hospital data in format:");
        Console.WriteLine("Department, Doctor first name, Doctor last name, Patient");
        Console.WriteLine("To stop adding patients, type 'Output' as department.\n");

        while (true)
        {
            Console.Write("Department: ");
            string departmentName = Console.ReadLine();

            if (departmentName == "Output")
                break;

            Console.Write("Doctor first name: ");
            string doctorFirstName = Console.ReadLine();

            Console.Write("Doctor last name: ");
            string doctorLastName = Console.ReadLine();

            Console.Write("Patient: ");
            string patientName = Console.ReadLine();

            string doctorFullName = doctorFirstName + " " + doctorLastName;

            Department department = null;
            foreach (var dep in departments)
            {
                if (dep.Name == departmentName)
                {
                    department = dep;
                    break;
                }
            }
            if (department == null)
            {
                department = new Department(departmentName);
                departments.Add(department);
            }

            // пошук лікаря
            Doctor doctor = null;
            foreach (var d in doctors)
            {
                if (d.Name == doctorFullName)
                {
                    doctor = d;
                    break;
                }
            }
            if (doctor == null)
            {
                doctor = new Doctor(doctorFullName);
                doctors.Add(doctor);
            }

            // створюємо пацієнта і додаємо
            Patient patient = new Patient(patientName);

            department.AddPatientToRoom(patient);
            doctor.AddPatient(patient);
        }

        Console.WriteLine("\nNow enter output commands (e.g., Cardiology, Surgery 5, John Smith). Type 'End' to finish.");
        while (true)
        {
            Console.Write("Command: ");
            string command = Console.ReadLine();

            if (command == "End")
                break;

            string[] parts = command.Split(' ');
            if (parts.Length > 1 && int.TryParse(parts[parts.Length - 1], out int roomNumber))
            {
                string departmentName = string.Join(" ", parts, 0, parts.Length - 1);

                // знайти відділення
                Department department = null;
                foreach (var dep in departments)
                {
                    if (dep.Name == departmentName)
                    {
                        department = dep;
                        break;
                    }
                }

                if (department != null)
                {
                    department.DisplayRoomPatients(roomNumber);
                }
                else
                {
                    Console.WriteLine("Invalid department or command.");
                }
            }
            else
            {
                // перевірка: чи це відділення?
                Department department = null;
                foreach (var dep in departments)
                {
                    if (dep.Name == command)
                    {
                        department = dep;
                        break;
                    }
                }
                if (department != null)
                {
                    department.DisplayInfo();
                    continue;
                }

                // перевірка: чи це лікар?
                Doctor doctor = null;
                foreach (var d in doctors)
                {
                    if (d.Name == command)
                    {
                        doctor = d;
                        break;
                    }
                }
                if (doctor != null)
                {
                    Console.WriteLine(doctor.DisplayInfo());
                    continue;
                }

                Console.WriteLine("Invalid command.");
            }
        }
    }
}
