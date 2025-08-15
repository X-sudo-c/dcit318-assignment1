using System;
using System.Collections.Generic;
using System.IO;

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
        if (Score >= 80 && Score <= 100) return "A";
        if (Score >= 70 && Score <= 79) return "B";
        if (Score >= 60 && Score <= 69) return "C";
        if (Score >= 50 && Score <= 59) return "D";
        return "F";
    }

    public override string ToString()
    {
        return $"{FullName} (ID: {Id}): Score = {Score}, Grade = {GetGrade()}";
    }
}

public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message) { }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message) { }
}

public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        var students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string line;
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                string[] parts = line.Split(',');

                if (parts.Length < 3)
                    throw new MissingFieldException($"Line {lineNumber}: Missing fields.");

                int id;
                if (!int.TryParse(parts[0].Trim(), out id))
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid ID format.");

                string fullName = parts[1].Trim();
                int score;
                if (!int.TryParse(parts[2].Trim(), out score))
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format.");

                students.Add(new Student(id, fullName, score));
            }
        }

        return students;
    }

    public void WriteReportToFile(List<Student> students, string outputFilePath)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (var student in students)
            {
                writer.WriteLine(student.ToString());
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        string inputPath = "students.txt";
        string outputPath = "report.txt";

        var processor = new StudentResultProcessor();

        try
        {
            List<Student> students = processor.ReadStudentsFromFile(inputPath);
            processor.WriteReportToFile(students, outputPath);
            Console.WriteLine("Report generated successfully at: " + outputPath);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("Error: Input file not found. " + ex.Message);
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine("Error: Invalid score format. " + ex.Message);
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine("Error: Missing data field. " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected Error: " + ex.Message);
        }
    }
}
