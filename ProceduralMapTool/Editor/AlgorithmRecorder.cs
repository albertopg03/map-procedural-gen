using UnityEngine;
using UnityEditor;

namespace MendedUtilities.ProceduralGeneration
{
    /// <summary>
    /// Clase que se ejecutará automaticamente al abrir la ventana. Permite poder llamar a la factoria
    /// de algoritmos y construir todos los que tendrá disponibles para elegir la tool.
    /// 
    /// IMPORTANTE -> Los datos hardcodeados son los por defecto, ya que luego se sobreescribiran
    /// con lo que vaya eligiendo el usuario.
    /// </summary>
    [InitializeOnLoad]
    public class AlgorithmRecorder
    {
        static AlgorithmRecorder()
        {
            AlgorithmFactory.Register("PerlinNoise", (w, h, s) => new PerlinNoise(w, h, s));
            AlgorithmFactory.Register("PerlinNoiseSmoothing", (w, h, s) => new PerlinNoiseSmoothing(w, h, s, 3));
            AlgorithmFactory.Register("RandomWalk", (w, h, s) => new RandomWalk(w, h, s));
            AlgorithmFactory.Register("RandomWalkSmoothing", (w, h, s) => new RandomWalkSmoothing(w, h, s, 1));
            AlgorithmFactory.Register("PerlinNoiseCave", (w, h, s) => new PerlinNoiseCave(w, h, s, false, 0.1f, 0, 0));
            AlgorithmFactory.Register("RandomWalkCave", (w, h, s) => new RandomWalkCave(w, h, s, 0.25f, false, false));
            AlgorithmFactory.Register("DirectionalTunnel", (w, h, s) => new DirectionalTunnel(w, h, s, 0.75f, 0.75f, 1, 4, 2));
            AlgorithmFactory.Register("RandomMap", (w, h, s) => new RandomMap(w, h, s, 0.45f, false));
            AlgorithmFactory.Register("AutomataCelularMoore", (w, h, s) => new AutomataCelularMoore(w, h, s, 3, false, 0.45f));
            AlgorithmFactory.Register("AutomataCelularVonNeumann", (w, h, s) => new AutomataCelularVonNeumann(w, h, s, 3, false, 0.45f));
        }
    }
}