﻿using System.Collections.Generic;
using UnityEngine;
using NeoFPS.SinglePlayer;
using System.Collections;

namespace NeoFPS
{
    public class SoloPlayerCharacterEventWatcher : MonoBehaviour, IPlayerCharacterWatcher
    {
        private List<IPlayerCharacterSubscriber> m_Subsribers = new List<IPlayerCharacterSubscriber>(4);
        private FpsSoloCharacter m_CurrentCharacter = null;
        private bool m_Initialised = false;

        IEnumerator Start()
        {
            yield return null;
            FpsSoloCharacter.onLocalPlayerCharacterChange += OnLocalPlayerCharacterChange;
            OnLocalPlayerCharacterChange(FpsSoloCharacter.localPlayerCharacter);
            m_Initialised = true;
        }

        void OnEnable()
        {
            // Check if initialised since we don't want this to happen before the Start
            if (m_Initialised)
            {
                FpsSoloCharacter.onLocalPlayerCharacterChange += OnLocalPlayerCharacterChange;
                OnLocalPlayerCharacterChange(FpsSoloCharacter.localPlayerCharacter);
            }
        }

        void OnDisable()
        {
            if (m_Initialised)
                FpsSoloCharacter.onLocalPlayerCharacterChange -= OnLocalPlayerCharacterChange;
        }

        protected void OnDestroy()
        {
            FpsSoloCharacter.onLocalPlayerCharacterChange -= OnLocalPlayerCharacterChange;
        }

        public void AttachSubscriber(IPlayerCharacterSubscriber subscriber)
        {
            if (subscriber == null)
                return;

            if (!m_Subsribers.Contains(subscriber))
            {
                m_Subsribers.Add(subscriber);
                subscriber.OnPlayerCharacterChanged(m_CurrentCharacter);
            }
            else
                Debug.LogError("Attempting to attach a player character subscriber that is already attached.");
        }

        public void ReleaseSubscriber(IPlayerCharacterSubscriber subscriber)
        {
            if (subscriber == null)
                return;

            if (m_Subsribers.Contains(subscriber))
                m_Subsribers.Remove(subscriber);
            //else
            //    Debug.LogError("Attempting to remove a player character subscriber that was not attached.");
        }

        void OnLocalPlayerCharacterChange(FpsSoloCharacter character)
        {
            m_CurrentCharacter = character;
            for (int i = 0; i < m_Subsribers.Count; ++i)
                m_Subsribers[i].OnPlayerCharacterChanged(m_CurrentCharacter);
        }
    }
}
