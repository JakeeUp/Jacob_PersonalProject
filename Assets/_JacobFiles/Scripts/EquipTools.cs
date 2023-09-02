using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTools : Equip
{
    public float attackRate;
    public bool attacking;
    public float attackDistance;
    public bool DoesGatherResources;
    public bool DoesDealDmg;
    public int dmg;

    [Header("Ranged Weapon")]
    public bool pistolType;
    public bool assaultType;
    public GameObject muzzle;
    public Transform muzzlePoint;
    public AudioClip shotSound;
    private AudioSource audios;

    private Animator itemAnim;
    private Camera cam;
    public Sprite weaponSprite;

    private void Awake()
    {
        itemAnim = GetComponent<Animator>();
        cam = Camera.main;
        audios= GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayerPrefs.GetFloat("CurrentPistolAmmo");
        PlayerPrefs.GetFloat("CurrentAssualtAmmo");
    }

    public override void OnAttackInput()
    {
        if (!attacking && !pistolType && !assaultType)
        {
            attacking = true;
            itemAnim.SetTrigger("Attacking");
            Invoke("OnCanAttack", attackRate);
        }
        else if (!attacking && pistolType && AmmoManager.instance.currentSmallAmmo > 0)
        {

            attacking = true;
            itemAnim.SetTrigger("Attacking");
            Invoke("OnCanAttack", attackRate);
            GameObject obj = Instantiate(muzzle, muzzlePoint.transform.position, muzzlePoint.transform.rotation * Quaternion.Euler(90, 0, 0));
            Destroy(obj, 0.05f);
            audios.PlayOneShot(shotSound);

            AmmoManager.instance.currentSmallAmmo--;
            PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.currentSmallAmmo);
        }
        else if (!attacking && assaultType && AmmoManager.instance.currentLargeAmmo > 0)
        {

            attacking = true;
            itemAnim.SetTrigger("Attacking");
            Invoke("OnCanAttack", attackRate);
            GameObject obj = Instantiate(muzzle, muzzlePoint.transform.position, muzzlePoint.transform.rotation * Quaternion.Euler(90, 0, 0));
            Destroy(obj, 0.05f);
            audios.PlayOneShot(shotSound);

            AmmoManager.instance.currentLargeAmmo--;
            PlayerPrefs.SetFloat("CurrentAssaultAmmo", AmmoManager.instance.currentLargeAmmo);
        }

    }

    public override void OnAltAttackInput()
    {
        if(!attacking && !pistolType && !assaultType)
        {
            attacking = true;
            itemAnim.SetTrigger("AltAttacking");
            Invoke("OnCanAttack", attackRate);
        }
    }

    public void PistolReload(float amount)
    {
        if(pistolType)
        {
            AmmoManager.instance.ReloadPistol(amount);
            PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.currentSmallAmmo);
        }
    }

    public void AssaultReload(float amount)
    {
        if(assaultType)
        {
            AmmoManager.instance.ReloadAssault(amount);
            PlayerPrefs.SetFloat("CurrentAssualtAmmo", AmmoManager.instance.currentLargeAmmo);
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }


    public void OnHit()
    {
        Debug.Log("Hit");
    }
}
