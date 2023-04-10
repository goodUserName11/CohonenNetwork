using System;
using System.Collections.Generic;

namespace CohonenNetwork
{
    /// <summary>
    /// Просто Main
    /// <para>TODO: </para>
    /// <list type="bullet">
    /// <item>ПЕРЕРАСЧЕТ таблицы соседства</item>
    /// <item>Сделать проверку сети</item>
    /// <item>NormalizeInput</item>
    /// </list>
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello NeuroWorld! Part 2");

            string inputFilePath;
            string outputFilePath;

            int numberOfEpoches;
            

            if (args.Length == 2)
            {
                inputFilePath = args[0];
                outputFilePath = args[1];
            }
            else
            {
                Console.WriteLine("Введите путь для файла с входными данными");
                inputFilePath = Console.ReadLine();

                Console.WriteLine("Введите путь для файла с результатами");
                outputFilePath = Console.ReadLine();
            }

            Console.WriteLine("Введите кол-во эпох");
            int.TryParse(Console.ReadLine(), out numberOfEpoches);

            var network = StaticHelpers.SetUpNetwork(
                inputFilePath, numberOfEpoches, 3, new ManhattanDistance(), out var inputs, out var normInputs, 1, 0.1);

            network.Train();

            var outputs = StaticHelpers.GetTeacherDataFromTxtFile(outputFilePath);

            List<int> outputClasses = new List<int>();

            foreach (var output in outputs)
            {
                outputClasses.Add(Convert.ToInt32(output[0]));
            }

            Console.WriteLine("Индекс Rand: {0:0.0000}", network.RandIndex(outputClasses));

            while (true)
            {
                Console.WriteLine("Вводите входные данные");
                Console.WriteLine("(Чтобы выйти бросьте пустой строкой)");

                var inputString = Console.ReadLine();

                if (inputString == "") break;

                var input = StaticHelpers.StringToDoubleValues(inputString);

                var vector = StaticHelpers
                    .ValuesArrayToVector(
                        StaticHelpers.NormalizeInput(inputs, normInputs, input),
                        new ManhattanDistance()
                    );

                var res = network.AskNetwork(vector);

                Console.WriteLine($"Это класс № {res}");
            }

            Console.WriteLine("Ok");
        }
    }
}
