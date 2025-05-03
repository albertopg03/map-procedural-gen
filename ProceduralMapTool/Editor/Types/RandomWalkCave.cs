using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class RandomWalkCave : MapTransformer, IAlgorithm
    {
        // atributos globales de los algoritmos
        private int[,] map;
        private float seed;
        
        // atributos especificos del algoritmo
        private float percentageToDelete;
        private bool areEdgeWalls;
        private bool isDiagonalMove;
        
        // elementos UI de este algoritmo
        private Slider percentageToDeleteField;
        private Toggle areEdgeWallsToggle;
        private Toggle isDiagonalMoveToggle;
        
        public Algorithms.Types Name => Algorithms.Types.RandomWalkCave;
        
        public RandomWalkCave(int width, int height, float seed, float percentageToDelete, bool areEdgeWalls, bool isDiagonalMove)
        {
            map = GenerateMatrix(width, height, true);
            this.seed = seed;
            this.percentageToDelete = percentageToDelete;
            this.areEdgeWalls = areEdgeWalls;
            this.isDiagonalMove = isDiagonalMove;
        }
        
        public int[,] Build()
        {
            // Resetear el mapa antes de construir
            map = GenerateMatrix(map.GetLength(0), map.GetLength(1), false);
            
            // la semilla de nuestro random
            Random.InitState(seed.GetHashCode());
    
            // definimos los limites
            int minValue = 0;
            int maxValueX = map.GetUpperBound(0);
            int maxValueY = map.GetUpperBound(1);
            int width = map.GetUpperBound(0) + 1; // +1 para que incluya el 0
            int high = map.GetUpperBound(1) + 1;
    
            if (areEdgeWalls)
            {
                minValue++;
                maxValueX--;
                maxValueY--;
    
                width -= 2;
                high -= 2;
            }
    
            // definir la posicion de inicio en X y en Y
            int posX = Random.Range(minValue, maxValueX);
            int posY = Random.Range(minValue, maxValueY);
    
            // calculamos la cantidad de losetas a eliminar
            int countTilesToRemove = Mathf.FloorToInt(width * high * percentageToDelete);
    
            // para contar cuantas losetas llevamos eliminadas
            int removedTiles = 0;
    
            while(removedTiles < countTilesToRemove)
            {
                if(map[posX, posY] == 1)
                {
                    map[posX, posY] = 0;
                    removedTiles++;
                }
    
                
                if (isDiagonalMove) 
                {
                    int randomDirX = Random.Range(-1, 2);
                    int randomDirY = Random.Range(-1, 2);
    
                    posX += randomDirX;
                    posY += randomDirY;
                }
                else // si no nos movemos en digonal. Solo arriba, abajo, izquierda y derecha
                { 
                    int randomDir = Random.Range(0, 4);
    
                    switch (randomDir)
                    {
                        case 0: // arriba
                            posY++;
                            break;
    
                        case 1: // abajo
                            posY--;
                            break;
    
                        case 2: // izquierda
                            posX--;
                            break;
    
                        case 3: // derecha
                            posX++;
                            break;
                    }
                }
    
                // nos aseguramos de que no nos salimos del �rea de trabajo
                posX = Mathf.Clamp(posX, minValue, maxValueX);
                posY = Mathf.Clamp(posY, minValue, maxValueY);
            }
    
            return map;
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            percentageToDeleteField = new Slider("Percentage to Delete", 0, 1);
            areEdgeWallsToggle = new Toggle("Edge Walls");
            isDiagonalMoveToggle = new Toggle("Diagonal Move");
            
            // valores por defecto
            percentageToDeleteField.value = 0.25f;
            
            percentageToDeleteField.RegisterValueChangedCallback(evt =>
            {
                percentageToDelete = evt.newValue;
            });

            areEdgeWallsToggle.RegisterValueChangedCallback(evt =>
            {
                areEdgeWalls = evt.newValue;
            });
            
            isDiagonalMoveToggle.RegisterValueChangedCallback(evt =>
            {
                isDiagonalMove = evt.newValue;
            });
            
            parent.Add(percentageToDeleteField);
            parent.Add(areEdgeWallsToggle);
            parent.Add(isDiagonalMoveToggle);
        }

        public void SetSeed(float seed) => this.seed = seed;

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>
            {
                { "percentageToDelete", percentageToDelete },
                { "areEdgeWalls", areEdgeWalls },
                { "isDiagonalMove", isDiagonalMove }
            };
        }
    }
}