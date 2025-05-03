using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class PerlinNoiseCave : MapTransformer, IAlgorithm
    {
        // atributos globales de los algoritmos
        private int[,] map;
        private float seed;
        
        // atributos especificos del algoritmo
        private bool areEdgeWalls;
        private float modifier;
        private float offsetX;
        private float offsetY;
        
        // elementos UI de este algoritmo
        private Toggle areEdgeWallsToggle;
        private FloatField modifierField;
        private FloatField offsetXField;
        private FloatField offsetYField;
            
        public Algorithms.Types Name => Algorithms.Types.PerlinNoiseCave;
        
        public PerlinNoiseCave(int width, int height, float seed, bool areEdgeWalls, float modifier,  float offsetX, float offsetY)
        {
            map = GenerateMatrix(width, height, true);
            this.seed = seed;
            this.areEdgeWalls =  areEdgeWalls;
            this.modifier = modifier;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }
        
        public int[,] Build()
        {
            // Resetear el mapa antes de construir
            map = GenerateMatrix(map.GetLength(0), map.GetLength(1), false);
            
            int nextPoint;

            for(int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    if(areEdgeWalls && (x == 0 || y == 0 || x == map.GetUpperBound(0) || y == map.GetUpperBound(1)))
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        nextPoint = Mathf.RoundToInt(Mathf.PerlinNoise(x * modifier + offsetX + seed, y * modifier + offsetY + seed));
                        map[x, y] = nextPoint;
                    }
                }
            }

            return map;
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            // inicialiacion
            areEdgeWallsToggle = new Toggle("Are Edge Walls");
            modifierField = new FloatField("Modifier");
            offsetXField = new FloatField("Offset X");
            offsetYField = new FloatField("Offset Y");
            
            // asignacion
            areEdgeWallsToggle.RegisterValueChangedCallback(evt =>
            {
                areEdgeWalls = evt.newValue;
            });

            modifierField.RegisterValueChangedCallback(evt =>
            {
                modifier = evt.newValue;
            });

            offsetXField.RegisterValueChangedCallback(evt =>
            {
                offsetX = evt.newValue;
            });

            offsetYField.RegisterValueChangedCallback(evt =>
            {
                offsetY = evt.newValue;
            });
            
            // agregar a la ventana
            parent.Add(areEdgeWallsToggle);
            parent.Add(modifierField);
            parent.Add(offsetXField);
            parent.Add(offsetYField);
        }

        public void SetSeed(float seed) => this.seed = seed;

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>
            {
                { "areEdgeWallsToggle", areEdgeWallsToggle },
                { "modifier", modifierField },
                { "offsetX", offsetXField },
                { "offsetY", offsetYField }
            };
        }
    }
}