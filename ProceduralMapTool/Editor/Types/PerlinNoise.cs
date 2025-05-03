using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class PerlinNoise : MapTransformer, IAlgorithm
    {
        private int[,] map;
        private float seed;

        // Nombre del algoritmo para el dropdown
        public Algorithms.Types Name => Algorithms.Types.PerlinNoise;

        // Constructor: crea un mapa vacio y guarda la semilla inicial
        public PerlinNoise(int width, int height, float seed)
        {
            map = GenerateMatrix(width, height, true);
            this.seed = seed;
        }

        public void SetSeed(float seed) => this.seed = seed;
        
        public int[,] Build()
        {
            // reinicio de la matriz
            map = GenerateMatrix(map.GetLength(0), map.GetLength(1), true);

            int nextPoint;
            float reduction = 0.5f; // para centrar el ruido en [-0.5, 0.5]

            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                // Calcula la altura
                nextPoint = Mathf.FloorToInt((Mathf.PerlinNoise(x, seed) - reduction)
                                              * map.GetUpperBound(1));
                // Ajuste vertical al centro
                nextPoint += map.GetUpperBound(1) / 2;

                // Rellena desde 0 hasta la nueva altura
                for (int y = nextPoint; y >= 0; y--)
                {
                    map[x, y] = 1;
                }
            }

            return map;
        }
        
        public void GetCustomSettingsUI(VisualElement parent)
        {
            // no require nada
        }

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>();
        }
    }
}
