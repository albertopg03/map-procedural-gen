using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class RandomMap : MapAutonomous, IAlgorithm
    {
        // atributos globales de los algoritmos
        private int[,] map;
        private float seed;
        
        // atributos especificos del algoritmo
        private float percentageFilled;
        private bool areEdgeWalls;
        
        // elementos UI de este algoritmo
        private Slider percentageFilledSlider;
        private Toggle areEdgeWallsToggle;
        
        public RandomMap(int width, int height, float seed, float percentageFilled, bool areEdgeWalls)
        {
            map = GenerateRandomMap(width, height, seed, percentageFilled, areEdgeWalls);
            this.seed = seed;
            this.percentageFilled = percentageFilled;
            this.areEdgeWalls = areEdgeWalls;
        }
        
        public Algorithms.Types Name => Algorithms.Types.RandomMap;
        public int[,] Build()
        {
            return GenerateRandomMap(map.GetLength(0), map.GetLength(1), seed, percentageFilled, areEdgeWalls);
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            percentageFilledSlider = new Slider("Percentage Filled", 0, 1);
            areEdgeWallsToggle = new Toggle("Are Edge Walls");
            
            // inicializar valores por defecto
            percentageFilledSlider.value = 0.45f;

            percentageFilledSlider.RegisterValueChangedCallback(evt =>
            {
                percentageFilled = evt.newValue;
            });

            areEdgeWallsToggle.RegisterValueChangedCallback(evt =>
            {
                areEdgeWalls = evt.newValue;
            });
            
            parent.Add(percentageFilledSlider);
            parent.Add(areEdgeWallsToggle);
        }

        public void SetSeed(float seed) => this.seed = seed;

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>
            {
                { "percentageFilled", percentageFilled },
                { "areEdgeWalls", areEdgeWalls }
            };
        }
    }
}