using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerAttributes : MonoBehaviour, IDamageable
{
    public pAttributes _health;
    public pAttributes _hunger;
    public pAttributes _thirst;

    public pAttributes _flashLight;
    public bool switchOn;
    public GameObject flashLightObj;

    public float EmtpyHungerHPDecay;
    public float EmptythirstHPDecay;
    public UnityEvent getDamage;

    public AudioSource audioSource;
    public AudioClip damageAudioClip;
    public AudioClip dieAudioClip;
    public float dieDelay = 2.0f;

    private PlayerController playerController;

    private void Start()
    {
        //current value is equal to the start value
        _health.currentValue = _health.startValue;
        _hunger.currentValue = _hunger.startValue;
        _thirst.currentValue = _thirst.startValue;
        _flashLight.currentValue = _flashLight.startValue;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = damageAudioClip;
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        Handle_NeedsOverTime();
        Handle_HealthDecayFromNoHungerOrThirst();
        Handle_PlayerDeath();
        Handle_UI();
        HandleFlashLight();
    }

    private void HandleFlashLight()
    {
        if (switchOn == true)
        {
            _flashLight.Subtract(_flashLight.decayRate * Time.deltaTime);
        }

        if (_flashLight.currentValue == 0.0f)
        {
            flashLightObj.SetActive(false);
            switchOn = false;
        }


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
    public void OnFlashInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Switch();
    }
    private void Handle_UI()
    {
        _health.uiSlider.fillAmount = _health.GetPercentage();
        _hunger.uiSlider.fillAmount = _hunger.GetPercentage();
        _thirst.uiSlider.fillAmount = _thirst.GetPercentage();
        _flashLight.uiSlider.fillAmount = _flashLight.GetPercentage();
    }

    public void Switch()
    {
        switchOn = !switchOn;
        if (switchOn == true)
            flashLightObj.SetActive(true);
        else
            flashLightObj.SetActive(false);
    }
    public void Heal(float amount)
    {
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
    public void Recharge(float amount)
    {
        _flashLight.Add(amount);
    }


    public void TakeDamage(int damageAmount)
    {
        _health.Subtract(damageAmount);
        if (audioSource != null && damageAudioClip != null)
        {
            audioSource.PlayOneShot(damageAudioClip);
        }
        //if we get damage then invoke 
        getDamage?.Invoke();
    }

    public void Die()
    {
        Debug.Log("Player Died");
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (audioSource != null && dieAudioClip != null)
        {
            audioSource.PlayOneShot(dieAudioClip);
        }
        StartCoroutine(DelayedSceneTransition());
    }
    private IEnumerator DelayedSceneTransition()
    {
        yield return new WaitForSeconds(dieDelay);

        // Transition to the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}

[System.Serializable]
public class pAttributes
{
    public float currentValue, maxValue, startValue, regenerateRate, decayRate;
    public Image uiSlider;

    public void Add(float amount)
    {
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        currentValue = Mathf.Max(currentValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return currentValue / maxValue;
    }
}

public interface IDamageable
{
    void TakeDamage(int DamageAmount);
}
