using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using System.ComponentModel;

namespace CohonenNetwork
{
    /// <summary>
    /// Сеть Кохонена
    /// </summary>
    public class NeuralNetwork
    {
        /// <summary>
        /// Нейроны
        /// </summary>
        private Neuron[] _neurons;
        /// <summary>
        /// Входные данные
        /// </summary>
        private Vector[] _inputs;
        /// <summary>
        /// "Радиус соседства"
        /// (Определяет на каком расстоянии нейроны не будут считаться соседями)
        /// (Коэффициент соседства)
        /// (Скорость снижения соседства в зависимости от расстояния)
        /// </summary>
        private double _neighborshipRadius;
        /// <summary>
        /// Матрица "соседства" нейронов
        /// </summary>
        private double[,] _neighborshipMatrix;
        /// <summary>
        /// Кол-во эпох (итераций)
        /// </summary>
        private double _numberOfEpoches;
        /// <summary>
        /// Начальная скорость обучения
        /// </summary>
        private double _learningRate;
        /// <summary>
        /// Скорость уменьшения скорости обучения
        /// </summary>
        private double _learningRateDropRate;
        

        public NeuralNetwork(int numberOfNeurons,
                             Vector[] inputs,
                             int numberOfEpoches,
                             double startingLearningRate,
                             double learningRateDropRate,
                             IDistanceFunction distanceFunction)
        {
            _neurons = new Neuron[numberOfNeurons];
            _numberOfEpoches = numberOfEpoches;
            _learningRate = startingLearningRate;
            _learningRate = learningRateDropRate;
            _inputs = inputs;

            for (int i = 0; i < _neurons.Length; i++)
            {
                _neurons[i] = new Neuron(_inputs[0].Count, distanceFunction);
            }

            InitializeNeighborshipMatrix();
        }

        public void Train()
        {
            int iteration = 0;
            double learningRate = _learningRate;

            while (iteration < _numberOfEpoches)
            {
                // Смотрю положение нейронов
                Console.WriteLine($"Итерация {iteration + 1}");
                foreach (Neuron neuron in _neurons)
                {
                    foreach (double weight in neuron.Weights)
                    {
                        Console.Write($"{weight:0.00000},");
                    }
                    Console.WriteLine();
                }

                for (int i = 0; i < _inputs.Length; i++)
                {
                    // Выходное расстояние между нейроном и входным вектором
                    var outputs = new double[_neurons.Length];

                    for (int j = 0; j < _neurons.Length; j++)
                    {
                        outputs[j] = _neurons[j].CalculateDistance(_inputs[i]);
                    }

                    var minIndex = 0;
                    for (var j = 1; j < _neurons.Length; j++)
                    {
                        if (outputs[j] < outputs[minIndex])
                            minIndex = j;
                    }

                    for (int j = 0; j < _neurons.Length; j++)
                    {
                        _neurons[j].UpdateWeights(
                            _neighborshipMatrix[j, minIndex], learningRate, _inputs[i]);

                        //Console.WriteLine("###################");
                        //foreach (Neuron neuron in _neurons)
                        //{
                        //    foreach (double weight in neuron.Weights)
                        //    {
                        //        Console.Write($"{weight:0.00000},");
                        //    }
                        //    Console.WriteLine();
                        //}

                        //_neighborshipMatrix[j, minIndex] = Math.Pow(Math.E, -0.5 *
                        //_neurons[j].CalculateDistance(_neurons[minIndex]));
                    }

                }

                iteration++;
                learningRate = _learningRate / (iteration + _learningRateDropRate);
                // Падение скорости обучения
                //learningRate = _learningRate * Math.Exp(-(double)iteration / _numberOfEpoches);
            }
        }

        /// <summary>
        /// Пропустить вектор через нейронную сеть
        /// (Для обученной сети)
        /// (обработка по правилу «победитель забирает всё»)
        /// </summary>
        /// <param name="input">Входной вектор</param>
        /// <returns>Номер победившего нейрона (с нуля)</returns>
        public int AskNetwork(Vector input)
        {
            // Выходное расстояние между нейроном и входным вектором
            double[] outputs = new double[_neurons.Length];

            for (int i = 0; i < _neurons.Length; i++)
            {
                outputs[i] = _neurons[i].Weights.CalculateDistance(input);
            }

            var minIndex = 0;
            for (var i = 1; i < _neurons.Length; i++)
            {
                if (outputs[i] < outputs[minIndex])
                    minIndex = i;
            }

            return minIndex;
        }

        /// <summary>
        /// Подстчет индекса Rand
        /// </summary>
        /// <param name="outputs">Результаты с номерами классов</param>
        /// <returns>Индекс Rand</returns>
        public double RandIndex(List<int> outputs)
        {
            // Элементы принадлежат одному кластеру и одному классу
            int Tp = 0;
            // Элементы принадлежат одному кластеру, но разным классам
            int Fp = 0;
            // Элементы принадлежат разным кластерам, но одному классу
            int Fn = 0;
            // Элементы принадлежат разным кластерам и разным классам
            int Tn = 0;

            List<int> networkResults = new List<int>();

            for (int j = 0; j < outputs.Count; j++)
            {
                networkResults.Add(AskNetwork(_inputs[j]));
            }

            for (int i = 0; i < networkResults.Count - 1; i++)
            {
                for (int j = 1; j < networkResults.Count; j++)
                {
                    if (networkResults[i] == networkResults[j] && outputs[i] == outputs[j])
                        Tp++;
                    else if (networkResults[i] == networkResults[j] && outputs[i] != outputs[j])
                        Fp++;
                    else if (networkResults[i] != networkResults[j] && outputs[i] == outputs[j])
                        Fn++;
                    else if (networkResults[i] != networkResults[j] && outputs[i] != outputs[j])
                        Tn++;
                }
            }


            return (Tp + Tn + 0.0)/(Tp + Tn + Fp + Fn);
        }

        /// <summary>
        /// Создание и заполнение матрицы "соседства"
        /// </summary>
        private void InitializeNeighborshipMatrix()
        {
            _neighborshipMatrix = new double[_neurons.Length, _neurons.Length];

            for (int i = 0; i < _neighborshipMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _neighborshipMatrix.GetLength(1); j++)
                {
                    // y=e^{-1/2*x}
                    _neighborshipMatrix[i, j] = Math.Pow(Math.E, -0.5 *
                        _neurons[i].CalculateDistance(_neurons[j]));
                }
            }
        }
    }
}
