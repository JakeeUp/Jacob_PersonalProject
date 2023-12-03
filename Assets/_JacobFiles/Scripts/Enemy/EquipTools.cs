using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EquipTools : Equip
{
     [Header("Iten Stats")]
     [SerializeField]float attackRate;
     [SerializeField]bool attacking;
     [SerializeField] float attackDistance;
     [SerializeField] bool DoesGatherResources;
     [SerializeField] bool DoesDealDmg;
     [SerializeField] int dmg;
     private EquipManager equipManager;

     [Header("Ranged Weapon")]
     public bool pistolType;
     public bool assaultType;
     [SerializeField]GameObject muzzle;
     [SerializeField] Transform muzzlePoint;
     [SerializeField] AudioClip shotSound;
     [SerializeField] AudioSource audios;

     Animator itemAnim;
     Camera cam;
     public Sprite weaponSprite;

     [Header("Zoom")]
     [SerializeField]bool isScoped;
     Scope scope;
     [SerializeField]float zoomFOV = 10f;
     [SerializeField] float normalFov;
     [SerializeField] bool isSniper;
     [SerializeField] GameObject bloodEffect;


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
            CanWeaponAttack();
        }
        else if (!attacking && pistolType && AmmoManager.instance.currentSmallAmmo > 0)
        {
            ExecuteAttack();
        }
        else if (!attacking && assaultType && AmmoManager.instance.currentLargeAmmo > 0)
        {
            ExecuteLargeWeaponAttack();
        }

    }

    private void ExecuteLargeWeaponAttack()
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

    private void ExecuteAttack()
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

    private void CanWeaponAttack()
    {
        attacking = true;
        itemAnim.SetTrigger("Attacking");
        Invoke("OnCanAttack", attackRate);
    }

    public override void OnAltAttackInput()
    {
        SniperCheck();
    }

    private void SniperCheck()
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

    void OnCanAttack()
    {
        attacking = false;
    }


    public void OnHit()
    {
        HitTarget();
    }

    private void HitTarget()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (DoesDealDmg && hit.collider.GetComponent<IDamageable>() != null)
            {
                hit.collider.GetComponent<IDamageable>().TakeDamage(dmg);
                GameObject obj = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(obj, 3);
            }
        }
        Debug.Log("Hit");
    }
}
