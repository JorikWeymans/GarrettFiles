//Created by Jorik Weymans 2021

using Garrett.UI;
using Garrett.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Garrett
{
    [DisallowMultipleComponent]
    public sealed class MasterCube : MonoBehaviour
    {
        [SerializeField][ReadOnly] private GameObject[] _CubePrefabs = null;

        [SerializeField][ReadOnly] private GameObject _GOCube = null;
        [SerializeField] private int _CubeToSpawn = 0;
        [SerializeField] private UnityEvent _OnSideMoved = default;
        [SerializeField] private float RotationThreshold = 1.0f;
        public CubeHandler CubeHandler { get; private set; } = null;
        public CubeRotationHandler CubeRotationHandler { get; private set; } = null;
        public CubeRotationInput CubeRotationInput { get; private set; } = null;


        private void Awake()
        {
            if (_GOCube == null)
            {
                _GOCube = Instantiate(_CubePrefabs[GamePanel.LvlToLoad > 0 ? GamePanel.LvlToLoad : _CubeToSpawn], transform);
            }

            InitCube();
        }


        public void OnModeChanged(PanModeManager.Mode newMode)
        {
            CubeRotationHandler.ResetRotation(newMode);

        }

        public void SpawnObject(int index = -1)
        {
            if (_GOCube != null)
            {
                DestroyImmediate(_GOCube);
            }

            _GOCube = Instantiate(_CubePrefabs[ index == -1 ? _CubeToSpawn : index], transform);
            InitCube();
        }

        private void InitCube()
        {
            CubeHandler = _GOCube.GetComponent<CubeHandler>();
            CubeRotationHandler = _GOCube.GetComponent<CubeRotationHandler>();
            CubeRotationInput = _GOCube.GetComponent<CubeRotationInput>();
            CubeHandler.OnSideMoved = _OnSideMoved;
            CubeHandler.RotationThreshold = RotationThreshold;
        }

        public void Undo()
        {
            CubeHandler.Undo();
        }
        
        public void AddOnSideMovedListener(UnityAction action)
        {
            _OnSideMoved.AddListener(action);
            CubeHandler.OnSideMoved = _OnSideMoved;
        }

        public void RemoveOnSideMovedListener(UnityAction action)
        {
            _OnSideMoved.RemoveListener(action);
            CubeHandler.OnSideMoved = _OnSideMoved;
        }


    }
}
