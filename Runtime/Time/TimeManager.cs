using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace PhikozzLib
{
    public class TimeManager : MonoBehaviour, ITimeService, IServiceRegister
    {
        public void RegisterService()
        {
            ServiceLocator.Register<ITimeService>(this);
        }

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

        public void FreezeFrame(float duration)
        {
            MMFreezeFrameEvent.Trigger(duration);
        }

        public void UnfreezeFrame()
        {
            MMTimeScaleEvent.Unfreeze();
        }

        public void ResetTimeScale()
        {
            MMTimeScaleEvent.Reset();
        }
    }
}