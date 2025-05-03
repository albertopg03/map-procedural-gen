using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace MendedUtilities.ProceduralGeneration
{
    /// <summary>
    /// Clase encargada de cargar y permitir interacturar con el usuario para la creacion de
    /// un mapa procedural.
    /// </summary>
    public class AssistantEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset;

        // campos tiles
        private ObjectField tilemapField;
        private ObjectField tileBaseField;
        
        // campos para la dimension del mapa
        private IntegerField wideField;
        private IntegerField highField;
        
        // semilla del mapa
        private FloatField seedField;
        private Toggle randomSeedField;
        
        // contenedores
        private VisualElement mapPreviewContainer;
        private VisualElement basicParametersContainer;
        private VisualElement algorithmSectionParams;
        
        // previsualizacion del mapa via textura
        private Texture2D previewTexture;
        
        // botones de accion
        private Button previewMapBtn;
        private Button createGeneratorBtn;
        private Button createRandomMapsBtn;
        
        // seleccion de un algoritmo
        private DropdownField dropdownAlgorithms;
        private Label labelAlgorithmSectionParams;
        private IAlgorithm algorithmSelected;
        
        // mapa a construir
        private int[,] map;

        /// <summary>
        /// Funcion encargada de permitir mostrar la ventana de la tool
        /// </summary>
        [MenuItem("Tools/Generador Procedural")]
        public static void ShowWindow()
        {
            AssistantEditor wnd = GetWindow<AssistantEditor>();
            wnd.titleContent = new GUIContent("Asistente Procedural");
            wnd.minSize = new Vector2(400, 600);
        }

        /// <summary>
        /// Inicializa cada uno de los campos de la ventana de la herramienta.
        /// En esta funcion solo se inizializan los campos que no presentan ningun tipado especial ni custom.
        /// </summary>
        /// <param name="layout"></param>
        private void InitCoreFields(VisualElement layout)
        {
            // campos integer
            wideField = layout.Q<IntegerField>("WideField");
            highField = layout.Q<IntegerField>("HighField");
            
            // campos float
            seedField = layout.Q<FloatField>("SeedField");
            
            // campos toggle
            randomSeedField = layout.Q<Toggle>("RandomSeedLabel");
            
            // elementos Button
            previewMapBtn = layout.Q<Button>("PreviewMapBtn");
            createRandomMapsBtn = layout.Q<Button>("CreateRandomMapsBtn");
            createGeneratorBtn = layout.Q<Button>("CreateGeneratorBtn");
            
            // contenedores
            mapPreviewContainer = layout.Q<VisualElement>("MapPreviewContainer");
            basicParametersContainer = layout.Q<VisualElement>("BasicParametersContainer");
            algorithmSectionParams = layout.Q<VisualElement>("AlgorithmSectionParams");
            
            // campos dropdown
            dropdownAlgorithms = layout.Q<DropdownField>("DropdownAlgorithms");

            // elementos label
            labelAlgorithmSectionParams = layout.Q<Label>("AlgorithmParamsLabel");
        }

        /// <summary>
        /// Inicializa cada uno de los campos de la ventana de la herramienta que
        /// repreenten algun tipado especial o custom.
        /// </summary>
        private void InitCustomFields()
        {
            tilemapField = new ObjectField("Tilemap") { objectType = typeof(Tilemap), name = "TilemapField" };
            tileBaseField = new ObjectField("TileBase") { objectType = typeof(TileBase), name = "TileBaseField" };
        }

        /// <summary>
        /// Funcion encargada de construir el contenido de la ventana de la tool
        /// </summary>
        public void CreateGUI()
        {
            var root = rootVisualElement;
            var layout = m_VisualTreeAsset.Instantiate();
            root.Add(layout);
            
            InitCoreFields(layout);
            
            // por defecto, oculta lo relacionado con los parametros de un algoritmo en especifico
            labelAlgorithmSectionParams.style.display = DisplayStyle.None;
            algorithmSectionParams.style.display = DisplayStyle.None;
            
            // rellena los datos del selector de algoritmo
            dropdownAlgorithms.choices = AlgorithmFactory.GetAllAlgorithmNames();

            // reacciona a un cambio de valor del algoritmo seleccionado 
            dropdownAlgorithms.RegisterValueChangedCallback(evt =>
            {
                // inicializar el objeto algoritmo seleccionado
                algorithmSelected = AlgorithmFactory.Create(evt.newValue, wideField.value, highField.value, seedField.value);
                
                if(algorithmSelected == null && algorithmSelected.GetAlgorithmParameters().Count < 1) return;
                
                algorithmSectionParams.Clear();
                
                // en el momento en el que se seleccione un algoritmo, se muestran sus parametros
                labelAlgorithmSectionParams.style.display = DisplayStyle.Flex;
                algorithmSectionParams.style.display = DisplayStyle.Flex;
                
                // carga a parte en la tool los campos propios del algoritmo seleccionado
                algorithmSelected.GetCustomSettingsUI(algorithmSectionParams);
            });

            // subscripcion de los botones
            previewMapBtn.clicked += BuildPreview;
            createGeneratorBtn.clicked += () => CreateMap();
            createRandomMapsBtn.clicked += () => CreateMap(true);

            InitCustomFields();
            
            basicParametersContainer.Add(tilemapField);
            basicParametersContainer.Add(tileBaseField);
        }

        /// <summary>
        /// Funcion encargada de mostrar en la ventana, una previsualizacion del mapa a generar
        /// en funcion de la semilla y el algoritmo seleccionado.
        /// </summary>
        private void BuildPreview()
        {
            float seed = seedField.value;

            if (randomSeedField.value)
            {
                seed = Random.Range(0f, 10000f);
                seedField.value = seed;
            }

            algorithmSelected.SetSeed(seed);

            map = algorithmSelected.Build();
            previewTexture = MapToTexture(map);
            
            ShowPreviewMap();
        }

        /// <summary>
        /// Funcion encargada de cargar en la ventana la imagen donde se construira la previsualizacion del mapa.
        /// </summary>
        private void ShowPreviewMap()
        {
            mapPreviewContainer.Clear();
            
            var img = new Image { image = previewTexture, scaleMode = ScaleMode.ScaleToFit };
            
            // settea el aspecto de la imagen que representa la previsualizacion del mapa
            img.style.width = 300;
            img.style.height = 300;
            img.style.marginLeft = StyleKeyword.Auto;
            img.style.marginRight = StyleKeyword.Auto;
            img.style.marginTop = 10;
            img.style.marginBottom = 10;

            mapPreviewContainer.style.justifyContent = Justify.Center;
            mapPreviewContainer.style.alignItems = Align.Center;
            mapPreviewContainer.Add(img);
        }

        /// <summary>
        /// Funcion encargada de transformar el mapa que se desea generar, a textura, para poder cargarlo
        /// en una imagen y previsualizarlo.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static Texture2D MapToTexture(int[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            
            var texture = new Texture2D(width, height) { filterMode = FilterMode.Point };
            
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    texture.SetPixel(x, y, map[x, y] == 1 ? Color.black : Color.white);
            
            texture.Apply();
            
            return texture;
        }
        
        /// <summary>
        /// Funcion encargada de generar un GameObject que tendra la capacidad de poder crear un mapa
        /// en runtime (al ejecutar el juego), en funcion de los datos indicados en la ventana de la herramienta.
        /// </summary>
        /// <param name="isRandomSeed"></param>
        private void CreateMap(bool isRandomSeed = false)
        {
            if (algorithmSelected != null)
            {
                var parameters = algorithmSelected.GetAlgorithmParameters();
                
                foreach (var kvp in parameters)
                {
                    Debug.Log($"{kvp.Key}: {kvp.Value}");
                }
                
                map = algorithmSelected.Build();

                AlgorithmData asset = GenerateAsset(map, algorithmSelected, tileBaseField.value as TileBase, tilemapField.value as Tilemap, seedField.value, isRandomSeed);
                
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
     
        /// <summary>
        /// Funcion encargada de poder construir el GameObject necesario y settearlo en una ruta en concreto.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="algorithmSelected"></param>
        /// <param name="tileBase"></param>
        /// <param name="tilemap"></param>
        /// <param name="seed"></param>
        /// <param name="randomSeed"></param>
        /// <returns></returns>
        private AlgorithmData GenerateAsset(int[,] map, IAlgorithm algorithmSelected, TileBase tileBase, Tilemap tilemap, float seed, bool randomSeed = false)
        {
            AlgorithmData asset = ScriptableObject.CreateInstance<AlgorithmData>();
            asset.width = map.GetLength(0);
            asset.height = map.GetLength(1);
            asset.cells = new int[asset.width * asset.height];
            asset.algorithmName = algorithmSelected.Name.ToString();

            for (int x = 0; x < asset.width; x++)
            for (int y = 0; y < asset.height; y++)
                asset[x, y] = map[x, y];

            // IMPORTANTE -> Modificar manualmente la ruta a la que se desee. No funcionara si no existe la carpeta 'GeneratedMaps'
            const string path = "Assets/GeneratedMaps/proceduralMap.asset";
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // IMPORTANTE -> Se puede modificar el nombre del objeto generado tanto aqui, como en el inspector manualmente tras generarlo.
            var go = new GameObject("Generator");
            var gen = go.AddComponent<MapGenerator>();
            
            gen.mapData = asset;
            gen.tileBase = tileBase;
            gen.tilemap = tilemap;
            gen.seed = seed;
            gen.randomSeed = randomSeed;

            return asset;
        }
    }
}
