namespace CohonenNetwork
{
    /// <summary>
    /// Функция определения расстояния
    /// (чтобы подменять реализацию)
    /// </summary>
    public interface IDistanceFunction
    {
        /// <summary>
        /// Считает расстояние между двумя векторами
        /// </summary>
        /// <param name="vector1">Начальный вектор</param>
        /// <param name="vector2">Конечный вектор</param>
        /// <returns>Расстояние между двумя векторами</returns>
        double CalculateDistance(Vector vector1, Vector vector2);

        /// <summary>
        /// Считает расстояние между двумя нейронами
        /// </summary>
        /// <param name="neuron1">Начальный нейрон</param>
        /// <param name="neuron2">Конечный нейрон</param>
        /// <returns>Расстояние между двумя нейронами</returns>
        //double CalculateDistance(Neuron neuron1, Neuron neuron2);
    }
}