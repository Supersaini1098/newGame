using System;
using UnityEngine;

namespace NeoFPS
{
    [HelpURL("https://docs.neofps.com/manual/hudref-mb-hudhider.html")]
    public class HudHider : MonoBehaviour
    {
        private static HudHider s_Instance = null;

        private bool m_CameraHide = false;
        private bool m_ManualHide = false;

        protected void Awake()
        {
            s_Instance = this;

            FirstPersonCameraBase.onCurrentCameraChanged += OnCurrentCameraChanged;
            OnCurrentCameraChanged(FirstPersonCameraBase.current);
        }

        protected void OnDestroy()
        {
            FirstPersonCameraBase.onCurrentCameraChanged -= OnCurrentCameraChanged;

            if (s_Instance == this)
                s_Instance = null;
        }

        private void OnCurrentCameraChanged(FirstPersonCameraBase cam)
        {
            m_CameraHide = (cam == null);
            gameObject.SetActive(!m_CameraHide && !m_ManualHide);
        }

        public static void HideHUD()
        {
            if (s_Instance != null)
            {
                s_Instance.m_ManualHide = true;
                s_Instance.gameObject.SetActive(!s_Instance.m_CameraHide && !s_Instance.m_ManualHide);
            }
        }

        public static void ShowHUD()
        {
            if (s_Instance != null)
            {
                s_Instance.m_ManualHide = false;
                s_Instance.gameObject.SetActive(!s_Instance.m_CameraHide && !s_Instance.m_ManualHide);
            }
        }
    }
}
