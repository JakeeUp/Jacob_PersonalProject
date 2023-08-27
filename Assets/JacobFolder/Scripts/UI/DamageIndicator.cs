using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//WRITE AS MANY NOTES AS POSSIBLE NEED TO KNOW THIS 
public class DamageIndicator : MonoBehaviour
{
    public Image damageImage;
    public float flashSpeed;

    private Coroutine fadeAwayImage;

    public void StartFlashing()
    {
        // If the flashing coroutine is already running, stop it
        if (fadeAwayImage != null)
            StopCoroutine(fadeAwayImage);

        // Enable the damage image and set it to white
        damageImage.enabled = true;
        damageImage.color = Color.white;

        fadeAwayImage = StartCoroutine(FadeAwayImage());
    }

    IEnumerator FadeAwayImage()
    {
        float imageAlpha = 1.0f;

        while (imageAlpha > 0.0f)
        {
            // Decrease the alpha over time to achieve the fading effect
            imageAlpha -= (1.0f / flashSpeed) * Time.deltaTime;
            damageImage.color = new Color(1.0f, 1.0f, 1.0f, imageAlpha);
            yield return null;
        }

        damageImage.enabled = false;
    }
}
