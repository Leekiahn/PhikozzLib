using System;

namespace PhikozzLib
{
    public class StatModifier
    {
        private readonly ObservableValue<float> _remainingTime = new ObservableValue<float>(0f);

        public float Value { get; private set; }
        public eStatModifierType Type { get; private set; }
        public object Source { get; private set; }
        public float Duration { get; internal set; }

        public float RemainingTime
        {
            get => _remainingTime.Value;
            internal set => _remainingTime.Value = value;
        }

        public StatModifier(float value, eStatModifierType type, object source)
        {
            Value = value;
            Type = type;
            Source = source;
        }

        public void SubscribeRemainingTime(Action<float> callback)
        {
            _remainingTime.Subscribe(callback);
        }

        public void UnsubscribeRemainingTime(Action<float> callback)
        {
            _remainingTime.Unsubscribe(callback);
        }
    }
}
