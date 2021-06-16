//Created by Jorik Weymans 2021

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Garrett.Utilities;
using UnityEngine.EventSystems;

namespace Garrett
{
    public sealed class CubeSide
    {
        private List<Cubicle> _Cubicles = null;

        private readonly GameObject _Center = null;
        private readonly int _SideIndex = 0;
        public Axis Axis { get; } = default;

        public bool ContainsPlayer
        {
            get
            {
                Cubicle found = _Cubicles.FirstOrDefault(cubicle => cubicle.ContainsPlayer);
                return found != null;
            }
        }

        public bool IsHighlighted
        {
            get
            {
                bool isHighlighted = true;
                _Cubicles.ForEach(cubicle =>
                {
                    if (!cubicle.IsHighlighted)
                        isHighlighted = false;
                });
                
                return isHighlighted;

            }
        }

        public void Highlight(bool val)
        {
            _Cubicles.ForEach(cube => cube.Highlight(val));
        }
        // ******
        // * Internal constructor
        // ******
        internal CubeSide(int index, Axis direction, GameObject center)
        {
            _SideIndex = index;
            Axis = direction;
            _Center = center;
        }

        // ******
        // * Internal functions
        // ******
        internal void UpdateCubicles(ref List<Cubicle> cubicles)
        {
            _Cubicles?.Clear();
            _Cubicles = new List<Cubicle>();
            switch (Axis)
            {
                case Axis.xAxis:
                    cubicles.ForEach(cubicle =>
                    {
                        CheckAndAddCubicle(cubicle, cubicle.Index.x);
                    });

                break;
                case Axis.yAxis:
                    cubicles.ForEach(cubicle =>
                    {
                        CheckAndAddCubicle(cubicle, cubicle.Index.y);
                    });
                break;
                case Axis.zAxis:
                    cubicles.ForEach(cubicle =>
                    {
                        CheckAndAddCubicle(cubicle, cubicle.Index.z);
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private float _RotationAmount = 0.0f;
        internal void OnStartRotation()
        {
            _Cubicles.ForEach(o =>
            {
                if (o != _Center)
                {
                    o.transform.parent = _Center.transform;
                }
            });
            _RotationAmount = 0.0f;
        }
         
        internal void Rotate(Vector2 deltaRotation)
        {
            switch (Axis)
            {
                case Axis.xAxis:
                    _Center.transform.Rotate(Vector3.right, deltaRotation.y * 0.1f);
                    _RotationAmount += deltaRotation.y * 0.1f;
                    break;
                case Axis.yAxis:
                    _Center.transform.Rotate(Vector3.up, deltaRotation.x * -0.1f);
                    _RotationAmount += deltaRotation.x * -0.1f;
                    break;
                case Axis.zAxis:
                    _Center.transform.Rotate(Vector3.forward, deltaRotation.y * 0.1f);
                    _RotationAmount += deltaRotation.x * -0.1f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //Debug.Log(_RotationAmount);

        }
        //Returns true when a rotation is done, false when it snaps to the starting rotation
        internal bool OnStopRotation(Transform parent)
        {
            Vector3 rotation = _Center.transform.localRotation.eulerAngles;

            //Snapping x
            if (rotation.x >= 0 && rotation.x < 90)
            {
                rotation.x = rotation.x < 45 ? 0 : 90;
            }
            else if (rotation.x >= 90 && rotation.x < 180)
            {
                rotation.x = rotation.x < 135 ? 90 : 180;
            }
            else if (rotation.x >= 180 && rotation.x < 270)
            {
                rotation.x = rotation.x < 255 ? 180 : 270;
            }
            else if (rotation.x >= 270 && rotation.x < 360)
            {
                rotation.x = rotation.x < 315 ? 270 : 360;
            }

            //Snapping Y
            if (rotation.y >= 0 && rotation.y < 90)
            {
                rotation.y = rotation.y < 45 ? 0 : 90;
            }
            else if (rotation.y >= 90 && rotation.y < 180)
            {
                rotation.y = rotation.y < 135 ? 90 : 180;
            }
            else if (rotation.y >= 180 && rotation.y < 270)
            {
                rotation.y = rotation.y < 255 ? 180 : 270;
            }
            else if (rotation.y >= 270 && rotation.y < 360)
            {
                rotation.y = rotation.y < 315 ? 270 : 360;
            }


            //Snapping z
            if (rotation.z >= 0 && rotation.z < 90)
            {
                rotation.z = rotation.z < 45 ? 0 : 90;
            }
            else if (rotation.z >= 90 && rotation.z < 180)
            {
                rotation.z = rotation.z < 135 ? 90 : 180;
            }
            else if (rotation.z >= 180 && rotation.z < 270)
            {
                rotation.z = rotation.z < 255 ? 180 : 270;
            }
            else if (rotation.z >= 270 && rotation.z < 360)
            {
                rotation.z = rotation.z < 315 ? 270 : 360;
            }



            _Center.transform.localRotation = Quaternion.Euler(rotation);

            _Cubicles.ForEach(o =>
            {
                o.transform.parent = parent;
            });

            _Center.transform.rotation = Quaternion.identity;

            return Mathf.Abs(_RotationAmount) > 45.0f;
        }

        internal bool Contains(Cubicle cubicle)
        {
            return _Cubicles.Contains(cubicle);
        }
        internal bool Contains(GameObject obj)
        {
            Cubicle cubicle = obj.GetComponent<Cubicle>();
            return cubicle != null && _Cubicles.Contains(cubicle);
        }

        internal void Undo(Transform parent)
        {
            if (Mathf.Abs(_RotationAmount) < 45) return;

            //Parenting everything
            _Cubicles.ForEach(o =>
            {
                if (o != _Center)
                {
                    o.transform.parent = _Center.transform;
                }
            });

            float clampValue;
            float absRotation = Mathf.Abs(_RotationAmount);
            if (absRotation >= 45 && absRotation < 135)
                clampValue = 90.0f;
            else if(absRotation >= 135 && absRotation < 225)
                clampValue = 180.0f;
            else if (absRotation >= 225 && absRotation < 315)
                clampValue = 270.0f;
            else
                clampValue = 360.0f;



            switch (Axis)
            {
                case Axis.xAxis:
                    _Center.transform.Rotate(Vector3.right, (_RotationAmount > 0.0f) ? -clampValue : clampValue);
                    break;
                case Axis.yAxis:
                    _Center.transform.Rotate(Vector3.up, (_RotationAmount > 0.0f) ? -clampValue : clampValue);
                    break;
                case Axis.zAxis:
                    _Center.transform.Rotate(Vector3.forward,  (_RotationAmount > 0.0f)? -clampValue : clampValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            _Cubicles.ForEach(o =>
            {
                o.transform.parent = parent;
            });

            _Center.transform.rotation = Quaternion.identity;
            
            _RotationAmount = 0.0f;



        }
        // ******
        // * Private functions
        // ******
        private void CheckAndAddCubicle(Cubicle cubicle, int x)
        {
            if (x != _SideIndex) return;
            _Cubicles.Add(cubicle);

        }

 

    }
}
