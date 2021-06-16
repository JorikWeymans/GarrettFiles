//Created by Jorik Weymans 2021

using Garrett.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Garrett
{
    public abstract class CubeHandlerImplementation
    {
        protected RaycastHit _Hit;
        protected  bool _IsMouseDown = false;
        protected CubeHandler _Owner = null;

        protected CubeHandlerImplementation(CubeHandler handler)
        {
            _Owner = handler;
        }
        public void OnMouseDown(InputAction.CallbackContext context)
        {
            if (!GameManager.GetInstance().IsCubeInputEnabled) return;
            if (PanModeManager.GetInstance().TheMode == PanModeManager.Mode.Pan) return;

            InternalOnMouseDown(context);

        }
        public void OnDeltaMousePosition(InputAction.CallbackContext context)
        {
            if (!GameManager.GetInstance().IsCubeInputEnabled) return;

            InternalOnDeltaMousePosition(context);

        }

        public void OnMouseHold(InputAction.CallbackContext context)
        {
            if (!GameManager.GetInstance().IsCubeInputEnabled) return;
            
            InternalOnMouseHold(context);
        }
        public void OnMouseUp(InputAction.CallbackContext context)
        {
            if (!GameManager.GetInstance().IsCubeInputEnabled) return;
            if (PanModeManager.GetInstance().TheMode == PanModeManager.Mode.Pan) return;

            InternalOnMouseUp(context);
        }

        public virtual void OnDrawGizmosSelected()
        {

        }

        protected bool DoRayCast()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputHandler.MousePosition);

            if (!Physics.Raycast(ray, out _Hit, 200.0f, _Owner.Mask)) return false;

            GameManager.GetInstance().Player.CheckForCubicle();
            return true;

        }


        protected abstract void InternalOnMouseDown(InputAction.CallbackContext context);
        protected abstract void InternalOnDeltaMousePosition(InputAction.CallbackContext context);
        protected abstract void InternalOnMouseUp(InputAction.CallbackContext context);
        protected abstract void InternalOnMouseHold(InputAction.CallbackContext context);
        




    }
}
