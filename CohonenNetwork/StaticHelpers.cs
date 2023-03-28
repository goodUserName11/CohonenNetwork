using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CohonenNetwork
{
    /// <summary>
    /// Вспомогательные функции
    /// TODO NormalizeInput
    /// </summary>
    public class StaticHelpers
    {
        /// <summary>
        /// Нормализовать входные данные
        /// </summary>
        /// <param name="inputs">Входные данные</param>
        /// <returns>Нормализованные входные данные</returns>
        public static List<double[]> NormalizeInputs(List<double[]> inputs)
        {
            var res = new List<double[]>();

            double[] maxInputs = new double[inputs[0].Length];
            // пока не использую
            double[] minInputs = new double[inputs[0].Length];

            for (int j = 0; j < inputs[0].Length; j++) 
            {
                for (int i = 0; i < inputs.Count; i++)
                {
                    if(inputs[i][j] < minInputs[j])
                        minInputs[j] = inputs[i][j];

                    if(inputs[i][j] > maxInputs[j])
                        maxInputs[j] = inputs[i][j];
                }
            }

            for (int i = 0; i < inputs.Count; i++)
            {
                double[] normInput = new double[inputs[i].Length];
                for (int j = 0; j < normInput.Length; j++)
                {
                    normInput[j] = inputs[i][j] / maxInputs[j];
                }

                res.Add(normInput);
            }

            return res;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] NormalizeInput(List<double[]> inputs, List<double[]> normInputs, double[] input) 
        {
            if (normInputs[0].Length != inputs[0].Length || normInputs.Count != inputs.Count
                || input.Length != normInputs[0].Length || inputs[0].Length != input.Length)
            {
                throw new ArgumentException(
                    "Входные данные должны иметь одинаковый размер inputs, normInputs, input");
            }

            double[] normInput = new double[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                normInput[i] = input[i] / (inputs[0][i] / normInputs[0][i]);
            }

            return normInput;
            // Работает с погрешностями
            //inputs.Add(input);

            //return NormalizeInputs(inputs).Last();
        }

        /// <summary>
        /// Преобразовать double[] в Vector
        /// </summary>
        /// <param name="input"></param>
        /// <param name="distanceFunction">Функция рассчета расстояния между векторами</param>
        /// <returns></returns>
        public static Vector ValuesArrayToVector(double[] input, IDistanceFunction distanceFunction)
        {
            Vector res = new Vector(distanceFunction);

            for (int i = 0; i < input.Length; i++)
            {
                res.Add(input[i]);
            }

            return res;
        }

        /// <summary>
        /// Преобразовать List<double[]> в Vector[]
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="distanceFunction">Функция рассчета расстояния между векторами</param>
        /// <returns></returns>
        public static Vector[] ValuesArrayListToVectorArray(List<double[]> inputs, IDistanceFunction distanceFunction)
        {
            Vector[] res = new Vector[inputs.Count];

            for (int i = 0; i < inputs.Count; i++)
            {
                res[i] = ValuesArrayToVector(inputs[i], distanceFunction);
            }

            return res;
        }

        /// <summary>
        /// Подготовить нейросеть
        /// </summary>
        /// <param name="inputFilePath">Путь до файла с входными данными</param>
        /// <param name="numberOfEpoches">Кол-во эпох</param>
        /// <param name="numberOfNeurons">Кол-во нейронов</param>
        /// <param name="distanceFunction">Функция рассчета расстояния между векторами</param>
        /// <param name="inputs">Вернет входные данные</param>
        /// <param name="normInputs">Вернет нормализованные входные данные</param>
        /// <param name="startingLearningRate">Начальная скорость обучения</param>
        /// <param name="learningRateDropRate">Скорость падения скорости обучения</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static NeuralNetwork SetUpNetwork(string inputFilePath,
                                                 int numberOfEpoches,
                                                 int numberOfNeurons,
                                                 IDistanceFunction distanceFunction,
                                                 out List<double[]> inputs,
                                                 out List<double[]> normInputs,
                                                 double startingLearningRate = 1,
                                                 double learningRateDropRate = 0.01
                                                 )
        {
            #region ArgumentExceptions
            if (string.IsNullOrWhiteSpace(inputFilePath))
                throw new ArgumentException(
                    $"\"{nameof(inputFilePath)}\" не может быть пустым или содержать только пробел.",
                    nameof(inputFilePath));

            if(numberOfEpoches < 0)
                throw new ArgumentException(
                    $"\"{nameof(numberOfEpoches)}\" не может быть меньше нуля.",
                    nameof(numberOfEpoches));

            if (numberOfNeurons < 0)
                throw new ArgumentException(
                    $"\"{nameof(numberOfNeurons)}\" не может быть меньше нуля.",
                    nameof(numberOfNeurons));
            #endregion

            NeuralNetwork network;

            inputs = YetAnotherPerceptron
                .NetworkStatics
                .GetTeacherDataFromTxtFile(inputFilePath);

            normInputs = NormalizeInputs(inputs);

            #region Смотрю нормализованные данные
            //for (int i = 0; i < normInputs.Count; i++)
            //{
            //    for (int j = 0; j < normInputs[0].Length; j++)
            //    {
            //        Console.Write($"{normInputs[i][j]:0.0000},");
            //    }
            //    Console.WriteLine("");
            //}
            # endregion

            var inputVectors = 
                ValuesArrayListToVectorArray(normInputs, distanceFunction);

            network = new NeuralNetwork(numberOfNeurons,
                                        inputVectors,
                                        numberOfEpoches,
                                        startingLearningRate,
                                        learningRateDropRate,
                                        distanceFunction);

            return network;
        }

        /// <summary>
        /// Печать коллекции (через вызов ToString у итема)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        public static void PrintList<T>(List<T> values) /*where T : struct*/
        {
            for (int i = 0; i < values.Count; i++)
            {
                Console.WriteLine(values[i]);
            }
        }
    }
}
