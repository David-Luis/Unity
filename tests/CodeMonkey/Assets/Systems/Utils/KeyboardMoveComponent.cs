using Systems.Movable;
using UnityEngine;

namespace Systems.Utils
{
    public class KeyboardMoveComponent : MonoBehaviour
    {
        [SerializeField] private MovableComponent movableComponent;

        // Update is called once per frame
        void Update()
        {
            var inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            inputAxis.Normalize();
            movableComponent.MoveToDirection(inputAxis);
        }
    }
}
