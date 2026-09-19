using System;
using System.Collections.Generic;
using Map;
using Player;
using ProceduralGeneration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class MapManager: MonoBehaviour
    {
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

        private List<List<MapNode>> _nodesPerLayer =  new List<List<MapNode>>();
        
        private void OnEnable()
        {
            GameManager.Instance.OnStateChanged += OnGameStateChanged;
        }


        private void OnDisable()
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
        }

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
        
        private void OnGameStateChanged(GameManager.GameState state)
        {
            if (state == GameManager.GameState.MapNavigation)
            {
                GenerateMap();
            }
        }

        private void GenerateMap()
        {

            if (_mapGenerator == null)
            {
                _mapGenerator = FindObjectOfType<MapGenerator>();
            }
            
            
            if (!_isMapGenerated)
            {
                _nodesPerLayer = _mapGenerator.Generate();
                CurrentNode = _mapGenerator.GetStartNode();
            }
            else
            {
                _mapGenerator.InstantiateNodes(_nodesPerLayer);
            }
                
            
            GeneratePlayerInNode(CurrentNode.Position);
            _isMapGenerated = true;
        }
        
        private void GeneratePlayerInNode(Vector3 spawnPoint)
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