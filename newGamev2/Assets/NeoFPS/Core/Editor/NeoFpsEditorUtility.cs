﻿#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

namespace NeoFPSEditor
{
    public static class NeoFpsEditorUtility
    {
        public static bool CheckAnimatorKeyValid(Animator animator, string key, AnimatorControllerParameterType parameterType)
        {
            // Do nothing in play mode
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return true;

            // Empty keys are always fine
            if (string.IsNullOrEmpty(key))
                return true;

            // Check animator is set
            if (animator == false)
                return false;

            SerializedObject animatorSO = new SerializedObject(animator);
            var controller = animatorSO.FindProperty("m_Controller").objectReferenceValue as AnimatorController;
            if (controller == null)
                return false;

            // Check through parameters for correct name and type
            var parameters = controller.parameters;
            for (int i = 0; i < parameters.Length; ++i)
            {
                var p = parameters[i];
                if (p.type == parameterType && p.name == key)
                    return true;
            }

            // None found
            return false;
        }

        public static Texture2D GetColourTexture(Color c)
        {
            c = c.linear;
            var tex = new Texture2D(4, 4);
            for (int i = 0; i < 16; ++i)
            {
                int row = i / 4;
                int column = i - (row * 4);
                tex.SetPixel(row, column, c);
            }
            tex.Apply();
            tex.hideFlags = HideFlags.HideAndDontSave;
            return tex;
        }

        public static void ClampFloatProperty(SerializedProperty prop, float min, float max)
        {
            prop.floatValue = Mathf.Clamp(prop.floatValue, min, max);
        }

        public static void ClampMinFloatProperty(SerializedProperty prop, float min)
        {
            if (prop.floatValue < min)
                prop.floatValue = min;
        }

        public static void ClampMaxFloatProperty(SerializedProperty prop, float max)
        {
            if (prop.floatValue > max)
                prop.floatValue = max;
        }

        public static void ClampIntProperty(SerializedProperty prop, int min, int max)
        {
            prop.intValue = Mathf.Clamp(prop.intValue, min, max);
        }

        public static void ClampMinIntProperty(SerializedProperty prop, int min)
        {
            if (prop.intValue < min)
                prop.intValue = min;
        }

        public static void ClampMaxIntProperty(SerializedProperty prop, int max)
        {
            if (prop.intValue > max)
                prop.intValue = max;
        }

        public static string GetCurrentComponentName(GameObject gameObject, Type t, UnityEngine.Object current)
        {
            // Get relevant components
            s_Components.Clear();
            gameObject.GetComponents(t, s_Components);

            // Get info on the current component
            var currentComponent = current as Component;
            var currentType = currentComponent.GetType();

            // Get number of components of same type, and index
            int index = 0;
            int count = 0;
            for (int i = 0; i < s_Components.Count; ++i)
            {
                if (s_Components[i].GetType() == currentType)
                {
                    ++count;
                    if (s_Components[i] == currentComponent)
                        index = count;
                }
            }

            if (count <= 1)
                return currentType.Name;
            else
                return string.Format("{0} ({1})", currentType.Name, index);
        }

        static List<Component> s_Components = new List<Component>();

        public static T GetRelativeComponent<T>(GameObject sourceObject, GameObject targetObject, T component) where T : class
        {
            var cast = component as Component;
            if (cast != null)
            {
                var gameObject = GetRelativeGameObject(sourceObject, targetObject, cast.gameObject);
                if (gameObject != null)
                {
                    // Adapt for multiple components of type
                    return gameObject.GetComponent<T>();
                }
            }
            return null;
        }

        public static GameObject GetRelativeGameObject(GameObject sourceObject, GameObject targetObject, GameObject target)
        {
            // Check if null    
            if (target == null || sourceObject == null || targetObject == null)
                return null;

            // Check if root            
            if (sourceObject == target)
                return targetObject;

            // Index list
            List<int> indices = new List<int>();

            // Crawl from source object to source root
            var itr = target.transform;
            while (itr != sourceObject && itr != null && itr.parent != null)
            {
                //int index = GetSafeSiblingIndex(itr);
                int index = itr.GetSiblingIndex();
                //Debug.Log(string.Format("Object {0} is child index {1} of {2}", itr.name, index, itr.parent.name));
                indices.Add(index);
                itr = itr.parent;
            }

            //string printString = "indices: ";
            //for (int i = indices.Count - 1; i >= 0; --i)
            //    printString += indices[i] + " ";
            //Debug.Log(printString);

            // Reverse for destination
            itr = targetObject.transform;
            for (int i = indices.Count - 1; i >= 0; --i)
                itr = itr.GetChild(indices[i]);

            return itr.gameObject;
        }
    }
}

#endif
