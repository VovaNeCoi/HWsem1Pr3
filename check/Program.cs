using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;



public class Pr3
{
    static List<Employee> employees = new List<Employee>();
    public static void Main()
    {
        // Получаем данные из фалов из директории
        var getedFiles = Directory.GetFiles("C:\\ITLearning\\C# Practics 1 sem\\Pr3Check\\", "*.xml");
        // Каждый XML переводим в тип Employee
        foreach (var File in getedFiles)
        {
            XmlSerializer xser = new XmlSerializer(typeof(Employee));
            using (StreamReader reader = new StreamReader(File))
            {
                Employee temp_employee = (Employee)xser.Deserialize(reader);
                employees.Add(temp_employee);
            }
        }

        // ------------------- Задание 1
        // Вставляем фамилию и получаем результат
        Searching("Третья");
        static void Searching(string lastName)
        {
            Console.WriteLine($"     Задание 1:\nФамилия:{lastName}");
            Employee employee = employees.Where(e => e.fio.EndsWith(lastName)).FirstOrDefault();
            if (employee != null)
            {
                var jobs = employee.jobList.OrderBy(j => j.firstDay);
                foreach (var job in jobs)
                {
                    Console.WriteLine($"{job.name} ({job.firstDay} - {job.lastDay})");
                }
            }

            // Считаем макс, мин и ср.знач.
            var salaries = employee.salaryList.Select(s => s.size);
            decimal maxSalary = salaries.Max();
            decimal minSalary = salaries.Min();
            decimal avgSalary = salaries.Average();
            Console.WriteLine($"Максимальная ЗП: {maxSalary}\nМинимальная ЗП: {minSalary}\nСрЗнач ЗП: {avgSalary}\n");
        }

        // ------------------- Задание 2
        // Выводим для каждого отделаколичество работников и названия позиций
        AboutDepartment();
        static void AboutDepartment()
        {
            Console.WriteLine("     Задание 2:");
            var departments = employees.GroupBy(e => e.jobList.Last().department).Select(g => new
               {
                   dep = g.Key,
                   numOfEmpl = g.Count(),
                   pos = g.SelectMany(e => e.jobList.Select(j => j.name)).Distinct().ToList()
               }).ToList();

            foreach (var department in departments)
            {
                Console.WriteLine($"Отдел: {department.dep}\nРабов: {department.numOfEmpl}\nПроцент: {Math.Round((double)department.numOfEmpl / employees.Count(), 2)*100}");
                Console.Write("Позиция: ");
                foreach (var position in department.pos)
                {
                    Console.Write(position + " --> ");
                }
            }
            Console.WriteLine("RIP\n");
        }

        // ------------------- Задание 3
        // Работники работающие в нескольких отделах
        EmplsInDiffDeparts();
        static void EmplsInDiffDeparts()
        {
            Console.WriteLine("     Задание 3:");
            var EmplsInDiffDeparts = employees.Where(e => e.jobList.Select(j => j.department).Distinct().Count() > 1)
               .Select(e => new
               {
                   empl = e.fio,
                   dep = e.jobList.Select(j => j.department).Distinct().ToList(),
                   cash = e.salaryList.Select(j => j.size).Max()
               })
               .ToList();

            foreach (var thisEmpl in EmplsInDiffDeparts)
            {
                Console.WriteLine($"Работник: {thisEmpl.empl}");
                Console.Write("Отделы: ");
                foreach (var department in thisEmpl.dep)
                {
                    Console.Write(department + " --> ");
                }
                Console.WriteLine(thisEmpl.cash);
            }
        }

        // ------------------- Задание 4
        // Отдел с 3 и больше сотрудниками
        LittleDepart();
        static void LittleDepart()
        {
            Console.WriteLine("\n\n     Задание 4:");
            var allDepartments = employees.GroupBy(e => e.jobList.Last().department).Select(g => new
               {
                   dep = g.Key,
                   numOfEmpls = g.Count(),
                   pos = g.SelectMany(e => e.jobList.Select(j => j.name)).Distinct().ToList()
               }).ToList();

            var littleDep = allDepartments.Where(d => d.numOfEmpls <= 3).ToList();

            foreach (var department in littleDep)
            {
                Console.WriteLine($"Отдел: {department.dep} \nРабов: {department.numOfEmpls}");
            }
            Console.WriteLine();
        }

        // ------------------- Задание 5
        // Года для 5 задания
        AboutIndustry();
        static void AboutIndustry()
        {
            Console.WriteLine("     Задание 5:");
            var years = employees.SelectMany(e => e.jobList.Select(j => j.firstDay?.Year))
                .Concat(employees.SelectMany(e => e.jobList.Select(j => j.lastDay?.Year)))
                .GroupBy(y => y).OrderByDescending(g => g.Count()).ToList();

            Console.WriteLine($"Год с наибольшим кол-вом уволенных рабов: {years.First().Key}");
            Console.WriteLine($"Год с наибольшим кол-вом принятых: {years.Last().Key}");
            Console.WriteLine($"Год с наименьшим кол-вом уволенных рабов: {years.OrderBy(g => g.Count()).Last().Key}");
            Console.WriteLine($"Год с наименьшим кол-вом принятых рабов: {years.OrderBy(g => g.Count()).First().Key}");
        }
       
    }
    
    
    
    
    
}

public class Employee
{
    public string fio { get; set; }
    public int birthYear { get; set; }
    public string adress { get; set; }
    public string phoneNum { get; set; }
    public List<Job> jobList { get; set; }
    public List<Salary> salaryList { get; set; }
}
public class Job
{
    public string name { get; set; }
    public DateTime? firstDay { get; set; }
    public DateTime? lastDay { get; set; }
    public string department { get; set; }
}
public class Salary
{
    public int year { get; set; }
    public int month { get; set; }
    public decimal size { get; set; }
}

