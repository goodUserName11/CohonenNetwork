using System;

namespace CohonenNetwork
{
    // Манхэттенское расстояние
    public class ManhattanDistance : IDistanceFunction
    {
        public double CalculateDistance(Vector vector1, Vector vector2)
        {
            double distance = 0;

            if (vector1.Count != vector2.Count) 
            {
                throw new ArgumentException("Вектора должны иметь одинаковую длину", "vector1");
            }

            // Вроде лаконично, если бы                      [не оно                    ]
            // distance = vector1.Select(xn => Math.Abs(xn - vector2[vector1.IndexOf(xn)])).Sum();

            for (int i = 0; i < vector1.Count; i++)
                distance += Math.Abs(vector1[i] - vector2[i]);

            return distance;
        }

        //public double CalculateDistance(Neuron neuron1, Neuron neuron2)
        //{
        //    return Math.Abs(neuron1.X - neuron2.X) + Math.Abs(neuron1.Y - neuron2.Y);
        //}
    }
}
