using UnityEditor;
using UnityEngine;

namespace Scripts.GameBoardLogic
{
    [CustomEditor(typeof(GameBoardGrid))]
    public class GameBoardGridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var grid = target as GameBoardGrid;

            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Create"))
            {
                grid.Create();
            }

            if(GUILayout.Button("Clear"))
            {
                grid.Clear();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}



