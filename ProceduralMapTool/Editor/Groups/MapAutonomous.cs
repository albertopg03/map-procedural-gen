using UnityEngine;

public class MapAutonomous
{
    /// <summary>
    /// Funcion encargada de generar un mapa aleatorio para los algoritmos especificos que implementen esta clase
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="seed"></param>
    /// <param name="percentageFilled">Indica el porcentaje de relleno del mapa a generar</param>
    /// <param name="areEdgeWalls">Indica si se desea que los bordes del mapa generado sean muros o no</param>
    /// <returns></returns>
    public static int[,] GenerateRandomMap(int width, int height, float seed, float percentageFilled, bool areEdgeWalls)
    {
        Random.InitState(seed.GetHashCode());

        int[,] map = new int[width, height];

        // recorremos todas las posiciones del mapa
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                if (areEdgeWalls && (x == 0 || x == map.GetUpperBound(0) || y == 0 || y == map.GetUpperBound(1)))
                {
                    // ponemos suelo si estamosen una posici�n del borde
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (Random.value < percentageFilled) ? 1 : 0;
                }
            }
        }

        return map;
    }
}
