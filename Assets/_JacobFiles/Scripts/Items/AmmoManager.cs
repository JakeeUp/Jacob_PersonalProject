using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance;

    [Header("Ammo UI")]
    public Image icon;
    public GameObject WeaponUI;
    public TextMeshProUGUI AmmoText;
    private EquipTools equipTools;
    [Header("Ammo Capacity")]
    public float currentSmallAmmo;
    public float maxSmallAmmo;
    public float currentLargeAmmo;
    public float maxLargeAmmo;
    public AudioClip reloadSound;
    private AudioSource audios;

    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
        WeaponUI.SetActive(false);
        audios = GetComponent<AudioSource>();
        currentSmallAmmo = PlayerPrefs.GetFloat("CurrentPistolAmmo");
        currentLargeAmmo = PlayerPrefs.GetFloat("CurrentAssaultAmmo");

       
    }

    private void Update()
    {
        equipTools = GameObject.FindObjectOfType<EquipTools>();
        if(currentLargeAmmo <= 0)
        {
            currentLargeAmmo = 0;
        }
        if (equipTools != null)
        {
            WeaponUI.SetActive(true);
            icon.sprite = equipTools.weaponSprite;
            AmmoText.text = string.Empty;
            if (equipTools.assaultType == true)
                AmmoText.text = ("" + currentLargeAmmo + "/" + "" + maxLargeAmmo).ToString();
            else if (equipTools.pistolType == true)
                AmmoText.text = ("" + currentSmallAmmo + "/" + "" + maxSmallAmmo).ToString();
        }
        else
        {
            WeaponUI.SetActive(false);
        }
    }

    public void ReloadPistol(float amount)
    {
        currentSmallAmmo += amount;
        audios.PlayOneShot(reloadSound);

        if (currentSmallAmmo >= maxSmallAmmo)
        {
            currentSmallAmmo = maxSmallAmmo;
        }
        PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.currentSmallAmmo);
    }

    public void ReloadAssault(float amount)
    {
        currentLargeAmmo += amount;
        audios.PlayOneShot(reloadSound);

        if (currentLargeAmmo >= maxLargeAmmo)
        {
            currentLargeAmmo = maxLargeAmmo;
        }
        PlayerPrefs.SetFloat("CurrentAssaultAmmo", AmmoManager.instance.currentLargeAmmo);
    }



}
