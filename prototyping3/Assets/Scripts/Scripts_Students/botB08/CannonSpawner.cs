using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSpawner : MonoBehaviour
{
    public string button2;

    [SerializeField] private MeshRenderer CannonIndicator;

    [SerializeField] private Color CannonReadyColor;
    [SerializeField] private Color CannonUsedColor;

    [SerializeField] private float CooldownTime;
    private float cooldownTimer = 0f;
    
    [SerializeField] private Transform CannonSpawnPoint;
    [SerializeField] private Transform CannonTarget;
    [SerializeField] private GameObject CannonPrefab;
    
    private bool CannonAvailable = true;

    private AudioSource AudioSrc;
    [SerializeField] private AudioClip CannonAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        CannonIndicator.material.color = CannonReadyColor;
        AudioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CannonAvailable && Input.GetButtonDown(button2))
        {
            GameObject cannon = Instantiate(CannonPrefab, CannonSpawnPoint.position, Quaternion.identity);
            cannon.GetComponent<CannonBehavior>().Target = CannonTarget.position;
            AudioSrc.PlayOneShot(CannonAudio);
            
            CannonAvailable = false;
            cooldownTimer = 0f;
        }

        if (!CannonAvailable)
        {
            cooldownTimer += Time.deltaTime;
            CannonIndicator.material.color = Color.Lerp(CannonUsedColor, CannonReadyColor, cooldownTimer / CooldownTime);

            if (cooldownTimer >= CooldownTime)
            {
                CannonAvailable = true;
            }
        }
    }
    
}
