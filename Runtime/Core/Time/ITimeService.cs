namespace PhikozzLib
{
    public interface ITimeService
    {
        void SetTimeScale(eTimeScaleMethods timeScaleMethod, float timeScale, float duration, bool lerp,
            float lerpSpeed, bool infinite);
        void FreezeFrame(float duration);
        void UnfreezeFrame();
        void ResetTimeScale();
    }
}

