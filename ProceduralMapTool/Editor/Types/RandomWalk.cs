using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class RandomWalk: MapTransformer, IAlgorithm
    {
        private int[,] map;
        private float seed;

        // Nombre del algoritmo para el dropdown
        public Algorithms.Types Name => Algorithms.Types.RandomWalk;

        // Constructor: crea un mapa vacio y guarda la semilla inicial
        public RandomWalk(int width, int height, float seed)
        {
            map = GenerateMatrix(width, height, true);
            this.seed = seed;
        }
        
        public int[,] Build()
        {
            // reinicio de la matriz
            map = GenerateMatrix(map.GetLength(0), map.GetLength(1), true);
            
            // La semilla de nuetro Random
            Random.InitState(seed.GetHashCode());

            // altura en la que vamos a empezar
            int lastHeight = Random.Range(0, map.GetUpperBound(1));

            for(int x = 0; x <= map.GetUpperBound(0); x++)
            {
                // 0 sube, 1 baja, 2 igual
                int nextMovement = Random.Range(0, 3);

                // subimos
                if(nextMovement == 0 && lastHeight < map.GetUpperBound(1))
                {
                    lastHeight++;
                }
                // bajamos
                else if(nextMovement == 1 && lastHeight > 0)
                {
                    lastHeight--;
                }

                // rellenamos de suelo desde la ultimaAltura hasta abajo
                for(int y = lastHeight; y >= 0; y--)
                {
                    map[x, y] = 1;
                }
            }

            return map;
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            // no requiere nada
        }

        public void SetSeed(float seed) => this.seed = seed;

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>();
        }
    }
}