using System;
using Map;
using Player;
using ProceduralGeneration;
using UnityEngine;
using UnityEngine.SceneManagement;

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
         * 4. Colocar jugador en nodo X
         * 5. Jugador elige camino X
         * 6. Mover jugador al siguiente nodo X
         * 7. Guardar estado del mapa X
         * 8. Cargar nuevo escenario
         * 
         */
        
        
        public static MapManager Instance;
        
        [Header("Player")] [SerializeField] private GameObject _playerPrefab;
        
        public MapNode CurrentNode { get; private set; }
        public MapNode TargetNode { get; private set; }

        public event Action<MapNode> OnNodeSelected;
        
        public event Action<MapNode> OnNodeReached;
        
        [SerializeField] private MapGenerator _mapGenerator;
        
        private bool _isMapGenerated;

        private GameObject _player;
        
        [SerializeField] private Vector3 _spawnPositionOffset = new Vector3(0,6,0);
        
        private bool isNodeSelected;

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

            GeneratePlayerInSpawn(CurrentNode.Position);
        }
        
        private void GeneratePlayerInSpawn(Vector3 spawnPoint)
        {
            _player = Instantiate(_playerPrefab, spawnPoint + _spawnPositionOffset, Quaternion.identity);
            
            
            MapNavigationController mapNavigationController = _player.GetComponent<MapNavigationController>();

            mapNavigationController.OnNodeReached += HandleNodeReached;
        }

        private void HandleNodeReached(MapNode node)
        {
            CurrentNode = node;
            isNodeSelected = false;
            
            OnNodeReached?.Invoke(node);
        }

        internal void SelectNode(MapNode node)
        {
            if (!CanSelectNode(node))
            {
                return;
            }

            if (isNodeSelected)
            {
                return;
            }

            TargetNode = node;
            
            OnNodeSelected?.Invoke(node);
            
            isNodeSelected = true;
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