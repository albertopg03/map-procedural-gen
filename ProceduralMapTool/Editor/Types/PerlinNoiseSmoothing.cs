using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class PerlinNoiseSmoothing : MapTransformer, IAlgorithm
    {
        // atributos globales de los algoritmos
        private int[,] map;
        private float seed;
        
        // atributos especificos de este algoritmo
        private int interval;

        // elementos UI de este algoritmo 
        private SliderInt intervalSlider;

        public void SetSeed(float seed) => this.seed = seed;
        
        public PerlinNoiseSmoothing(int width, int height, float seed, int interval)
        {
            map = GenerateMatrix(width, height, true);
            this.seed = seed;
            this.interval = interval;
        }

        public Algorithms.Types Name => Algorithms.Types.PerlinNoiseSmoothing;

        public int[,] Build()
        {
            // Resetear el mapa antes de construir
            map = GenerateMatrix(map.GetLength(0), map.GetLength(1), true);

            if (interval > 1)
            {
                Vector2Int currentPos, previousPos;
                List<int> noiseX = new List<int>();
                List<int> noiseY = new List<int>();

                for (int x = 0; x <= map.GetUpperBound(0) + interval; x += interval)
                {
                    int nextPoint = Mathf.FloorToInt(Mathf.PerlinNoise(x, seed) * map.GetUpperBound(1));
                    
                    noiseX.Add(x);
                    noiseY.Add(nextPoint);
                }

                for (int i = 1; i < noiseY.Count; i++)
                {
                    previousPos = new Vector2Int(noiseX[i - 1], noiseY[i - 1]);
                    currentPos = new Vector2Int(noiseX[i], noiseY[i]);
                    Vector2 diff = currentPos - previousPos;
                    float step = diff.y / interval;
                    float currentHeight = previousPos.y;

                    for (int x = previousPos.x; x < currentPos.x && x <= map.GetUpperBound(0); x++)
                    {
                        for (int y = Mathf.FloorToInt(currentHeight); y >= 0; y--)
                        {
                            map[x, y] = 1;
                        }
                        currentHeight += step;
                    }
                }
            }

            return map;
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            intervalSlider = new SliderInt("IntervalSlider", 1, 10)
            {
                value = interval
            };

            intervalSlider.RegisterValueChangedCallback(evt =>
            {
                interval = evt.newValue;
            });

            parent.Add(intervalSlider);
        }

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>
            {
                { "interval", interval }
            };
        }
    }
}