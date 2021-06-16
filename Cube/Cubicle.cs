//Created by Jorik Weymans 2021

using UnityEngine;
using Garrett.Utilities;
using UnityEngine.Events;

namespace Garrett
{
    public sealed class Cubicle : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private Vector3Int _Index = Vector3Int.zero;
        [SerializeField] private GameObject _GOHighlight = null;
        public bool ContainsPlayer { get; set; } = false;

        [SerializeField] private UnityEvent<Vector3Int> _OnIndexChanged = default;
        public Vector3Int Index
        {
            get => _Index;
            //set => _Index = value;
        }

        public bool IsHighlighted => _GOHighlight.activeSelf;
        private void Awake()
        {
            UpdateIndices();
        }

        public void UpdateIndices()
        {
            Vector3 pos = transform.position;
            _Index.x = (int)Mathf.Clamp(pos.x, -1, 1);
            _Index.y = (int)Mathf.Clamp(pos.y, -1, 1);
            _Index.z = (int)Mathf.Clamp(pos.z, -1, 1);

            gameObject.name = _Index.String();

            _OnIndexChanged?.Invoke(_Index);
        }

        public void Highlight(bool val)
        {
            _GOHighlight.SetActive(val);
        }

        public void AddOnIndexChangedListener(UnityAction<Vector3Int> action)
        {
            _OnIndexChanged.AddListener(action);
        }
        public void RemoveOnIndexChangedListener(UnityAction<Vector3Int> action)
        {
            _OnIndexChanged.RemoveListener(action);
        }

    }
}
