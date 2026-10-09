using System;
using System.Collections.Generic;
using System.IO;

namespace CatAndMouseGame
{
    public class Game
    {
        public static string InputFile { get; set; }
        public static string OutFile { get; set; }

        public int size;
        public Player cat;
        public Player mouse;
        public GameState state;

        private List<string> logLines = new List<string>();
        private int caughtCell = -1;

        public Game(int size)
        {
            this.size = size;
            cat = new Player("Cat", size);
            mouse = new Player("Mouse", size);
            state = GameState.Start;
        }

        public void Run()
        {
            if (!File.Exists(InputFile))
            {
                Console.WriteLine($"Ошибка: Входной файл {InputFile} не найден.");
                return;
            }

            string[] allLines = File.ReadAllLines(InputFile);
            if (allLines.Length == 0) return;

            if (int.TryParse(allLines[0].Trim(), out int fileSize))
            {
                this.size = fileSize;
                cat = new Player("Cat", fileSize);
                mouse = new Player("Mouse", fileSize);
            }

            for (int i = 1; i < allLines.Length; i++)
            {
                string line = allLines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                char commandType = parts[0][0];

                if (commandType == 'P')
                {
                    DoPrintCommand();
                }
                else if (parts.Length > 1 && int.TryParse(parts[1], out int steps))
                {
                    DoMoveCommand(commandType, steps);

                    if (cat.state == State.Playing && mouse.state == State.Playing && cat.location == mouse.location)
                    {
                        caughtCell = cat.location;
                        cat.state = State.Winner;
                        mouse.state = State.Loser;
                        state = GameState.End;
                        break;
                    }
                }
            }

            if (state != GameState.End)
            {
                mouse.state = State.Winner;
                cat.state = State.Loser;
                state = GameState.End;
            }

            WriteLogToFile();
        }

        private void DoMoveCommand(char command, int steps)
        {
            switch (command)
            {
                case 'M': mouse.Move(steps); break;
                case 'C': cat.Move(steps); break;
            }
        }

        private void DoPrintCommand()
        {
            string catPos = cat.state == State.NotInGame ? "??" : cat.location.ToString();
            string mousePos = mouse.state == State.NotInGame ? "??" : mouse.location.ToString();
            string distanceStr = "";

            if (cat.state == State.Playing && mouse.state == State.Playing)
            {
                distanceStr = GetDistance().ToString();
            }

            logLines.Add($"{catPos,3}{mousePos,7}{distanceStr,10}");
        }

        private int GetDistance()
        {
            if (cat.location == -1 || mouse.location == -1) return 0;
            int directDist = Math.Abs(cat.location - mouse.location);
            int circularDist = size - directDist;
            return Math.Min(directDist, circularDist);
        }

        private void WriteLogToFile()
        {
            using (StreamWriter writer = new StreamWriter(OutFile, false))
            {
                writer.WriteLine("Cat and Mouse");
                writer.WriteLine();
                writer.WriteLine("Cat Mouse  Distance");
                writer.WriteLine("-------------------");

                foreach (var line in logLines)
                {
                    writer.WriteLine(line);
                }

                writer.WriteLine("-------------------");
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine($"Distance traveled:   Mouse    Cat");
                writer.WriteLine($"                        {mouse.distanceTraveled}     {cat.distanceTraveled}");
                writer.WriteLine();

                if (caughtCell != -1)
                {
                    writer.WriteLine($"Mouse caught at:  {caughtCell}");
                }
                else
                {
                    writer.WriteLine("Mouse evaded Cat");
                }
            }
        }
    }
}
