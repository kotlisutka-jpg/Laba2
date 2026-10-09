using System;

namespace CatAndMouseGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Запуск игры Кошки-Мышки (Лабораторная 2)...");

            // Настраиваем файлы
            Game.InputFile = "ChaseData.txt";
            Game.OutFile = "PursuitLog.txt";

            // Создаем игру с базовым размером поля 16
            Game game = new Game(16);
            game.Run();

            Console.WriteLine("Игра завершена! Результаты записаны в Laba2/PursuitLog.txt");
        }
    }
}