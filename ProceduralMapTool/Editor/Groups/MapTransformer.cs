using UnityEngine;

public class MapTransformer
{
    /// <summary>
    /// Funcion encargada de generar e inicializar el mapa segun el algoritmo que se haya seleccionado
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="isEmpty"></param>
    /// <returns></returns>
    public virtual int[,] GenerateMatrix(int width, int height, bool isEmpty)
    {
        int[,] map = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = isEmpty ? 0 : 1;
            }
        }

        return map;
    }
}
