using UnityEngine;
using NeoFPS.CharacterMotion;
using NeoFPS.CharacterMotion.Parameters;
using NeoSaveGames.Serialization;
using System.Collections;

namespace NeoFPS.CharacterMotion
{
    [MotionGraphElement("Animation/SetAnimatorInt", "SetAnimatorIntBehaviour")]
    public class SetAnimatorIntBehaviour : MotionGraphBehaviour
    {
        [SerializeField, Tooltip("The name of the animator parameter to write to.")]
        private string m_ParameterName = string.Empty;
        [SerializeField, Tooltip("When should the parameter be modified.")]
        private When m_When = When.OnEnter;
        [SerializeField, Tooltip("The value to write to the parameter on entering the state / subgraph.")]
        private int m_OnEnterValue = 0;
        [SerializeField, Tooltip("The value to write to the parameter on exiting the state / subgraph.")]
        private int m_OnExitValue = 0;

        private int m_ParameterHash = -1;

        public enum When
        {
            OnEnter,
            OnExit,
            Both
        }

        public override void Initialise(MotionGraphConnectable o)
        {
            base.Initialise(o);

            if (controller.bodyAnimator != null && !string.IsNullOrWhiteSpace(m_ParameterName))
                m_ParameterHash = Animator.StringToHash(m_ParameterName);
            else
                enabled = false;
        }

        public override void OnEnter()
        {
            if (m_When != When.OnExit)
                controller.bodyAnimator.SetInteger(m_ParameterHash, m_OnEnterValue);
        }

        public override void OnExit()
        {
            if (m_When != When.OnEnter)
                controller.bodyAnimator.SetInteger(m_ParameterHash, m_OnExitValue);
        }
    }
}