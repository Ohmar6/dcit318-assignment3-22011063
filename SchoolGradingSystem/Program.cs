using System;
using System.Collections.Generic;
using System.IO;

namespace SchoolGradingSystem
{
    // a) Student class
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            return Score switch
            {
                >= 80 and <= 100 => "A",
                >= 70 and <= 79 => "B",
                >= 60 and <= 69 => "C",
                >= 50 and <= 59 => "D",
                < 50 => "F",
                _ => "Invalid"
            };
        }

        public override string ToString() =>
            $"{FullName} (ID: {Id}): Score = {Score}, Grade = {GetGrade()}";
    }

    // b) Custom exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // d) StudentResultProcessor
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using var reader = new StreamReader(inputFilePath);
            string? line;
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                var parts = line.Split(',');

                if (parts.Length != 3)
                    throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields, found {parts.Length}.");

                if (!int.TryParse(parts[0].Trim(), out int id))
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid ID format.");

                string fullName = parts[1].Trim();

                if (!int.TryParse(parts[2].Trim(), out int score))
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Score is not a valid integer.");

                students.Add(new Student(id, fullName, score));
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using var writer = new StreamWriter(outputFilePath);

            foreach (var student in students)
            {
                writer.WriteLine(student.ToString());
            }
        }
    }

    // e) Main application flow
    public static class Program
    {
        public static void Main()
        {
            var processor = new StudentResultProcessor();
            string inputPath = "students.txt";
            string outputPath = "report.txt";

            try
            {
                var students = processor.ReadStudentsFromFile(inputPath);
                processor.WriteReportToFile(students, outputPath);
                Console.WriteLine("✅ Report generated successfully.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"❌ FileNotFoundException: {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"❌ InvalidScoreFormatException: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"❌ MissingFieldException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Unexpected error: {ex.Message}");
            }
        }
    }
}