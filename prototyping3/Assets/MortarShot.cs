using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShot : MonoBehaviour
{
    public GameObject ShootFrom;
    public float Range = 3.0f;
    public float LineWidth = 1.0f;
    public Color Telemetry = Color.red;

    public GameObject ExplosionPrefab = null;

    public float ShotCooldown = 2.0f;
    float Cooldown = 0.0f;

    public LineRenderer render;

    string button2;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<LineRenderer>();
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
    }

    void LineRender()
    {
        render.enabled = true;
        Vector3 begin = ShootFrom.transform.position;
        Vector3 end = begin + transform.up * Range;
        Vector3[] array = { begin, end };
        render.SetPositions(array);
        render.startColor = Telemetry;
        render.endColor = Telemetry;
        render.material.SetColor("_Color", Telemetry);
        render.startWidth = LineWidth;
        render.endWidth = LineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        LineRender();

        if ((Input.GetButtonDown(button2)) && (Cooldown <= 0.0f))
        {
            Ray ray = new Ray();
            ray.origin = ShootFrom.transform.position;
            ray.direction = transform.up;
            RaycastHit hit;
            bool result = Physics.Raycast(ray, out hit);

            GameObject Splosion = Instantiate(ExplosionPrefab);

            float SpawnDist = hit.distance;
            //if (SpawnDist > Range)
            //{
                SpawnDist = Range;
            //}

            Splosion.transform.position = ray.origin + ray.direction * SpawnDist;

            if (gameObject.transform.root.tag == "Player1") { Splosion.GetComponent<HazardDamage>().isPlayer1Weapon = true; }
            if (gameObject.transform.root.tag == "Player2") { Splosion.GetComponent<HazardDamage>().isPlayer2Weapon = true; }
        }

    }
}
