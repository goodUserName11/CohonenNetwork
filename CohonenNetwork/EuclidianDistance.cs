using System;
using System.Linq;

namespace CohonenNetwork
{
    public class EuclidianDistance : IDistanceFunction
    {
        public double CalculateDistance(Vector vector1, Vector vector2)
        {
            double distance = 0;

            if (vector1.Count != vector2.Count)
            {
                throw new ArgumentException("Вектора должны иметь одинаковую длину", "vector1");
            }

            // Вроде лаконично, если бы                   [не оно                    ]
            distance = vector1.Select(xn => Math.Pow(xn - vector2[vector1.IndexOf(xn)], 2)).Sum();

            return distance;
        }

        //public double CalculateDistance(Neuron neuron1, Neuron neuron2)
        //{
        //    return Math.Pow((neuron1.X - neuron2.X), 2) +Math.Pow((neuron1.Y - neuron2.Y), 2);
        //}
    }
}
