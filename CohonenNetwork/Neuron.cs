using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CohonenNetwork
{
    /// <summary>
    /// Нейрон
    /// </summary>
    internal class Neuron
    {
        private static Random _random = new Random();
        private IDistanceFunction _distanceFunction;

        /// <summary>
        /// Позиция нейрона на гиперплосткости (веса)
        /// (вектор нейрона)
        /// </summary>
        public Vector Weights { get; }

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
        /// Обновить веса
        /// </summary>
        /// <param name="neighborship">Степень соседства</param>
        /// <param name="learningRate">Скорость обучения</param>
        public void UpdateWeights(double neighborship, double learningRate, Vector currVector)
        {
            // i - оно же l, оно же j
            for (int i = 0; i < Weights.Count; i++)
            {
                // Записи
                // k - итерация (эпоха)
                //W^(k+1) = W^(k) + nu^(k) + (x - w^k)
                // j         j        j            j Подстрочное
                //Weights[i] = Weights[i] + neighborship * learningRate + currVector[i] - neighborship;

                // Wikipedia
                //Wnew = Wold * (1 - nu * lr) + x * nu * lr
                // l      l            j(x)l          j(x)l    Подстрочное
                Weights[i] = Weights[i] * (1 - neighborship * learningRate) + currVector[i] * neighborship * learningRate;

            }
        }

        /// <summary>
        /// Рассчитывает расстояние между двумя нейронами
        /// </summary>
        /// <param name="neuron">Второй нейрон</param>
        /// <returns></returns>
        public double CalculateDistance(Neuron neuron) 
        { 
            return this.Weights.CalculateDistance(neuron.Weights);
        }

        /// <summary>
        /// Рассчитывает расстояние между вектором нейрона и другим вектором
        /// </summary>
        /// <param name="neuron">Второй нейрон</param>
        /// <returns></returns>
        public double CalculateDistance(Vector vector)
        {
            return this.Weights.CalculateDistance(vector);
        }
    }
}
