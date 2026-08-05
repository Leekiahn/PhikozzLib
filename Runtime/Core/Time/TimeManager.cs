using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhikozzLib
{
    [RequireComponent(typeof(MMTimeManager))]
    public class TimeManager : MonoBehaviour, ITimeService, IServiceRegister
    {
        public void RegisterService()
        {
            ServiceLocator.Register<ITimeService>(this);
        }

        [PropertySpace(SpaceBefore = 30f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void SetTimeScale(eTimeScaleMethods timeScaleMethod, float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite)
        {
            MMTimeScaleMethods method = default;

            switch (timeScaleMethod)
            {
                case eTimeScaleMethods.For:
                    method = MMTimeScaleMethods.For;
                    break;
                case eTimeScaleMethods.Reset:
                    method = MMTimeScaleMethods.Reset;
                    break;
                case eTimeScaleMethods.Unfreeze:
                    method = MMTimeScaleMethods.Unfreeze;
                    break;
            }

            MMTimeScaleEvent.Trigger(method, timeScale, duration, lerp, lerpSpeed, infinite);
        }

        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public void FreezeFrame(float duration)
        {
            MMFreezeFrameEvent.Trigger(duration);
        }

        [ButtonGroup]
        public void UnfreezeFrame()
        {
            MMTimeScaleEvent.Unfreeze();
        }

        [ButtonGroup]
        public void ResetTimeScale()
        {
            MMTimeScaleEvent.Reset();
        }
    }
}