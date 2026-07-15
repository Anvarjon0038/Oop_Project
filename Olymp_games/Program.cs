// using System;
// using System.Collections.Generic;
//
// class Solve
// {
//     public abstract class Person
//     {
//         public string FirstName { get; set; }
//         public string LastName { get; set; }
//         public int Age { get; set; }
//         public string Country { get; set; }
//
//         public virtual void getInfo()
//         {
//             Console.WriteLine($"{FirstName} {LastName} is {Age} years old");
//         }
//     }
//     
//     public class Athlete : Person
//     {
//         public string Sport { get; set; }
//         public string Decipline { get; set; }
//         public int Rank { get; set; }
//         private List<Result> result=new List<Result>();
//
//         public void AddResult(Result r)
//         {
//             result.Add(r);
//         }
//
//         public Dictionary<string, int> GetMedalCount()
//         {
//             Dictionary<string, int> medals = new Dictionary<string, int>
//             {
//                 {"Gold",0},
//                 {"Silver",0},
//                 {"Bronze",0}
//             };
//             foreach (var i in result)
//             {
//                 if (medals.ContainsKey(i.Medal))
//                 {
//                     medals[i.Medal]++;
//                 }
//             }
//
//             return medals;
//         }
//
//         public Result GetResult(string decipline)
//         {
//             Result mx = new Result();
//             int ok = 0;
//             foreach (var i in result)
//             {
//                 if(ok==0 || i.Value<mx.Value)
//                 {
//                     mx = i;
//                     ok = 1;
//                 }
//             }
//             return mx;
//         }
//         
//         public override void getInfo()
//         {
//             Console.WriteLine($"{Sport} {Decipline} rank {Rank}");
//         }
//     }
//
//     public class TrainerCoach : Person
//     {
//         public string Sport { get; set; }
//         public int YearsExperience { get; set; }
//         public string TeamName {get; set;}
//
//         public override void getInfo()
//         {
//             Console.Write($"{Sport} and  years experience is {YearsExperience} {TeamName}");
//         }
//     }
//     
//     public class Judge : Person
//     {
//         public string Sport { get; set; }
//         public bool Category {get; set;}
//         public int TotalJudged { get; set; }
//
//         public override void getInfo()
//         {
//             Console.WriteLine($"{Sport} category is {Category} and total judged {TotalJudged}");
//         }
//     }
//
//     public class Volunteer : Person
//     {
//         public string AssignedVenue { get; set; }
//         public string Role { get; set; }
//
//         public override void getInfo()
//         {
//             Console.WriteLine($"AssignedVenue is {AssignedVenue} role is {Role}");
//         }
//     }
//
//     public struct Result
//     {
//         public string Sport;
//         public string Discipline;
//         public double Value;
//         public string Unit;
//         public string Medal;
//         public DateTime Date;
//     }
//
//     public class  NationalTeam
//     {
//         public string Country { get; set; }
//         public string FlagCode { get; set; }
//         public TrainerCoach Coach { get; set; }
//         private List<Athlete> athletes=new List<Athlete>();
//
//         public void AddAthlete(Athlete a)
//         {
//             
//         }
//
//         public List<Athlete> GetAthletesBySport(string sport)
//         {
//             
//         }
//
//         public Dictionary<string, int> GetTotalMedals()
//         {
//             
//         }
//
//         public Athlete GetStarAthlete()
//         {
//             
//         }
//     }
//     
// }