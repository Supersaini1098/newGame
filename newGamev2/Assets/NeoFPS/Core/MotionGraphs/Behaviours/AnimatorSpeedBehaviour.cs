using UnityEngine;
using NeoFPS.CharacterMotion;
using NeoFPS.CharacterMotion.Parameters;
using NeoSaveGames.Serialization;
using System.Collections;

namespace NeoFPS.CharacterMotion
{
    [MotionGraphElement("Animation/AnimatorSpeed", "AnimatorSpeedBehaviour")]
    public class AnimatorSpeedBehaviour : MotionGraphBehaviour
    {
        [SerializeField, Tooltip("The animator parameter name the speed value should be written to.")]
        private string m_SpeedParamName = "speed";

        private int m_SpeedParamHash = -1;

        public override void Initialise(MotionGraphConnectable o)
        {
            base.Initialise(o);

            if (controller.bodyAnimator != null)
                m_SpeedParamHash = Animator.StringToHash(m_SpeedParamName);
            else
                enabled = false;
        }

        public override void Update()
        {
            base.Update();

            controller.bodyAnimator.SetFloat(m_SpeedParamHash, controller.characterController.velocity.magnitude);
        }
    }
}