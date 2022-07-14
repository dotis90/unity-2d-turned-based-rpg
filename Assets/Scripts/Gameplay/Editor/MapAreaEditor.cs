using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapArea))]
public class MapAreaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        int totalChanceInGrass = serializedObject.FindProperty("totalChance").intValue;
        int totalChanceInWater = serializedObject.FindProperty("totalChanceWater").intValue;

        if (totalChanceInGrass != 100 && totalChanceInGrass != -1)
        {
            EditorGUILayout.HelpBox($"The total chance percentage of Pokemon in grass is { totalChanceInGrass }, not 100", MessageType.Error);
        }

        if (totalChanceInWater != 100 && totalChanceInWater != -1)
        {
            EditorGUILayout.HelpBox($"The total chance percentage of Pokemon in water is { totalChanceInWater }, not 100", MessageType.Error);
        }
    }
}
