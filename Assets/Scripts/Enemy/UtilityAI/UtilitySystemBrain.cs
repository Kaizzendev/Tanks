using System;
using System.Collections.Generic;
using Enemy.UtilityAI.Actions;
using UnityEngine;

namespace Enemy.UtilityAI
{
    public class UtilitySystemBrain: MonoBehaviour
    {
        [SerializeField] private List<UtilitySystemAction> _actions;
        private AIContext _context = new AIContext();

        private float score = 0f;
        
        private Dictionary<string, float> _actionScore = new Dictionary<string, float>();

        private float _thinkTimer;
        [SerializeField] private float _thinkRatio = 5f;


        private EnemyController _enemyController;
        private EnemyStats _enemyStats;
        
        private UtilitySystemAction bestAction = null;
        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
            _enemyStats = GetComponent<EnemyStats>();
        }

        public void Tick(float deltaTime)
        {
            _thinkTimer += deltaTime;
            if (_thinkTimer >= _thinkRatio)
            {
                Execute();
                _thinkTimer = 0f;
            }
        }

        private void Update()
        {
            Tick(Time.deltaTime);

            if (bestAction != null)
            {
                ExecuteCurrentAction();
            }
            
        }

        private void ExecuteCurrentAction()
        {
            bestAction.ExecuteAction(_context);
        }

        public void Execute()
        {
            Sense();
            Act();
        }

        private void Sense()
        {
            _context.enemy = _enemyController;
            _context.currentHealth = _enemyStats.currentHealth;
            _context.distanceToPlayer = _enemyController.distance;
        }   

        private void Act()
        {
            float bestScore = 0f;
            
            foreach (var action in _actions)
            {
                score = action.EvaluateScore(_context);
                LoadDictionary(action._actionName, score);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestAction = action;
                }
            }
            Debug.Log($"La mejor accion es: {bestAction} con una puntuacion de {bestScore}");
        }

        private void LoadDictionary(string actionName, float score)
        {
            if (!_actionScore.ContainsKey(actionName))
            {
                _actionScore.Add(actionName, score);
            }
            else
            {
                _actionScore[actionName] = score;
            }
        }
    }
}