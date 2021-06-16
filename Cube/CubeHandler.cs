//Created by Jorik Weymans 2021

using System.Collections.Generic;
using System.Linq;
using Garrett.UI;
using Garrett.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Garrett
{
    //Garrett
    [DisallowMultipleComponent]
    public sealed class CubeHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _Center = null;
        [SerializeField] internal LayerMask Mask = default;

        internal float RotationThreshold = 1.0f;
        internal List<CubeSide> Sides = new List<CubeSide>();
        internal List<CubeSide> CurrentAllSides = null; 
        internal CubeSide CurrentSide = null; 
        internal CubeSide Previous = null;


        internal List<Cubicle> Cubicles = new List<Cubicle>();
        public UnityEvent OnSideMoved { get; set; } = null;



        private CubeHandlerImplementation _CurrentHandler = null;
        private void Start()
        {
            var FoundCubicles = GetComponentsInChildren<Cubicle>();

            foreach(Cubicle cube in FoundCubicles)
            {
                Cubicles.Add(cube);
            }
            
            InitSides();

            InputHandler.AddMouseDownAction(OnMouseDown);
            InputHandler.AddMouseUpAction(OnMouseUp);
            InputHandler.AddDeltaMousePositionAction(OnDeltaMousePosition);
            InputHandler.AddMouseHoldAction(OnMouseHold);

            _CurrentHandler = new FinalCubeImplementation(this);
        }

        private void OnDestroy()
        {
            InputHandler.RemoveMouseDownAction(OnMouseDown);
            InputHandler.RemoveMouseUpAction(OnMouseUp);
            InputHandler.RemoveDeltaMousePositionAction(OnDeltaMousePosition);
            InputHandler.RemoveMouseHoldAction(OnMouseHold);
        }

        internal void Undo()
        {
            if (Previous == null) return;

            Previous.Undo(transform);

            Cubicles.ForEach(cubicle =>
            {
                cubicle.UpdateIndices();
                cubicle.ContainsPlayer = false;
            });
            Sides.ForEach(cubeSide => cubeSide.UpdateCubicles(ref Cubicles));
        }


        // ** Delegating mouse functions
        private void OnMouseDown(InputAction.CallbackContext context)
        {
            _CurrentHandler.OnMouseDown(context);
        }
        private void OnDeltaMousePosition(InputAction.CallbackContext context)
        {
            _CurrentHandler.OnDeltaMousePosition(context);
        }
        private void OnMouseUp(InputAction.CallbackContext context)
        {
            _CurrentHandler.OnMouseUp(context);
        }
        private void OnMouseHold(InputAction.CallbackContext context)
        {
            _CurrentHandler.OnMouseHold(context);
        }

        private void OnDrawGizmosSelected()
        {
            _CurrentHandler?.OnDrawGizmosSelected();
        }
        // *
        private void InitSides()
        {
            CreateAxisSides(Axis.xAxis);
            CreateAxisSides(Axis.yAxis);
            CreateAxisSides(Axis.zAxis);

            Sides.ForEach(cubeSide => cubeSide.UpdateCubicles( ref Cubicles));

        }

        private void CreateAxisSides(Axis axis)
        {
            for(int i = -1; i <= 1; i++)
                Sides.Add(new CubeSide(i, axis, _Center));
        }
    }
}
