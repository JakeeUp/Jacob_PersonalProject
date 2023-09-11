using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpScare : MonoBehaviour
{
    public bool fear;
    public AudioClip scareSFX;
    private AudioSource audios;
    public TextMeshProUGUI infoText;
    public string[] talkText;
    public Animator cameraAnim;
    public Animator patientAnim;
    public GameObject patient;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && fear != true)
        {
            audios.PlayOneShot(scareSFX);
            cameraAnim.SetTrigger("Shake");
            patientAnim.SetTrigger("Scare");
            Invoke("PlayAudio", 1f);
            Invoke("StopAudio", 5f);

            fear = true;
        }
    }

    public void PlayAudio()
    {
        audios.Play();
        infoText.text = talkText[Random.Range(0, talkText.Length)];
    }

    public void StopAudio()
    {
        audios.Stop();
        infoText.text = string.Empty;
        patient.SetActive(false);
    }
}
