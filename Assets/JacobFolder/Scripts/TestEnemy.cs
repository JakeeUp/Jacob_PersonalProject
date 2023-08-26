using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public int howMuchDamage;
    public float howOftenWeDoDamage;
    public List<IDamageable> listOfThingsToDamage = new List<IDamageable>();

    private void Start()
    {
        StartCoroutine("DamageStuff");
    }

   
    IEnumerator DamageStuff()
    {
        while (true)  
        {
           
            for (int index = 0; index < listOfThingsToDamage.Count; index++)
            {
                // Use the interface to damage things
                listOfThingsToDamage[index].TakeDamage(howMuchDamage);
            }
            yield return new WaitForSeconds(howOftenWeDoDamage);
        }
    }

    
    void OnCollisionEnter(Collision whenSomethingHitsUs)
    {
        IDamageable thingThatHitUs = whenSomethingHitsUs.gameObject.GetComponent<IDamageable>();
        if (thingThatHitUs != null)  
        {
            listOfThingsToDamage.Add(thingThatHitUs);
        }
    }

    
    void OnCollisionExit(Collision whenSomethingStopsTouchingUs)
    {
        IDamageable thingThatStoppedTouching = whenSomethingStopsTouchingUs.gameObject.GetComponent<IDamageable>();
        if (thingThatStoppedTouching != null)
        {
            listOfThingsToDamage.Remove(thingThatStoppedTouching);
        }
    }
}
