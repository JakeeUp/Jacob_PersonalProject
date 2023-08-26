using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PlayerAttributes : MonoBehaviour, IDamageable
{
    public pAttributes _health;
    public pAttributes _hunger;
    public pAttributes _thirst;

    public float EmtpyHungerHPDecay;
    public float EmptythirstHPDecay;
    public UnityEvent getDamage;
    private void Start()
    {
        //current value is equal to the start value
        _health.currentValue = _health.startValue;
        _hunger.currentValue = _hunger.startValue;
        _thirst.currentValue = _thirst.startValue;

    }

    private void Update()
    {
        Handle_NeedsOverTime();
        Handle_HealthDecayFromNoHungerOrThirst();
        Handle_PlayerDeath();
        Handle_UI();
    }

    private void Handle_NeedsOverTime()
    {
        _hunger.Subtract(_hunger.decayRate * Time.deltaTime);
        _thirst.Subtract(_thirst.decayRate * Time.deltaTime);
    }

    private void Handle_HealthDecayFromNoHungerOrThirst()
    {
        if (_hunger.currentValue == 0.0f)
        {
            _health.Subtract(EmtpyHungerHPDecay * Time.deltaTime);
        }

        if (_thirst.currentValue == 0.0f)
        {
            _health.Subtract(EmptythirstHPDecay * Time.deltaTime);
        }
    }

    private void Handle_PlayerDeath()
    {
        if (_health.currentValue == 0.0f)
        {
            Die();
        }
    }

    private void Handle_UI()
    {
        _health.uiSlider.fillAmount = _health.GetPercentage();
        _hunger.uiSlider.fillAmount = _hunger.GetPercentage();
        _thirst.uiSlider.fillAmount = _thirst.GetPercentage();
    }


    public void Heal(float amount)
    {
        //add to _health depend on that amount
        _health.Add(amount);
    }

    public void Eat(float amount)
    {
        _hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        _thirst.Add(amount);
    }

    public void TakeDamage(int damageAmount)
    {
        //reduce _health depend on damage amount
        _health.Subtract(damageAmount);
        //if we get damage then invoke 
        getDamage?.Invoke();
    }

    public void Die()
    {
        Debug.Log("Player Died");
    }
}

[System.Serializable]
public class pAttributes
{
    public float currentValue, maxValue, startValue, regenerateRate, decayRate;
    public Image uiSlider;

    public void Add(float amount)
    {
        //current value is the minimum value of 2 number 
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        //current value is the maximum value of 2 number
        currentValue = Mathf.Max(currentValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        //return us the result of divid between current value and max value
        return currentValue / maxValue;
    }
}

//What is the interface???
//Interfaces are a set of methods, properties, and other members that a target class must implement.
//C# is a type-safe language, meaning it checks if every statement is using Data Types correctly,
//by using an interface as your Data Type, you are able to access the interface fields on different classes.
//The idea is to have multiple classes that implement the IDamageable class,
//so we can reference the Interface rather than the classes themselves.
//In this example, we could have the Player, Enemies, and Props all implement the IDamegeable,
//this way if we get a collision in Unity,
//we check if it is IDamegeable, and apply corresponding damage, regardless of its class.
public interface IDamageable
{
    void TakeDamage(int DamageAmount);
}
