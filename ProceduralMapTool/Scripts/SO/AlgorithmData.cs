using MendedUtilities.ProceduralGeneration;
using UnityEngine;

/// <summary>
/// ScriptableObject que almacena los datos resultantes de un algoritmo de generación procedural.
/// Incluye dimensiones, celdas en formato 1D, y el nombre del algoritmo utilizado.
/// </summary>
[CreateAssetMenu(fileName = "AlgorithmData", menuName = "Scriptable Objects/AlgorithmData")]
public class AlgorithmData : ScriptableObject
{
    public int width;
    public int height;
    public int[] cells;
    
    public string algorithmName;

    /// <summary>
    /// Convierte el array unidimensional de celdas a una matriz bidimensional.
    /// </summary>
    /// <returns>Arreglo 2D [width, height] con los valores de las celdas.</returns>
    public int[,] To2DArray()
    {
        var result = new int[width, height];

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
            result[x, y] = this[x, y];

        return result;
    }

    /// <summary>
    /// Indexador para acceder a las celdas como si fueran una matriz 2D.
    /// </summary>
    /// <param name="x">Coordenada X.</param>
    /// <param name="y">Coordenada Y.</param>
    /// <returns>Valor de la celda en la posicion (x, y).</returns>
    public int this[int x, int y]
    {
        get => cells[y * width + x];
        set => cells[y * width + x] = value;
    }
}