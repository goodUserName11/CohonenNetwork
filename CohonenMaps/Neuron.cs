using System;
using System.Collections.Generic;
using System.Text;

namespace CohonenMaps
{
    /// <summary>
    /// Нейрон
    /// </summary>
    internal class Neuron
    {
        private static Random _random = new Random();

        /// <summary>
        /// Функция для подстчета расстояния
        /// </summary>
        IDistanceFunction _distanceFunction;

        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Взвешенные связи
        /// </summary>
        public Vector Weights { get; set; }

        public Neuron(int numOfWeights, IDistanceFunction distanceFunction)
        {
            Weights = new Vector(distanceFunction);
            _distanceFunction = distanceFunction;

            for (int i = 0; i < numOfWeights; i++)
            {
                Weights.Add(_random.NextDouble());
            }
        }

        /// <summary>
        /// Считает расстояние между двумя нейронами
        /// </summary>
        /// <param name="neuron">Второй нейрон</param>
        /// <returns>Расстояние между двумя нейронами</returns>
        public double Distance(Neuron neuron)
        {
            return _distanceFunction.CalculateDistance(this, neuron);
        }

        /// <summary>
        /// Обновить взвешенные связи
        /// </summary>
        /// <param name="input">Входные данные</param>
        /// <param name="distanceDecay">"Близость"</param>
        /// <param name="learningRate">Скорость обучения</param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdateWeights(Vector input, double distanceDecay, double learningRate)
        {
            if (input.Count != Weights.Count)
                throw new ArgumentException("Размерность input не соответствует Weights", "input");

            for (int i = 0; i < Weights.Count; i++)
            {
                Weights[i] += distanceDecay * learningRate * (input[i] - Weights[i]);
        }

    }
}
}
