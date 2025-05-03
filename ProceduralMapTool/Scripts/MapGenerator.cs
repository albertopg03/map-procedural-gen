using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace MendedUtilities.ProceduralGeneration
{
    /// <summary>
    /// Clase encargada de generar el mapa procedural tras recibir los datos necesarios para poder construirlo
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        [Header("Data")]
        public AlgorithmData mapData;
        
        [Header("Tiles")]
        public Tilemap tilemap; 
        public TileBase tileBase;
        
        [Header("Seed")]
        public bool randomSeed;
        public float seed;

        private void Start()
        {
            if (randomSeed)
            {
                seed = Random.Range(0f, 10000f);

                // Reconstruir el algoritmo
                var algorithm = AlgorithmFactory.Create(mapData.algorithmName, mapData.width, mapData.height, seed);

                if (algorithm != null)
                {
                    int[,] map = algorithm.Build();
                    GenerateMap(map, tilemap, tileBase);
                }
                else
                {
                    Debug.LogWarning("Could not create algorithm from name: " + mapData.algorithmName);
                }
            }
            else
            {
                GenerateMap(mapData.To2DArray(), tilemap, tileBase);
            }
        }

    
        /// <summary>
        /// Funcion encargada de generar el mapa recibiendo el mapa construido previamente por
        /// el algoritmo seleccionado
        /// </summary>
        /// <param name="map"></param>
        /// <param name="tileMap"></param>
        /// <param name="tileBase"></param>
        private void GenerateMap(int[,] map, Tilemap tileMap, TileBase tileBase)
        {
            tileMap.ClearAllTiles();

            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    if (map[x, y] == 1)
                    {
                        tileMap.SetTile(new Vector3Int(x, y, 0), tileBase);
                    }
                }
            }
        }
    }   
}