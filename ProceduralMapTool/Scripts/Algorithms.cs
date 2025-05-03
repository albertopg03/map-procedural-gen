namespace MendedUtilities.ProceduralGeneration
{
    /// <summary>
    /// Clase estática que contiene la enumeración de los tipos de algoritmos disponibles
    /// para la generación procedural, y proporciona utilidades relacionadas.
    /// </summary>
    public static class Algorithms
    {
        /// <summary>
        /// Enumeración que representa todos los tipos de algoritmos de generación procedural
        /// disponibles en la aplicación.
        /// </summary>
        public enum Types
        {
            PerlinNoise,
            PerlinNoiseSmoothing,
            RandomWalk,
            RandomWalkSmoothing,
            PerlinNoiseCave,
            RandomWalkCave,
            DirectionalTunnel,
            RandomMap,
            AutomataCelularMoore,
            AutomataCelularVonNeumann,
            Dungeon
        }
    }
}