using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class AutomataCelularMoore : MapAutonomous, IAlgorithm
    {
        // atributos globales de los algoritmos
        private int[,] map;
        private float seed;
        
        // atributos especificos del algoritmo
        private int totalIterations;
        private bool areEdgeWalls;
        private float percentageFilled;
        
        // elementos UI de este algoritmo
        private IntegerField totalIterationsField;
        private Toggle areEdgeWallsField;
        private Slider percentageFilledField;
        
        public AutomataCelularMoore(int width, int height, float seed, int totalIterations, bool areEdgeWalls, float percentageFilled)
        {
            map = GenerateRandomMap(width, height, seed, percentageFilled, areEdgeWalls);
            this.seed = seed;
            this.totalIterations = totalIterations;
            this.areEdgeWalls = areEdgeWalls;
            this.percentageFilled = percentageFilled;
        }
        
        public Algorithms.Types Name => Algorithms.Types.AutomataCelularMoore;
        public int[,] Build()
        {
            map = GenerateRandomMap(map.GetLength(0), map.GetLength(1), seed, percentageFilled, areEdgeWalls);
            
            for(int i = 0; i < totalIterations; i++)
            {
                for (int x = 0; x <= map.GetUpperBound(0); x++)
                {
                    for (int y = 0; y <= map.GetUpperBound(1); y++)
                    {
                        int losetasVecinas = NearTileCount(map, x, y, true);
                        // Si estamos en un borde y losBordesSonMuros est� activado, ponemos muro
                        if(areEdgeWalls && (x == 0 || x == map.GetUpperBound(0) || y == 0 || y == map.GetUpperBound(1)))
                        {
                            map[x, y] = 1;
                        }
                        // si tenemos mas de 4 vecinas, ponemos suelo. en caso contrario, no. Si hay justo 4, no cambiamos nada
                        else if(losetasVecinas > 4)
                        {
                            map[x, y] = 1;
                        }
                        else if(losetasVecinas < 4)
                        {
                            map[x, y] = 0;
                        }
                    }
                }
            }

            return map;
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            totalIterationsField = new IntegerField("Total Iterations");
            areEdgeWallsField = new Toggle("Edge Walls");
            percentageFilledField = new Slider("Percentage Filled", 0, 1);
            
            // inicializar campos
            totalIterationsField.value = 3;
            percentageFilledField.value = 0.45f;

            totalIterationsField.RegisterValueChangedCallback(evt =>
            {
                totalIterations = evt.newValue;
            });

            areEdgeWallsField.RegisterValueChangedCallback(evt =>
            {
                areEdgeWalls = evt.newValue;
            });

            percentageFilledField.RegisterValueChangedCallback(evt =>
            {
                percentageFilled = evt.newValue;
            });
            
            parent.Add(totalIterationsField);
            parent.Add(areEdgeWallsField);
            parent.Add(percentageFilledField);
        }

        public void SetSeed(float seed) => this.seed = seed;

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>
            {
                { "totalIterations", totalIterations },
                { "areEdgeWalls", areEdgeWalls },
                { "percentageFilled", percentageFilled }
            };
        }
        
        // Funciones extras
        
        /// <summary>
        /// Funcion encargada de devolver el numero total de tiles vecinos 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="includeDiagonals"></param>
        /// <returns></returns>
        private int NearTileCount(int[,] map, int x, int y, bool includeDiagonals)
        {
            // lleva la cuenta de losetas vecinas
            int tileCount = 0;

            // recorrer las posiciones vecinas
            for (int nearX = x - 1; nearX <= x + 1; nearX++)
            {
                for(int nearY = y-1; nearY <= y+1; nearY++)
                {
                    if (nearX >= 0 && nearX <= map.GetUpperBound(0) && nearY >= 0 && nearY <= map.GetUpperBound(1))
                    {
                        if((nearX != x || nearY != y) && (includeDiagonals || (nearX == x || nearY == y)))
                        {
                            tileCount += map[nearX, nearY];
                        }
                    }
                }
            }

            return tileCount;
        }
    }
}