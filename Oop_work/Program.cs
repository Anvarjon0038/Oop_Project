using System;
using System.Collections.Generic;


class Solve
{
    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }

        public virtual void getInfo()
        {
            // TODO
            Console.WriteLine($"Name: {FirstName} {LastName}");
            Console.WriteLine($"Age {Age}");
            Console.WriteLine($"Country {Country}");
        }
    }

    public class Player : Person
    {
        // TODO: Position, Number, ClubTeam
        // TODO: override getInfo()
        public string position { get; set; }
        public int number { get; set; }
        public string Clubteam { get; set; }

        public override void getInfo()
        {
            base.getInfo();
            Console.WriteLine($"Position {position}");
            Console.WriteLine($"Number {number}");
            Console.WriteLine($"Club {Clubteam}");

        }
    }

    public class Coach : Person
    {
        public string NationalTeam { get; set; }
        public int YearsOfExperience { get; set; }

        public override void getInfo()
        {
            base.getInfo();
            Console.WriteLine($"Team {NationalTeam}");
            Console.WriteLine($"Experience {YearsOfExperience}");
        }
    }

    public class Referee : Person
    {
        public string Nationality { get; set; }
        public int MatchesOfficiated { get; set; }

        public override void getInfo()
        {
            base.getInfo();
            Console.WriteLine($"Nationality {Nationality}");
            Console.WriteLine($"Matches {MatchesOfficiated}");
        }
    }


    public struct Match
    {
        public string HomeTeam;
        public string AwayTeam;
        public int HomeGoals;
        public int AwayGoals;
        public string Stage;
        public DateTime Date;
    }

    public class NationalTeam
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public Coach Coach { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        private List<Match> history = new List<Match>();
        private List<Player> squad = new List<Player>();

        public void AddPlayer(Player p)
        {
            // TODO: проверка на 26 игроков
            if (squad.Count > 26) throw new InvalidCastException("It's maximum");
            squad.Add(p);
        }

        public List<Player> GetPlayersByPosition(string position)
        {
            // TODO
            List<Player> res = new List<Player>();
            foreach (var i in squad)
            {
                if (i.position == position) res.Add(i);
            }

            return res;
        }

        public Player GetCaptain(int captainNumber)
        {
            // TODO
            foreach (var i in squad)
            {
                if (i.number == captainNumber) return i;
            }

            return null;
        }

        public string GetResult(Match m)
        {
            bool isHome = m.HomeTeam == this.Name;
            bool isAway = m.AwayTeam == this.Name;

            if (!isHome && !isAway) return "Не участвовали";
            int myGoals = isHome ? m.HomeGoals : m.AwayGoals;
            int Egol = isHome ? m.AwayGoals : m.HomeGoals;

            if (myGoals > Egol) return "Win";
            if (myGoals == Egol) return "Draw";
            return "Loss";
        }

        public void AddMatchResult(Match m)
        {
            history.Add(m);

            bool isHome = m.HomeTeam == this.Name;
            int myGoals = isHome ? m.HomeGoals : m.AwayGoals;
            int enemyGoals = isHome ? m.AwayGoals : m.HomeGoals;

            GoalsScored += myGoals;
            GoalsConceded += enemyGoals;

            string result = GetResult(m);
            if (result == "Win") Points += 3;
            else if (result == "Draw") Points += 1;
        }

    }

    public class Group
    {
        public string Name { get; set; }
        public List<NationalTeam> teams = new List<NationalTeam>();

        public void CalculateStandings()
        {
            Console.WriteLine($"\n--- Турнирная таблица Группы {Name} ---");
            List<NationalTeam> sortedTeams = SortByPoints();

            for (int i = 0; i < sortedTeams.Count; i++)
            {
                NationalTeam t = sortedTeams[i];
                int gd = t.GoalsScored - t.GoalsConceded;
                Console.WriteLine($"{i + 1}. {t.Name} | Очки: {t.Points} | Забито: {t.GoalsScored} | Разница: {gd}");
            }
        }

        public List<NationalTeam> SortByPoints()
        {
            List<NationalTeam> sorted = new List<NationalTeam>(teams);

            for (int i = 0; i < sorted.Count - 1; i++)
            {
                for (int j = 0; j < sorted.Count - 1 - i; j++)
                {
                    bool needSwap = false;
                    NationalTeam t1 = sorted[j];
                    NationalTeam t2 = sorted[j + 1];

                    int gd1 = t1.GoalsScored - t1.GoalsConceded;
                    int gd2 = t2.GoalsScored - t2.GoalsConceded;

                    // Критерий 1: Очки
                    if (t1.Points < t2.Points)
                    {
                        needSwap = true;
                    }
                    else if (t1.Points == t2.Points)
                    {
                        // Критерий 2: Разница голов
                        if (gd1 < gd2)
                        {
                            needSwap = true;
                        }
                        // Критерий 3: Забитые голы
                        else if (gd1 == gd2 && t1.GoalsScored < t2.GoalsScored)
                        {
                            needSwap = true;
                        }
                    }

                    if (needSwap)
                    {
                        NationalTeam temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            return sorted;
        }

        public List<NationalTeam> GetTopTwo()
        {
            List<NationalTeam> sorted = SortByPoints();
            List<NationalTeam> topTwo = new List<NationalTeam>();
            if (sorted.Count >= 1) topTwo.Add(sorted[0]);
            if (sorted.Count >= 2) topTwo.Add(sorted[1]);
            return topTwo;
        }

    }

    public class KnockoutStage
    {
        private Random random = new Random();

        public NationalTeam PlayMatch(NationalTeam a, NationalTeam b)
        {
            int goalsA = random.Next(0, 4);
            int goalsB = random.Next(0, 4);
            Console.Write($"Матч: {a.Name} {goalsA} - {goalsB} {b.Name}. ");
            if (goalsA > goalsB)
            {
                Console.WriteLine($"Победитель: {a.Name}");
                return a;
            }

            if (goalsB > goalsA)
            {
                Console.WriteLine($"Победитель: {b.Name}");
                return b;
            }

            Console.Write("Серия пенальти! ");
            int penA = 0, penB = 0;
            while (penA == penB)
            {
                penA = random.Next(2, 6);
                penB = random.Next(2, 6);
            }

            Console.WriteLine($"({penA}:{penB}). Победитель: {(penA > penB ? a.Name : b.Name)}");
            return penA > penB ? a : b;
        }

        public NationalTeam RunTournament(List<NationalTeam> currentTeams)
        {
            if (currentTeams.Count == 1)
            {
                return currentTeams[0];
            }

            List<NationalTeam> winners = new List<NationalTeam>();

            Console.WriteLine($"\n--- Раунд (Осталось команд: {currentTeams.Count}) ---");

            for (int i = 0; i < currentTeams.Count; i += 2)
            {
                if (i + 1 < currentTeams.Count)
                {
                    NationalTeam winner = PlayMatch(currentTeams[i], currentTeams[i + 1]);
                    winners.Add(winner);
                }
                else
                {
                    winners.Add(currentTeams[i]);
                }
            }

            return RunTournament(winners);
        }
    }
}

class Project
{
    static void Main()
    {
        Solve.Player p = new Solve.Player();
        p.FirstName = "Cristiano";
        p.LastName = "Ronaldo";
        p.Age = 41;
        p.Country = "Portugal";
        p.position = "Vinger";
        p.number = 7;
        p.Clubteam = "Al Nasr";
        
        p.getInfo();
        
        
        
    }
}