using System;
using System.Collections.Generic;

namespace CohonenMaps
{
    /// <summary>
    /// Вектор
    /// (входной слой, взвешенные связи и входные данные)
    /// </summary>
    internal class Vector : List<double>
    {
        /// <summary>
        /// Функция подстчета расстояния
        /// </summary>
        private IDistanceFunction _distanceFunc;

        public Vector(IDistanceFunction distanceFunction)
        {
            _distanceFunc = distanceFunction;
        }


        /// <summary>
        /// Расстояние между двумя векторами
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public double Distance(Vector vector)
        {
            if (Count != vector.Count)
            {
                throw new ArgumentException("Вектора должны иметь одинаковую длину", "vector");
            }

            return _distanceFunc.CalculateDistance(this, vector);
        }
    }
}
