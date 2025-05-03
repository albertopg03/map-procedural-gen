using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class RandomWalkSmoothing : MapTransformer, IAlgorithm
    {
        // atributos globales de los algoritmos
        private int[,] map;
        private float seed;
        
        // atributos especificos del algoritmo
        private int minSectionWidth;
        
        // elementos UI de este algoritmo
        private SliderInt minSectionWidthSlider;
        
        public Algorithms.Types Name => Algorithms.Types.RandomWalkSmoothing;
        
        public RandomWalkSmoothing(int width, int height, float seed, int minSectionWidth)
        {
            map = GenerateMatrix(width, height, true);
            this.seed = seed;
            this.minSectionWidth = minSectionWidth;
        }
        
        public int[,] Build()
        {
            // Resetear el mapa antes de construir
            map = GenerateMatrix(map.GetLength(0), map.GetLength(1), true);
            
            Random.InitState(seed.GetHashCode());

            // altura en la que vamos a empezar
            int lastHeight = Random.Range(0, map.GetUpperBound(1));

            int sectionWidth = 0;

            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                if(sectionWidth >= minSectionWidth)
                {
                    // 0 sube, 1 baja, 2 igual
                    int siguienteMovimiento = Random.Range(0, 3);

                    // si sube...
                    if (siguienteMovimiento == 0 && lastHeight < map.GetUpperBound(1))
                    {
                        lastHeight++;
                    }
                    // si baja...
                    else if (siguienteMovimiento == 1 && lastHeight > 0)
                    {
                        lastHeight--;
                    }

                    // empezamos una nueva seccion de suelo con la misma altura
                    sectionWidth = 0;
                }

                sectionWidth++;

                for (int y = lastHeight; y >= 0; y--)
                {
                    map[x, y] = 1;
                }
            }

            return map;
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            minSectionWidthSlider = new SliderInt("Min Section Width", 0, 10)
            {
                value = minSectionWidth
            };

            minSectionWidthSlider.RegisterValueChangedCallback(evt =>
            {
                minSectionWidth = evt.newValue;
            });

            parent.Add(minSectionWidthSlider);
        }

        public void SetSeed(float seed) => this.seed = seed;

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>
            {
                { "minSectionWidth", minSectionWidth }
            };
        }
    }
}