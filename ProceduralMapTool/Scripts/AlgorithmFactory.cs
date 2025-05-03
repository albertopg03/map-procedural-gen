using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MendedUtilities.ProceduralGeneration
{
    /// <summary>
    /// Clase estática que implementa el patrón Factory para instanciar diferentes algoritmos
    /// de generación procedural registrados dinámicamente. Permite crear algoritmos según su nombre
    /// y recuperar una lista de todos los algoritmos disponibles.
    /// </summary>
    public static class AlgorithmFactory
    {
        /// <summary>
        /// Diccionario interno que almacena los algoritmos registrados.
        /// La clave es el nombre del algoritmo, y el valor es una función que construye instancias de ese algoritmo.
        /// </summary>
        private static readonly Dictionary<string, Func<int, int, float, IAlgorithm>> Registry =
            new Dictionary<string, Func<int, int, float, IAlgorithm>>();

        /// <summary>
        /// Registra un nuevo algoritmo en la fábrica.
        /// </summary>
        /// <param name="name">Nombre identificador único del algoritmo.</param>
        /// <param name="constructor">Función que construye una instancia del algoritmo dado su ancho, alto y semilla.</param>
        public static void Register(string name, Func<int, int, float, IAlgorithm> constructor)
        {
            Registry[name] = constructor;
        }

        /// <summary>
        /// Devuelve una lista con todos los nombres de los algoritmos actualmente registrados.
        /// </summary>
        /// <returns>Lista de nombres de algoritmos.</returns>
        public static List<string> GetAllAlgorithmNames() => Registry.Keys.ToList();

        /// <summary>
        /// Crea una nueva instancia de un algoritmo registrado.
        /// </summary>
        /// <param name="name">Nombre del algoritmo a instanciar.</param>
        /// <param name="width">Ancho del área de generación.</param>
        /// <param name="height">Alto del área de generación.</param>
        /// <param name="seed">Valor de semilla para la generación procedural.</param>
        /// <returns>Instancia del algoritmo solicitado, o null si no se encuentra registrado.</returns>
        public static IAlgorithm Create(string name, int width, int height, float seed)
        {
            if (Registry.TryGetValue(name, out var constructor))
                return constructor(width, height, seed);

            Debug.LogError($"No algorithm found with the name: {name}");
            return null;
        }
    }
}
