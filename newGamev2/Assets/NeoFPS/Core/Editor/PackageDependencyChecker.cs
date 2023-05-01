#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;

namespace NeoFPSEditor
{
    public class PackageDependencyChecker : ScriptableObject
    {
        private const int k_TargetVersion = 1;

        private const string k_JsonPackages = "NeoFpsPackages";

        private static PackageDependencyChecker s_Instance = null;

        public static void CheckPackages(Action onComplete)
        {
            if (NeoFpsEditorPrefs.currentPackageDependenciesVersion < k_TargetVersion)
            {
                // Load settings
                s_Instance = CreateInstance<PackageDependencyChecker>();
                if (!s_Instance.LoadFromJsonAsset(k_JsonPackages))
                {
                    // Destroy
                    DestroyImmediate(s_Instance);
                    s_Instance = null;

                    // Signal completion
                    if (onComplete != null)
                        onComplete();

                    return;
                }

                // Perform check
                s_Instance.CheckPackagesInternal(onComplete);

                // Update current version
                NeoFpsEditorPrefs.currentPackageDependenciesVersion = k_TargetVersion;
            }
            else
            {
                // Signal completion
                if (onComplete != null)
                    onComplete();
            }
        }

        public Package[] packages = new Package[0];

        [Serializable]
        public class Package
        {
            public string packageName = string.Empty;
            public string version = string.Empty;
        }

        private Action m_OnComplete = null;
        private AddRequest m_AddRequest = null;
        private Queue<Package> m_PackagesToAdd = null;

        public bool LoadFromJsonAsset(string assetName)
        {
            string[] guids = AssetDatabase.FindAssets(assetName);
            if (guids.Length == 0)
                return false;

            var json = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guids[0]));
            if (json == null)
                return false;

            JsonUtility.FromJsonOverwrite(json.text, this);

            return true;
        }

        void CheckPackagesInternal(Action onComplete)
        {
            // Store completion callback
            m_OnComplete = onComplete;

            if (packages != null && packages.Length > 0)
            {
                Debug.Log("Checking NeoFPS Package Dependencies");

                // Check installed packages
                m_PackagesToAdd = new Queue<Package>();
                for (int i = 0; i < packages.Length; ++i)
                {
                    if (!IsPackageInstalled(packages[i]))
                        m_PackagesToAdd.Enqueue(packages[i]);
                }

                // Start install queue if any required
                if (m_PackagesToAdd.Count > 0)
                {
                    // Wait for check to complete
                    EditorApplication.update += InstallPackageFromQueue;
                }
                else
                    OnCompleted();

            }
            else
                OnCompleted();
        }

        void InstallPackageFromQueue()
        {
            // Unsubscribe from event
            EditorApplication.update -= InstallPackageFromQueue;

            // Get the package
            var package = m_PackagesToAdd.Dequeue();

            // Package not installed. Send add request
            if (package.version == string.Empty)
                m_AddRequest = Client.Add(package.packageName);
            else
                m_AddRequest = Client.Add(package.packageName + "@" + package.version);

            // Subscribe to install event
            EditorApplication.update += WaitForInstall;
        }

        void WaitForInstall()
        {
            if (m_AddRequest.Status == StatusCode.InProgress)
                return;

            // Unsubscribe from event
            EditorApplication.update -= WaitForInstall;

            // Subscribe to queue event or complete
            if (m_PackagesToAdd != null && m_PackagesToAdd.Count > 0)
                EditorApplication.update += InstallPackageFromQueue;
            else
                OnCompleted();
        }

        void OnCompleted()
        {
            // Signal completion
            if (m_OnComplete != null)
                m_OnComplete();

            DestroyImmediate(s_Instance);
            s_Instance = null;
        }



        [Serializable]
        private struct PackageInfo
        {
            public string name;
            public string version;
        }

        public static bool IsPackageInstalled(Package package)
        {
            var path = "Packages/" + package.packageName + "/package.json";

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            // Check if package is installed at all
            if (asset == null)
                return false;

            // Check if specific version required
            if (string.IsNullOrWhiteSpace(package.version))
                return true;

            // Check if specific version is installed
            var info = JsonUtility.FromJson<PackageInfo>(asset.text);

            if (info.name != package.packageName) return false;

            int dashIndex = info.version.IndexOf('-');

            if (dashIndex > -1)
            {
                info.version = info.version.Substring(0, dashIndex);
            }

            Version parsedVersion;
            Version targetVersion;

            try
            {
                parsedVersion = new Version(info.version);
                targetVersion = new Version(package.version);
            }
            catch
            {
                Debug.LogError(string.Format("Failed to parse package version strings (info:`{0}`, package:`{1}`) for package: {2}", info.version, package.version, package.packageName));
                return false;
            }

            return parsedVersion >= targetVersion;
        }
         
    }
}

#endif