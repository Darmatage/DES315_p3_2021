using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterShieldLogic_AmyS : MonoBehaviour
{
    public float shieldPower;
    private float shieldTotal;
    GameObject canvas;
    public Sprite sprite;

    [System.Serializable]
    public enum ShieldPos
    {
        LeftShield,
        RightShield,
        BodyShield
    }

    public ShieldPos shieldPos;
    public GameObject DeactivateObj;

    private float HealthLeftAnchorX = 190f - 50f;
    private float HealthLeftAnchorY = 93f - 80f;
    private float BackgroundLeftAnchorX = 130f - 50f;
    private float BackgroundLeftAnchorY = 100f - 80f;

    private GameObject shieldText;
    private GameObject shieldImage;
    private Text myText;
    private BoxCollider[] boxColliders;

    // Start is called before the first frame update
    void Start()
    {
        if(DeactivateObj != null)
        {
            boxColliders = DeactivateObj.GetComponents<BoxCollider>();

             // make sure they can't damage the bot while the shields are up
            foreach(BoxCollider boxCollider in boxColliders)
            {
                if(boxCollider != null)
                {
                    //Debug.Log("Deactivating");
                    boxCollider.enabled = false;
                }
            }
        }

        shieldTotal = shieldPower;
      
        canvas = GameObject.Find("GameHUD");

        if(canvas != null)
        {
            //Debug.Log("Canvas stuff");
             shieldText = new GameObject("shieldHealth");

             shieldImage = new GameObject("shieldImage");



            setUpHUD(shieldText, shieldImage);
            

            

        }
        
    }

    private void setUpHUD(GameObject shieldHealth, GameObject shieldBackground)
    {
        
        shieldBackground.transform.SetParent(canvas.transform);
        Image image = shieldBackground.AddComponent<Image>();
        image.sprite = sprite;
        image.color = new Color(189f / 255f, 214f / 255f, 98f / 255f, 190f / 255f);
        image.type = Image.Type.Sliced;
        shieldBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(220f, 50f);

        shieldHealth.transform.SetParent(canvas.transform);
        myText = shieldHealth.AddComponent<Text>();
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        myText.font = ArialFont;
        myText.material = ArialFont.material;
        myText.fontSize = 30;
        myText.color = Color.black;
        shieldHealth.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 50f);

        

    }

    void UpdateHUD(GameObject shieldHealth, GameObject shieldBackground, Text myText)
    {
        switch (shieldPos)
        {
            case ShieldPos.LeftShield:
                {
                    myText.text = "Left Shield: " + shieldTotal.ToString();
                    shieldHealth.GetComponent<RectTransform>().localPosition = new Vector3(HealthLeftAnchorX, HealthLeftAnchorY, 0f);
                    shieldHealth.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);



                    shieldBackground.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    shieldBackground.GetComponent<RectTransform>().localPosition = new Vector3(BackgroundLeftAnchorX, BackgroundLeftAnchorY, 0f);

                    break;
                }
            case ShieldPos.BodyShield:
                {
                    myText.text = "Body Shield: " + shieldTotal.ToString();
                    shieldHealth.GetComponent<RectTransform>().localPosition = new Vector3(HealthLeftAnchorX + 500f, HealthLeftAnchorY, 0f);
                    shieldHealth.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);



                    shieldBackground.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    shieldBackground.GetComponent<RectTransform>().localPosition = new Vector3(BackgroundLeftAnchorX + 512f, BackgroundLeftAnchorY, 0f);
                    break;
                }
            case ShieldPos.RightShield:
                {
                    myText.text = "Right Shield: " + shieldTotal.ToString();
                    shieldHealth.GetComponent<RectTransform>().localPosition = new Vector3(HealthLeftAnchorX + 500f + 510f, HealthLeftAnchorY, 0f);
                    shieldHealth.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);



                    shieldBackground.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    shieldBackground.GetComponent<RectTransform>().localPosition = new Vector3(BackgroundLeftAnchorX + 512f + 512f, BackgroundLeftAnchorY, 0f);
                    break;
                }
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision");
        if(other.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(other.gameObject.GetComponent<HazardDamage>().damage);

        }
    }

    void TakeDamage(float amount)
    {
        shieldTotal -= amount;

        if (shieldTotal <= 0)
        {
            shieldTotal = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUD(shieldText, shieldImage, myText);

        if(shieldTotal <= 0)
        {
            shieldImage.GetComponent<Image>().color = Color.gray;

            gameObject.transform.root.gameObject.GetComponent<MonsterAttack_AmyS>().setNumShieldsBroken();

            foreach(BoxCollider boxCollider in boxColliders)
            {
                if(boxCollider != null)
                {
                    //Debug.Log("Activating");
                    boxCollider.enabled = true;
                }
            }

            gameObject.SetActive(false);
        }
    }
}
