using System;
using UnityEngine;

namespace Systems.Movable
{
    public class MovableComponent : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1;

        private Vector3? _target = null;

        public bool IsMoving => _target != null;

        void Update()
        {
            if (_target != null)
            {
                float step = speed*Time.deltaTime;
                if (Vector3.Distance(transform.position, _target.Value) <= (step + float.Epsilon))
                {
                    transform.position = _target.Value;
                    _target = null;
                }
                else
                {

                    transform.position = Vector3.MoveTowards(transform.position, _target.Value, step);
                }
            }
        }

        public void MoveToTarget(Vector3 target)
        {
            _target = target;
        }
        
        public void MoveToDirection(Vector3 direction)
        {
            var position = transform.position;
            _target = new Vector3(position.x + direction.x * speed, position.y + direction.y * speed, 0);
        }
    }
}
