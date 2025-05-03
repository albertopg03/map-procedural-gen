using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MendedUtilities.ProceduralGeneration
{
    public class DirectionalTunnel : MapTransformer, IAlgorithm
    {
        // atributos globales de los algoritmos
        private int[,] map;
        private float seed;
        
        // atributos especificos del algoritmo
        private float roughness;
        private float displacement;
        private int minWidth;
        private int maxWidth;
        private int maxDisplacement;
        
        // elementos UI de este algoritmo
        private Slider roughnessField;
        private Slider displacementField;
        private IntegerField minWidthField;
        private IntegerField maxWidthField;
        private IntegerField maxDisplacementField;
        
        public DirectionalTunnel(int width, int height, float seed, float roughness, float displacement, int minWidth, int maxWidth, int maxDisplacement)
        {
            map = GenerateMatrix(width, height, false);
            this.seed = seed;
            this.roughness = roughness;
            this.displacement = displacement;
            this.minWidth = minWidth;
            this.maxWidth = maxWidth;
            this.maxDisplacement = maxDisplacement;
        }
        
        public Algorithms.Types Name => Algorithms.Types.DirectionalTunnel;
        public int[,] Build()
        {
            // Resetear el mapa antes de construir
            map = GenerateMatrix(map.GetLength(0), map.GetLength(1), false);
            
            // este valor va desde su valor en negativo hasta el valor positivo
            // en este caso, con el vaor 1, el ancho del t�nel es 3 (-1, 0, 1)
            int tunnelWidth = 1;

            // empezamos en el medio. Obtenemos la posici�n X del centro del t�nel
            int x = map.GetUpperBound(0) / 2;

            // la semilla del random
            Random.InitState(seed.GetHashCode());

            // recorremos el ancho (esto es para el caso de que sea vertical, aunque se puede cambiar el c�digo para hacerlo horizontal)
            for(int y = 0; y <= map.GetUpperBound(1); y++)
            {
                for(int i = -tunnelWidth; i <= tunnelWidth; i++)
                {
                    map[x + i, y] = 0;
                }

                // comprobamos si cambiamos el ancho bas�ndonos en la aspereza
                if(Random.value < roughness)
                {
                    // obtenemos elaeatoriamente la cantidad que cambiaremos el ancho
                    int changeInWidth = Random.Range(-minWidth, maxWidth);
                    tunnelWidth += changeInWidth;

                    tunnelWidth = Mathf.Clamp(tunnelWidth, minWidth, maxWidth);
                }

                // comprobamos si cambiamos la posici�n central del t�nel
                if(Random.value < displacement)
                {
                    int changeInX = Random.Range(-maxDisplacement, maxDisplacement);
                    x += changeInX;

                    x = Mathf.Clamp(x, maxWidth + 1, map.GetUpperBound(0) - maxWidth);
                }
            }

            return map;
        }

        public void GetCustomSettingsUI(VisualElement parent)
        {
            roughnessField = new Slider("Roughness", 0, 1);
            displacementField = new Slider("Displacement", 0, 1);
            maxDisplacementField = new IntegerField("Max Displacement");
            minWidthField = new IntegerField("Min Width");
            maxWidthField = new IntegerField("Max Width");

            roughnessField.RegisterValueChangedCallback(evt =>
            {
                roughness = evt.newValue;
            });

            displacementField.RegisterValueChangedCallback(evt =>
            {
                displacement = evt.newValue;
            });

            maxDisplacementField.RegisterValueChangedCallback(evt =>
            {
                maxDisplacement = evt.newValue;
            });

            minWidthField.RegisterValueChangedCallback(evt =>
            {
                minWidth = evt.newValue;
            });

            maxWidthField.RegisterValueChangedCallback(evt =>
            {
                maxWidth = evt.newValue;
            });
            
            parent.Add(roughnessField);
            parent.Add(displacementField);
            parent.Add(maxDisplacementField);
            parent.Add(minWidthField);
            parent.Add(maxWidthField);
        }

        public void SetSeed(float seed) => this.seed = seed;

        public Dictionary<string, object> GetAlgorithmParameters()
        {
            return new Dictionary<string, object>
            {
                { "roughness", roughness },
                { "displacement", displacement },
                { "maxDisplacement", maxDisplacement },
                { "minWidth", minWidth },
                { "maxWidth", maxWidth }
            };
        }
    }
}