using System.Collections.Generic;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    /// <summary>
    /// Interfaz que implementara todos los atributos y acciones que compartiran todos
    /// los diferentes tipos de algoritmos de las que disponga la tool.
    /// </summary>
    public interface IAlgorithm
    {
        /// <summary>
        /// Nombre del algoritmo
        /// </summary>
        Algorithms.Types Name { get; }
        
        /// <summary>
        /// Funcion que permite construir el mapa segun los parametros propocionados por el usuario
        /// </summary>
        /// <returns></returns>
        int[,] Build();
        
        /// <summary>
        /// Funcion que permite poder construir en la ventana de la tool, todos los campos propios
        /// y exclusivos del algoritmo seleccionado.
        /// </summary>
        /// <param name="parent"></param>
        void GetCustomSettingsUI(VisualElement parent);
        
        /// <summary>
        /// Funcion que permite indicarle al algoritmo, la semilla generada/seleccionada
        /// </summary>
        /// <param name="seed"></param>
        void SetSeed(float seed);
        
        /// <summary>
        /// Funcion que permite obtener un diccionario de todos los datos setteados por los campos
        /// propios del algoritmo seleccionado.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetAlgorithmParameters();
    }
}
