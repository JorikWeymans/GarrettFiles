//Created by Jorik Weymans 2021

using Garrett.UI;
using Garrett.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Garrett
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CubeRotationHandler))]
    public sealed class CubeRotationInput : MonoBehaviour
    {
        [SerializeField] private float _Margin = 0.9f;
        private CubeRotationHandler _RotationHandler = null;

        private Vector2 _StartPosition = Vector2.zero, _EndPosition = Vector2.zero;
        private void Start()
        {
            _RotationHandler = GetComponent<CubeRotationHandler>();
        }

        public void OnMouseDown(InputAction.CallbackContext context)
        {
            if (!GameManager.GetInstance().IsCubeInputEnabled) return;

            _EndPosition = InputHandler.MousePosition;
        }

        public void OnMouseUp(InputAction.CallbackContext context)
        {
            if (!GameManager.GetInstance().IsCubeInputEnabled) return;

            _StartPosition = InputHandler.MousePosition;

            Vector2 direction = (_EndPosition - _StartPosition).normalized;


            if (Vector2.Dot(Vector2.down, direction) > _Margin)
            {
                _RotationHandler.StartTurningUp();

            }
            else if (Vector2.Dot(Vector2.up, direction) > _Margin)
            {
                _RotationHandler.StartTurningDown();

            }
            else if (Vector2.Dot(Vector2.left, direction) > _Margin)
            {
                _RotationHandler.StartTurningRight();

            }
            else if (Vector2.Dot(Vector2.right, direction) > _Margin)
            {
                _RotationHandler.StartTurningLeft();

            }

        }

    }
}
