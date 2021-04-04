using System;
using Systems.Movable;
using UnityEngine;

namespace Systems.Utils
{
    public class TeatAI1 : MonoBehaviour
    {
        [SerializeField] private Vector2[] positions;
        
        [SerializeField] private MovableComponent movableComponent;

        private int _currentPositionIndex = 0;

        private void Start()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                var position = transform.position;
                positions[i].x += position.x;
                positions[i].y += position.y;
            }

            movableComponent.MoveToTarget(positions[_currentPositionIndex]);
        }

        void Update()
        {
            if (!movableComponent.IsMoving)
            {
                _currentPositionIndex = (_currentPositionIndex + 1) % positions.Length;
                movableComponent.MoveToTarget(positions[_currentPositionIndex]);
            }
        }
    }
}