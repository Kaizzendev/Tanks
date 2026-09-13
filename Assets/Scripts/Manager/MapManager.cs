using System;
using Map;
using UnityEngine;

namespace Manager
{
    public class MapManager: MonoBehaviour
    {
        
        /*
         * TODO: Navegar el mapa
         *
         * 1. Cambiar a la escena mapa X
         * 2. Por primera vez: generar mapa X
         * 3. Coger mapa generado X
         * 4. Colocar jugador en nodo
         * 5. Jugador elige camino
         * 6. Mover jugador al siguiente nodo
         * 7. Guardar estado del mapa
         * 8. Cargar nuevo escenario
         * 
         */
        
        
        public static MapManager Instance;

        public MapNode CurrentNode { get; private set; }

        public event Action<MapNode> OnNodeSelected;
        public event Action<MapNode> OnNodeReached;
        
        [SerializeField] private MapGenerator _mapGenerator;
        
        private bool _isMapGenerated;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void OnEnable()
        {
            
        }

        private void Start()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            if (_isMapGenerated)
            {
                return;
            }
            
            _mapGenerator.Generate();
            _isMapGenerated = true;

            CurrentNode = _mapGenerator.GetStartNode();
        }

        private void SelectNode(MapNode node)
        {
            if (!CanSelectNode(node))
            {
                return;
            }

            CurrentNode = node;
            
            OnNodeSelected?.Invoke(node);
            
        }

        private bool CanSelectNode(MapNode node)
        {
            if (CurrentNode == null)
            {
                return false;
            }

            return CurrentNode.Children.Contains(node);

        }
        
    }
}