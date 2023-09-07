using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EquipTools : Equip
{
    public float attackRate;
    public bool attacking;
    public float attackDistance;
    public bool DoesGatherResources;
    public bool DoesDealDmg;
    public int dmg;
    private EquipManager equipManager;
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

    [Header("Zoom")]
    public bool isScoped;
    private Scope scope;
    public float zoomFOV = 10f;
    public float normalFov;
    public bool isSniper;


    private void Awake()
    {
        itemAnim = GetComponent<Animator>();
        cam = Camera.main;
        audios= GetComponent<AudioSource>();
        equipManager = FindObjectOfType<EquipManager>();
    }

    private void Start()
    {
        PlayerPrefs.GetFloat("CurrentPistolAmmo");
        PlayerPrefs.GetFloat("CurrentAssualtAmmo");
    }

    private void Update()
    {
        scope = GameObject.FindObjectOfType<Scope>();
        AutoFire();
    }

    private void AutoFire()
    {
        if (EquipManager.instance.autoFire == true)
        {
            OnAttackInput();
        }
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

       
        if (isSniper == true)
        {
            isScoped = !isScoped;
            scope.scopeImage.SetActive(isScoped);
            scope.weaponCamera.SetActive(!isScoped);

            if (isScoped == true)
            {
                //change the fov of main camera to zoom fov
                normalFov = scope.mainCam.fieldOfView;
                scope.mainCam.fieldOfView = zoomFOV;
                equipManager.DisableCrosshairImage();

            }
            if (isScoped == false)
            {
                scope.mainCam.fieldOfView = normalFov;
                equipManager.EnableCrosshairImage();
            }
        }
    }

    //public void PistolReload(float amount)
    //{
    //    if(pistolType)
    //    {
    //        AmmoManager.instance.ReloadPistol(amount);
    //        PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.currentSmallAmmo);
    //    }
    //}

    //public void AssaultReload(float amount)
    //{
    //    if(assaultType)
    //    {
    //        AmmoManager.instance.ReloadAssault(amount);
    //        PlayerPrefs.SetFloat("CurrentAssualtAmmo", AmmoManager.instance.currentLargeAmmo);
    //    }
    //}

    void OnCanAttack()
    {
        attacking = false;
    }


    public void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, attackDistance))
        {
            if(DoesDealDmg && hit.collider.GetComponent<IDamageable>() != null)
            {
                hit.collider.GetComponent<IDamageable>().TakeDamage(dmg);
            }
        }
        Debug.Log("Hit");
    }
}
