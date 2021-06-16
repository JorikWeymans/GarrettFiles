//Created by Jorik Weymans 2021

using System;
using System.Linq;
using Garrett.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Garrett
{
    public sealed class FinalCubeImplementation : CubeHandlerImplementation
    {
        private enum PressedFace { None, Up, Right, Forward}
        public FinalCubeImplementation(CubeHandler handler) : base(handler) { }

        private PressedFace _PressedFace = PressedFace.None;
        protected override void InternalOnMouseDown(InputAction.CallbackContext context)
        {
            _Owner.CurrentAllSides?.ForEach(side => side.Highlight(false));
            if (!DoRayCast()) return;

            if (_Hit.collider == null) return;


            _Owner.CurrentAllSides = _Owner.Sides.FindAll(cubeSide => cubeSide.Contains(_Hit.collider.gameObject));

            Vector3 pos = _Hit.point;
            Vector3 normalizedPos = pos.normalized;

            if(Vector3.Dot(normalizedPos, Vector3.up) > 0.70)
            {
                _PressedFace = PressedFace.Up;
                SelectSideUnsafe(Axis.xAxis);
                SelectSideUnsafe(Axis.zAxis);
                _IsMouseDown = true;

            }
            else if(Vector3.Dot(normalizedPos, Vector3.right) > 0.70)
            {
                _PressedFace = PressedFace.Right;
                SelectSideUnsafe(Axis.zAxis);
                SelectSideUnsafe(Axis.yAxis);
                _IsMouseDown = true;
            }
            else
            {
                _PressedFace = PressedFace.Forward;
                SelectSideUnsafe(Axis.yAxis);
                SelectSideUnsafe(Axis.xAxis);
                _IsMouseDown = true;
            }
        }

        protected override void InternalOnDeltaMousePosition(InputAction.CallbackContext context)
        {
            if (!_IsMouseDown) return;
            if (!GameManager.GetInstance().IsCubeInputEnabled) return;

            Vector2 delta = context.ReadValue<Vector2>();
            if (_Owner.CurrentSide == null)
            {
                switch (_PressedFace)
                {
                    case PressedFace.None:
                        break;
                    case PressedFace.Up:
                        CheckMoveUp(delta);
                        break;
                    case PressedFace.Right:
                        CheckMoveRight(delta);
                        break;
                    case PressedFace.Forward:
                        CheckMoveForward(delta);
                        break;
                }
                
            }

            _Owner.CurrentSide?.Rotate(delta);

        }

        protected override void InternalOnMouseUp(InputAction.CallbackContext context)
        {
            _Owner.CurrentAllSides?.ForEach(side => side.Highlight(false));
            //throw new System.NotImplementedException();
            _IsMouseDown = false;
            bool hasRotated = false;
            if (_Owner.CurrentSide != null)
            {
                hasRotated = _Owner.CurrentSide.OnStopRotation(_Owner.transform);
            }

            _Owner.Cubicles.ForEach(cubicle =>
            {
                cubicle.UpdateIndices();
                cubicle.ContainsPlayer = false;
            });
            _Owner.Sides.ForEach(cubeSide => cubeSide.UpdateCubicles(ref _Owner.Cubicles));


            _Owner.CurrentAllSides = null;
            _Owner.Previous = _Owner.CurrentSide;
            _Owner.CurrentSide = null;

            if (hasRotated)
            {
                _Owner.OnSideMoved?.Invoke();
            }
        }

        protected override void InternalOnMouseHold(InputAction.CallbackContext context)
        {
        }

        public override void OnDrawGizmosSelected()
        {
            if (_Hit.collider == null) return;

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(_Hit.point, .1f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_Hit.collider.gameObject.transform.position, _Hit.point);
            
        }

        private void SelectSideUnsafe(Axis axis)
        {
            CubeSide found = _Owner.CurrentAllSides.FirstOrDefault(cubeSide => cubeSide.Axis == axis);
            if (found != null && !found.ContainsPlayer)
            {
                found.Highlight(true);
            }
        }

        private void CheckMoveForward(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > _Owner.RotationThreshold)
            {
                CubeSide found = _Owner.CurrentAllSides.FirstOrDefault(cubeSide => cubeSide.Axis == Axis.yAxis);

                if (found == null || !found.IsHighlighted) return;

                _Owner.CurrentAllSides.ForEach(cubeSide => cubeSide.Highlight(false));
                _Owner.CurrentSide = found;
                _Owner.CurrentSide.OnStartRotation();
                _Owner.CurrentSide.Highlight(true);
            }
            else if (Mathf.Abs(delta.y) > _Owner.RotationThreshold)
            {
                CubeSide found = _Owner.CurrentAllSides.FirstOrDefault(cubeSide => cubeSide.Axis == Axis.xAxis);

                if (found == null || !found.IsHighlighted) return;

                _Owner.CurrentAllSides.ForEach(cubeSide => cubeSide.Highlight(false));
                _Owner.CurrentSide = found;
                _Owner.CurrentSide.OnStartRotation();
                _Owner.CurrentSide.Highlight(true);
            }
        }
        private void CheckMoveRight(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > _Owner.RotationThreshold)
            {
                CubeSide found = _Owner.CurrentAllSides.FirstOrDefault(cubeSide => cubeSide.Axis == Axis.yAxis);

                if (found == null || !found.IsHighlighted) return;

                _Owner.CurrentAllSides.ForEach(cubeSide => cubeSide.Highlight(false));
                _Owner.CurrentSide = found;
                _Owner.CurrentSide.OnStartRotation();
                _Owner.CurrentSide.Highlight(true);
            }
            else if (Mathf.Abs(delta.y) > _Owner.RotationThreshold)
            {
                CubeSide found = _Owner.CurrentAllSides.FirstOrDefault(cubeSide => cubeSide.Axis == Axis.zAxis);

                if (found == null || !found.IsHighlighted) return;

                _Owner.CurrentAllSides.ForEach(cubeSide => cubeSide.Highlight(false));
                _Owner.CurrentSide = found;
                _Owner.CurrentSide.OnStartRotation();
                _Owner.CurrentSide.Highlight(true);
            }
        }
        private void CheckMoveUp(Vector2 delta)
        {
            if (Mathf.Abs(delta.y) > _Owner.RotationThreshold)
            {
                CubeSide found = _Owner.CurrentAllSides.FirstOrDefault(cubeSide => cubeSide.Axis == Axis.xAxis);

                if (found == null || !found.IsHighlighted) return;

                _Owner.CurrentAllSides.ForEach(cubeSide => cubeSide.Highlight(false));
                _Owner.CurrentSide = found;
                _Owner.CurrentSide.OnStartRotation();
                _Owner.CurrentSide.Highlight(true);
            }
            else if (Mathf.Abs(delta.x) > _Owner.RotationThreshold)
            {
                CubeSide found = _Owner.CurrentAllSides.FirstOrDefault(cubeSide => cubeSide.Axis == Axis.zAxis);

                if (found == null || !found.IsHighlighted) return;

                _Owner.CurrentAllSides.ForEach(cubeSide => cubeSide.Highlight(false));
                _Owner.CurrentSide = found;
                _Owner.CurrentSide.OnStartRotation();
                _Owner.CurrentSide.Highlight(true);
            }
        }
    }
}
