﻿#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using NeoFPS.CharacterMotion.Conditions;
using NeoFPS.CharacterMotion.Parameters;

namespace NeoFPSEditor.CharacterMotion.Conditions
{
    [MotionGraphConditionDrawer(typeof(CompareFloatsCondition))]
    [HelpURL("http://docs.neofps.com/manual/motiongraphref-mgc-comparefloatscondition.html")]
    public class CompareFloatsConditionDrawer : MotionGraphConditionDrawer
    {
        protected override void Inspect (Rect line1)
        {
            Rect r1 = line1;
            Rect r2 = r1;
            Rect r3 = r1;
            r1.width *= 0.4f;
            r2.width *= 0.2f;
            r3.width *= 0.4f;
            r2.x += r1.width;
            r3.x += r1.width + r2.width + 2f;
            r3.width -= 2f;
            r1.width -= 2f;

            MotionGraphEditorGUI.ParameterDropdownField<FloatParameter>(r1, graphRoot, serializedObject.FindProperty("m_PropertyA"));

            // Draw the comparison type dropdown
            var m_ComparisonTypeString = GetComparisonTypeString(serializedObject.FindProperty("m_Comparison").enumValueIndex);
            if (EditorGUI.DropdownButton(r2, new GUIContent(m_ComparisonTypeString), FocusType.Passive))
            {
                GenericMenu gm = new GenericMenu();
                for (int i = 0; i < 6; ++i)
                    gm.AddItem(new GUIContent(GetComparisonTypeString(i)), false, OnComparisonTypeDropdownSelect, i);
                gm.ShowAsContext();
            }

            MotionGraphEditorGUI.ParameterDropdownField<FloatParameter>(r3, graphRoot, serializedObject.FindProperty("m_PropertyB"));
        }

        void OnComparisonTypeDropdownSelect (object o)
        {
            int index = (int)o;
            serializedObject.FindProperty("m_Comparison").enumValueIndex = index;
            serializedObject.ApplyModifiedProperties();
        }

        string GetComparisonTypeString (int i)
        {
            switch (i)
            {
                case 0:
                    return "=";
                case 1:
                    return "!=";
                case 2:
                    return ">";
                case 3:
                    return ">=";
                case 4:
                    return "<";
                case 5:
                    return "<=";
            }
            return "=";
        }
    }
}

#endif
