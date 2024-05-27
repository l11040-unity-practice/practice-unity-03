using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition UICondition;
    Condition _health { get { return UICondition.Health; } }
    Condition _hunger { get { return UICondition.Hunger; } }
    Condition _stamina { get { return UICondition.Stamina; } }

    public float NoHungerHealthDecay;

    private void Update()
    {
        _hunger.Subtract(_hunger.PassiveValue * Time.deltaTime);
        _stamina.Add(_stamina.PassiveValue * Time.deltaTime);

        if (_hunger.CurValue == 0f)
        {
            _health.Subtract(NoHungerHealthDecay * Time.deltaTime);
        }

        if (_health.CurValue == 0f)
        {
            Die();
        }
    }
    public void Heal(float amount)
    {
        _health.Add(amount);
    }

    public void Eat(float amount)
    {
        _hunger.Add(amount);
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
