//Created by Jorik Weymans 2021

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

namespace Garrett
{
	[CustomEditor(typeof(MasterCube))]
    [CanEditMultipleObjects]
	public sealed class MasterCubeEditor : Editor
    {
        private SerializedProperty _CubePrefabsProperty = null;
        private SerializedProperty _GOCubeProperty = null;
        private SerializedProperty _CubeToSpawnProperty = null;
        private SerializedProperty _OnSideMovedProperty = null;
        private SerializedProperty _RotationThresholdProperty = null;
        private string[] _Options = null;

        private void OnEnable()
        {

            _CubePrefabsProperty = serializedObject.FindProperty("_CubePrefabs");
            _GOCubeProperty = serializedObject.FindProperty("_GOCube");
            _CubeToSpawnProperty = serializedObject.FindProperty("_CubeToSpawn");
            _OnSideMovedProperty = serializedObject.FindProperty("_OnSideMoved");
            _RotationThresholdProperty = serializedObject.FindProperty("RotationThreshold");


            ReLoadCubes();
            
        }




        public override void OnInspectorGUI()
		{
            serializedObject.Update();


            if (_CubeToSpawnProperty.intValue >= _Options.Length)
            {
                _CubeToSpawnProperty.intValue = _Options.Length - 1;
            }

            EditorGUILayout.PropertyField(_OnSideMovedProperty);
            

            EditorGUILayout.BeginHorizontal();
            int newIndex = EditorGUILayout.Popup("Current Cube", _CubeToSpawnProperty.intValue, _Options, EditorStyles.popup);
            if (newIndex != _CubeToSpawnProperty.intValue)
            {
                //Ugly but at least it works now
                _CubeToSpawnProperty.intValue = newIndex;

                DestroyImmediate(_GOCubeProperty.objectReferenceValue);
                _GOCubeProperty.objectReferenceValue = null;

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                ((MasterCube)target).SpawnObject();

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(_RotationThresholdProperty);
            EditorGUILayout.Space(5.0f);

            //EditorGUILayout.PropertyField(_CubePrefabsProperty);

            if (GUILayout.Button("Refresh List"))
            {
                ReLoadCubes();
            }


            if (GUILayout.Button("re-spawn Cube"))
            {
                DestroyImmediate(_GOCubeProperty.objectReferenceValue);
                _GOCubeProperty.objectReferenceValue = null;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                ((MasterCube)target).SpawnObject();
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            if (GUILayout.Button("remove Cube"))
            {
                DestroyImmediate(_GOCubeProperty.objectReferenceValue);
                _GOCubeProperty.objectReferenceValue = null;

            }

            //EditorGUILayout.PropertyField(_OnSideMovedProperty);
            serializedObject.ApplyModifiedProperties();
		}



        private void ReLoadCubes()
        {

            //Loading in levels
            GameObject[] prefabs = Resources.LoadAll<GameObject>("Levels");

            _CubePrefabsProperty.arraySize = prefabs.Length;
            for (int i = 0 ; i < prefabs.Length; i++)
            {
                _CubePrefabsProperty.GetArrayElementAtIndex(i).objectReferenceValue = prefabs[i];
            }


            //Generating List
            List<string> str = new List<string>();
            for (int i = 0; i < _CubePrefabsProperty.arraySize; i++)
            {
                str.Add(_CubePrefabsProperty.GetArrayElementAtIndex(i).objectReferenceValue.name);
            }

            _Options = str.ToArray();
        }
	}
}