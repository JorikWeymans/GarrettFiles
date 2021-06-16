//Created by Jorik Weymans 2021

using System;
using System.Collections;
using Garrett.UI;
using UnityEngine;
using UnityEngine.Events;
using Garrett.Utilities;
namespace Garrett
{
    [DisallowMultipleComponent]
    public sealed class CubeRotationHandler : MonoBehaviour
    {
        //[SerializeField][Range(1,10)] private float _RotationSpeed = 10.0f;
        [SerializeField][ReadOnly] private bool _IsRotating = false;
        public bool IsRotating => _IsRotating;

        [SerializeField] private UnityEvent _OnStartRotation = default;
        [SerializeField] private UnityEvent _OnEndRotation = default;
        [SerializeField][Range(1,10)] private float _Speed = 1.0f;

        private Vector3 _RotationAxis = Vector3.zero;

        private bool _HasRotated = false;
        public void StartTurningLeft()
        {
            if (_IsRotating) return;

            _RotationAxis = Vector3.up;
            StartCoroutine(Rotate());
        }
        public void StartTurningRight()
        {
            if (_IsRotating) return;

            _RotationAxis = Vector3.down;
            StartCoroutine(Rotate());

        }
        public void StartTurningUp()
        {
            if(_IsRotating) return;

            _RotationAxis = Vector3.right;
            StartCoroutine(Rotate());


        }
        public void StartTurningDown()
        {
            if (_IsRotating) return;

            _RotationAxis = Vector3.left;
            StartCoroutine(Rotate());

        }
        
        private IEnumerator Rotate()
        {
            int rot = 0;
            _OnStartRotation?.Invoke();
            _IsRotating = true;
            _HasRotated = true;
            int rotAmount = 5;
            while (rot < 90)
            {
                rot += rotAmount;
                transform.Rotate(_RotationAxis * rotAmount, Space.World);
                yield return new WaitForFixedUpdate();
            }

            _OnEndRotation?.Invoke();
            _IsRotating = false;
            yield return null;
        }


        public void ResetRotation(PanModeManager.Mode newMode)
        {

            switch (newMode)
            {
                case PanModeManager.Mode.Normal:
                    StopAllCoroutines();
                    if (_HasRotated)
                    {
                        StartCoroutine(FinalRotation());
                    }
                    break;
                case PanModeManager.Mode.Pan:
                    StopAllCoroutines();
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newMode), newMode, null);
            }

            _HasRotated = false;
        }


        private IEnumerator FinalRotation()
        {
            Vector3 startRotation = transform.rotation.eulerAngles;
            Vector3 finalRotation = Vector3.zero;
            float time = 0.0f;
            _IsRotating = true;


            bool panModeInput = PanModeManager.GetInstance().IsInputEnabled;
            GameManager.GetInstance().IsCubeInputEnabled = false;
            PanModeManager.GetInstance().IsInputEnabled = false;
            while (time < 1.0f)
            {
                time += Time.deltaTime * _Speed;
                Vector3 thisRotation = Vector3.Lerp(startRotation, finalRotation, time);
                transform.rotation = Quaternion.Euler(thisRotation);
                yield return null;
            }

            GameManager.GetInstance().IsCubeInputEnabled = true;
            PanModeManager.GetInstance().IsInputEnabled = panModeInput;

            transform.rotation = Quaternion.Euler(finalRotation);
            _IsRotating = false;
            _OnEndRotation?.Invoke();

            _IsRotating = false;
        }

    }
}
