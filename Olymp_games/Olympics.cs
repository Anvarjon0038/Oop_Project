using System;
using System.Collections.Generic;
using System.Linq;

namespace Solve
{
    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }

        public virtual void GetInfo()
        {
            Console.WriteLine($"{FirstName} {LastName}, {Age} лет из: {Country}");
        }
    }

    public class Athlete : Person
    {
        public string Sport { get; set; }
        public string Discipline { get; set; }
        public string Rank { get; set; }
        
        private List<Result> results = new List<Result>();

        public override void GetInfo()
        {
            base.GetInfo();
            Console.WriteLine($"  Спортсмен | Спорт: {Sport}, Дисциплина: {Discipline}, Ранк: {Rank}");
        }

        public void AddResult(Result r)
        {
            results.Add(r);
        }
        
        public Dictionary<string, int> GetMedalCount()
        {
            var count = new Dictionary<string, int>
            {
                { "gold", 0 },
                { "silver", 0 },
                { "bronze", 0 }
            };

            foreach (var r in results)
            {
                if (count.ContainsKey(r.Medal))
                {
                    count[r.Medal]++;
                }
            }

            return count;
        }

        public Result GetBestResult(string discipline)
        {
            return results
                .Where(r => r.Discipline == discipline)
                .OrderBy(r => r.Value)
                .FirstOrDefault();
        }

        public List<Result> GetResults()
        {
            return results;
        }
    }

    public class TrainerCoach : Person
    {
        public string Sport { get; set; }
        public int YearsExperience { get; set; }
        public string TeamName { get; set; }

        public override void GetInfo()
        {
            base.GetInfo();
            Console.WriteLine($"  Тренер | Спорт: {Sport}, Опыт: {YearsExperience} лет, Команда: {TeamName}");
        }
    }

    public class Judge : Person
    {
        public string Sport { get; set; }
        public string Category { get; set; } 
        public int TotalJudged { get; set; }

        public override void GetInfo()
        {
            base.GetInfo();
            Console.WriteLine($"  Судья | Спорт: {Sport}, Категория: {Category} Судил соревнований: {TotalJudged}");
        }
    }

    public class Volunteer : Person
    {
        public string AssignedVenue { get; set; }
        public string Role { get; set; } 

        public override void GetInfo()
        {
            base.GetInfo();
            Console.WriteLine($"  Волонтёр | Площадка: {AssignedVenue}, Роль: {Role}");
        }
    }
    
    public struct Result
    {
        public string Sport;
        public string Discipline;
        public double Value;
        public string Unit;
        public string Medal;
        public DateTime Date;
    }

    public class NationalTeam
    {
        public string Country { get; set; }
        public string FlagCode { get; set; }
        public TrainerCoach Coach { get; set; }

        private List<Athlete> athletes = new List<Athlete>();

        public void AddAthlete(Athlete a)
        {
            bool duplicate = athletes.Any(x =>
                x.FirstName == a.FirstName && x.LastName == a.LastName);

            if (duplicate)
            {
                throw new ArgumentException(
                    $"Спортсмен {a.FirstName} {a.LastName} уже добавлен в {Country}.");
            }

            athletes.Add(a);
        }

        public List<Athlete> GetAthletesBySport(string sport)
        {
            return athletes.Where(a => a.Sport == sport).ToList();
        }

        public Dictionary<string, int> GetTotalMedals()
        {
            var total = new Dictionary<string, int>
            {
                { "gold", 0 },
                { "silver", 0 },
                { "bronze", 0 }
            };

            foreach (var athlete in athletes)
            {
                var medals = athlete.GetMedalCount();
                total["gold"] += medals["gold"];
                total["silver"] += medals["silver"];
                total["bronze"] += medals["bronze"];
            }

            return total;
        }

        public Athlete GetStarAthlete()
        {
            return athletes
                .OrderByDescending(a => a.GetMedalCount()["gold"])
                .FirstOrDefault();
        }
        public List<Athlete> GetAthletes() => athletes;
    }

    public class OlympicEvent
    {
        public string Name { get; set; }
        public string Sport { get; set; }
        public string Discipline { get; set; }
        public DateTime Date { get; set; }
        public Judge Judge { get; set; }

        private List<Athlete> participants = new List<Athlete>();
        
        private Dictionary<Athlete, double> scores = new Dictionary<Athlete, double>();
        private string resultUnit = "";

        public void RegisterAthlete(Athlete a)
        {
            if (!participants.Contains(a))
            {
                participants.Add(a);
            }
        }

        public void RecordResult(Athlete a, double value, string unit)
        {
            if (!participants.Contains(a))
            {
                throw new InvalidOperationException(
                    $"{a.FirstName} {a.LastName} не зарегистрирован на это соревнование.");
            }

            scores[a] = value;
            resultUnit = unit;

            a.AddResult(new Result
            {
                Sport = Sport,
                Discipline = Discipline,
                Value = value,
                Unit = unit,
                Medal = "null",
                Date = Date
            });
        }

        public void AwardMedals()
        {
            var sorted = scores.OrderBy(kv => kv.Value).ToList();

            string[] medals = { "gold", "silver", "bronze" };

            for (int i = 0; i < sorted.Count && i < 3; i++)
            {
                var athlete = sorted[i].Key;
                var value = sorted[i].Value;

                athlete.AddResult(new Result
                {
                    Sport = Sport,
                    Discipline = Discipline,
                    Value = value,
                    Unit = resultUnit,
                    Medal = medals[i],
                    Date = Date
                });
            }
        }

        public List<Athlete> GetLeaderboard()
        {
            return scores
                .OrderBy(kv => kv.Value)
                .Select(kv => kv.Key)
                .ToList();
        }
        public List<Athlete> GetParticipants() => participants;
        public Dictionary<Athlete, double> GetScores() => scores;
    }

    public class MedalTable
    {
        private List<NationalTeam> teams = new List<NationalTeam>();

        public void AddTeam(NationalTeam t)
        {
            teams.Add(t);
        }

        public List<NationalTeam> GetRanking()
        {
            return teams
                .OrderByDescending(t => t.GetTotalMedals()["gold"])
                .ThenByDescending(t => t.GetTotalMedals()["silver"])
                .ThenByDescending(t => t.GetTotalMedals()["bronze"])
                .ToList();
        }

        public List<NationalTeam> GetTopN(int n)
        {
            return GetRanking().Take(n).ToList();
        }

        public void PrintTable()
        {
            var ranking = GetRanking();

            Console.WriteLine("Место | Страна | Золото | Серебро | Бронза | Всего");
            Console.WriteLine("---------------------------------------------------");

            int place = 1;
            foreach (var team in ranking)
            {
                var medals = team.GetTotalMedals();
                int total = medals["gold"] + medals["silver"] + medals["bronze"];

                Console.WriteLine(
                    $"{place,5} | {team.Country,-15} | {medals["gold"],6} | {medals["silver"],7} | {medals["bronze"],6} | {total,5}");

                place++;
            }
        }
    }
    
    public class Venue
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }
        public string Sport { get; set; }
    }
    public class VenueConflictException : Exception
    {
        public VenueConflictException(string msg) : base(msg) { }
    }

    public class Schedule
    {
        private List<(OlympicEvent Event, Venue Venue)> entries = new();

        public void AddEvent(OlympicEvent e, Venue v)
        {
            foreach (var entry in entries)
            {
                if (entry.Venue.Name == v.Name)
                {
                    double hoursDiff = Math.Abs((entry.Event.Date - e.Date).TotalHours);
                    if (hoursDiff < 2)
                    {
                        throw new VenueConflictException(
                            $"Площадка \"{v.Name}\" уже занята соревнованием \"{entry.Event.Name}\" " +
                            $"в это время ({entry.Event.Date}).");
                    }
                }
            }

            entries.Add((e, v));
        }

        public List<OlympicEvent> GetEventsByDate(DateTime date)
        {
            return entries
                .Where(entry => entry.Event.Date.Date == date.Date)
                .Select(entry => entry.Event)
                .ToList();
        }

        public List<OlympicEvent> GetEventsByVenue(string venueName)
        {
            return entries
                .Where(entry => entry.Venue.Name == venueName)
                .Select(entry => entry.Event)
                .ToList();
        }
    }
    
    public class OlympicsStats
    {
        private List<OlympicEvent> events;

        public OlympicsStats(List<OlympicEvent> allEvents)
        {
            events = allEvents;
        }
        public double GetOlympicRecord(string discipline)
        {
            var values = events
                .Where(e => e.Discipline == discipline)
                .SelectMany(e => e.GetScores().Values);

            if (!values.Any())
            {
                return 0; 
            }

            return values.Min();
        }

        public Athlete GetMostMedals(string medal)
        {
            var allAthletes = events
                .SelectMany(e => e.GetParticipants())
                .Distinct();

            return allAthletes
                .OrderByDescending(a => a.GetMedalCount()[medal])
                .FirstOrDefault();
        }

        public Dictionary<string, int> GetCountryStats(string country)
        {
            var stats = new Dictionary<string, int>
            {
                { "gold", 0 },
                { "silver", 0 },
                { "bronze", 0 }
            };

            var athletesFromCountry = events
                .SelectMany(e => e.GetParticipants())
                .Where(a => a.Country == country)
                .Distinct();

            foreach (var athlete in athletesFromCountry)
            {
                var medals = athlete.GetMedalCount();
                stats["gold"] += medals["gold"];
                stats["silver"] += medals["silver"];
                stats["bronze"] += medals["bronze"];
            }

            return stats;
        }

        public Athlete GetMostActiveAthlete()
        {
            var participationCount = new Dictionary<Athlete, int>();

            foreach (var ev in events)
            {
                foreach (var athlete in ev.GetParticipants())
                {
                    if (!participationCount.ContainsKey(athlete))
                    {
                        participationCount[athlete] = 0;
                    }
                    participationCount[athlete]++;
                }
            }

            if (!participationCount.Any())
            {
                return null;
            }

            return participationCount
                .OrderByDescending(kv => kv.Value)
                .First()
                .Key;
        }
    }
    
    public static class Ceremony
    {
        public static void PrintOlympicsReport(List<Person> participants)
        {
            Console.WriteLine("=== Церемония открытия: список участников ===\n");

            foreach (var p in participants)
            {
                p.GetInfo();
            }

            Console.WriteLine("\n=== Статистика по ролям ===");
            Console.WriteLine($"Спортсменов: {participants.OfType<Athlete>().Count()}");
            Console.WriteLine($"Тренеров: {participants.OfType<TrainerCoach>().Count()}");
            Console.WriteLine($"Судей: {participants.OfType<Judge>().Count()}");
            Console.WriteLine($"Волонтёров: {participants.OfType<Volunteer>().Count()}");

            if (participants.Any())
            {
                var topCountryGroup = participants
                    .GroupBy(p => p.Country)
                    .OrderByDescending(g => g.Count())
                    .First();

                Console.WriteLine(
                    $"\nСтрана с наибольшим числом участников: {topCountryGroup.Key} ({topCountryGroup.Count()} чел.)");
            }
        }
    }
    
    

    public class Program
    {
        public static void Main()
        {   
            var athlete1 = new Athlete
            {
                FirstName = "Эрлинг",
                LastName = "Холанд",
                Age = 25,
                Country = "Норвегия",
                Sport = "Футбол",
                Discipline = "Нападающий",
                Rank = "World"
            };

            var athlete2 = new Athlete
            {
                FirstName = "Кевин",
                LastName = "ДеБрёйне",
                Age = 27,
                Country = "Бельгия",
                Sport = "Футбол",
                Discipline = "Полузащитник",
                Rank = "Olympic"
            };

            var athlete3 = new Athlete
            {
                FirstName = "Джуд",
                LastName = "Беллингем",
                Age = 22,
                Country = "Англия",
                Sport = "Футбол",
                Discipline = "Полузащитник",
                Rank = "National"
            };

            var coach = new TrainerCoach
            {
                FirstName = "Пеп",
                LastName = "Гвардиола",
                Age = 50,
                Country = "Испания",
                Sport = "Футбол",
                YearsExperience = 15,
                TeamName = "Сборная Испания"
            };

            var teamNorway = new NationalTeam
            {
                Country = "Норвегия",
                FlagCode = "NOR",
                Coach = coach
            };
            teamNorway.AddAthlete(athlete1);

            var teamBelgium = new NationalTeam { Country = "Бельгия", FlagCode = "BEL" };
            teamBelgium.AddAthlete(athlete2);

            var teamEngland = new NationalTeam { Country = "Англия", FlagCode = "ENG" };
            teamEngland.AddAthlete(athlete3);

            var venue = new Venue
            {
                Name = "Олимпийский стадион",
                City = "Париж",
                Capacity = 60000,
                Sport = "Футбол"
            };

            var judge = new Judge
            {
                FirstName = "Максим",
                LastName = "Иван",
                Age = 38,
                Country = "Франция",
                Sport = "Футбол",
                Category = "international",
                TotalJudged = 120
            };

            var footballEvent = new OlympicEvent
            {
                Name = "Финал по футболу",
                Sport = "Футбол",
                Discipline = "Финальный матч",
                Date = new DateTime(2026, 7, 20, 18, 0, 0),
                Judge = judge
            };

            footballEvent.RegisterAthlete(athlete1);
            footballEvent.RegisterAthlete(athlete2);
            footballEvent.RegisterAthlete(athlete3);

            footballEvent.RecordResult(athlete1, 3, "голов");
            footballEvent.RecordResult(athlete2, 2, "голов");
            footballEvent.RecordResult(athlete3, 1, "гол");

            footballEvent.AwardMedals();

            Console.WriteLine("--- Лидерборд матча ---");
            foreach (var a in footballEvent.GetLeaderboard())
            {
                Console.WriteLine($"{a.FirstName} {a.LastName}");
            }
            Console.WriteLine();

            var schedule = new Schedule();
            schedule.AddEvent(footballEvent, venue);

            var medalTable = new MedalTable();
            medalTable.AddTeam(teamNorway);
            medalTable.AddTeam(teamBelgium);
            medalTable.AddTeam(teamEngland);
            medalTable.PrintTable();
            Console.WriteLine();

            var stats = new OlympicsStats(new List<OlympicEvent> { footballEvent });
            Console.WriteLine($"Олимпийский рекорд в дисциплине 'Финальный матч': " + 
                              $"{stats.GetOlympicRecord("Финальный матч")} голов");

            var mostGold = stats.GetMostMedals("gold");
            Console.WriteLine($"Больше всего золота: {mostGold?.FirstName} {mostGold?.LastName}");
            Console.WriteLine();

            var volunteer = new Volunteer
            {
                FirstName = "Алекс",
                LastName = "Максим",
                Age = 30,
                Country = "Франция",
                AssignedVenue = "Волонтер",
                Role = "Волонтер"
            };

            var allParticipants = new List<Person>
            {
                athlete1, athlete2, athlete3, coach, judge, volunteer
            };

            Ceremony.PrintOlympicsReport(allParticipants);
        }
    }
}
