#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using NeoFPS.CharacterMotion;
using NeoFPS.CharacterMotion.Behaviours;
using NeoFPS.CharacterMotion.Parameters;

namespace NeoFPSEditor.CharacterMotion.Behaviours
{
    [MotionGraphBehaviourEditor(typeof(SetAnimatorIntBehaviour))]
    public class SetAnimatorIntBehaviourEditor : MotionGraphBehaviourEditor
    {
        protected override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ParameterName"));

            var whenProp = serializedObject.FindProperty("m_When");
            EditorGUILayout.PropertyField(whenProp);

            switch (whenProp.enumValueIndex)
            {
                case 0:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OnEnterValue"));
                    break;
                case 1:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OnExitValue"));
                    break;
                case 2:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OnEnterValue"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OnExitValue"));
                    break;
            }
        }
    }
}

#endif
