using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhikozzLib
{
    public class Stat
    {
        private readonly List<StatModifier> _modifiers = new List<StatModifier>();
        private readonly ObservableValue<float> _value;
        private float _baseValue;

        public float BaseValue
        {
            get => _baseValue;
            set
            {
                if (_baseValue != value)
                {
                    _baseValue = value;
                    RecalculateValue();
                }
            }
        }

        public float Value => _value.Value;

        public IReadOnlyList<StatModifier> Modifiers => _modifiers;

        public Stat(float baseValue)
        {
            _baseValue = baseValue;
            _value = new ObservableValue<float>(baseValue);
        }

        public void Subscribe(Action<float> callback)
        {
            _value.Subscribe(callback);
        }

        public void Unsubscribe(Action<float> callback)
        {
            _value.Unsubscribe(callback);
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            RecalculateValue();
        }

        public async UniTask AddModifierForDuration(StatModifier modifier, float duration)
        {
            modifier.Duration = duration;
            modifier.RemainingTime = duration;
            AddModifier(modifier);

            try
            {
                // RemoveModifier() 등으로 먼저 해제되면 RemainingTime이 0이 되어 루프가 끝난다.
                while (modifier.RemainingTime > 0f)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update);
                    modifier.RemainingTime = Mathf.Max(0f, modifier.RemainingTime - Time.deltaTime);
                }
            }
            finally
            {
                RemoveModifier(modifier);
            }
        }

        public void RemoveModifier(StatModifier modifier)
        {
            if (_modifiers.Remove(modifier))
            {
                modifier.RemainingTime = 0f;
                RecalculateValue();
            }
        }

        public void RemoveAllModifiersFromSource(object source)
        {
            bool isRemoved = false;

            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                if (_modifiers[i].Source == source)
                {
                    _modifiers[i].RemainingTime = 0f;
                    _modifiers.RemoveAt(i);
                    isRemoved = true;
                }
            }

            if (isRemoved)
            {
                RecalculateValue();
            }
        }

        private void RecalculateValue()
        {
            _value.Value = CalculateValue();
        }

        private float CalculateValue()
        {
            float flat = 0f;
            float percentAdd = 0f;
            float percentMult = 1f;

            foreach (var modifier in _modifiers)
            {
                switch (modifier.Type)
                {
                    case eStatModifierType.Flat:
                        flat += modifier.Value;
                        break;
                    case eStatModifierType.PercentAdd:
                        percentAdd += modifier.Value;
                        break;
                    case eStatModifierType.PercentMult:
                        percentMult *= 1 + modifier.Value;
                        break;
                }
            }

            float finalValue = _baseValue + flat;
            finalValue *= 1 + percentAdd;
            finalValue *= percentMult;

            return finalValue;
        }
    }
}
