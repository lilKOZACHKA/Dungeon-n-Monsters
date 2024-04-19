using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Scripts.UnitLogic
{
    [CustomEditor(typeof(Unit))]
    public class UnitEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var unit = target as Unit;

            DrawDefaultInspector();

            if (GUILayout.Button("Initialize"))
            {
                unit.FindCell();
            }
        }

        [MenuItem("Tools/Initialize units")]
        private static void InitializeUnits()
        {
            foreach (Unit unit in FindObjectsOfType<Unit>())
            {
                unit.FindCell();

                EditorUtility.SetDirty(unit);
            }

            AssetDatabase.SaveAssets();
        }
    }
}