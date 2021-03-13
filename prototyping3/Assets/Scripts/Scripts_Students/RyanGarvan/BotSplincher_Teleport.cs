using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSplincher_Teleport : MonoBehaviour
{
    public GameObject m_cursorPrefab;
    public float m_cursorSpeed;
    public GameObject m_damageTriggerPrefab;
    GameObject m_damageTrigger = null;
    public LayerMask m_enemyMask;

    //grab axis from parent object
    public string m_button1;
    public string m_button2;
    public string m_button3;
    public string m_button4; // currently boost in player move script

    BotSplincher_Move m_botMove;

    bool m_cursorOut = false;
    GameObject m_cursor;

    

    void Start()
    {
        m_button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        m_button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        m_button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        m_button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        m_botMove = GetComponent<BotSplincher_Move>();
    }

    void Update()
    {
        if (Input.GetButtonDown(m_button1))
        {
            if (m_cursorOut)
            {
                Vector3 targetPos = new Vector3(m_cursor.transform.position.x, transform.position.y, m_cursor.transform.position.z);

                if (!GetInsideWall(targetPos))
                {
                    Transform enemy = GetInsideEnemy(targetPos);

                    if (enemy != null)
                    {
                        m_damageTrigger = Instantiate(m_damageTriggerPrefab);
                        BotSplincher_DamageBox m_triggerDamageBox = m_damageTrigger.GetComponent<BotSplincher_DamageBox>();
                        m_triggerDamageBox.m_targetPos = enemy.position;
                        m_damageTrigger.transform.position = enemy.position + (targetPos - enemy.position).normalized * 3;
                        m_triggerDamageBox.m_startPos = m_damageTrigger.transform.position;
                        m_damageTrigger.transform.rotation = transform.rotation;

                        HazardDamage triggerHazardDamage = m_damageTrigger.GetComponent<HazardDamage>();

                        if (transform.root.tag == "Player1")
                        {
                            triggerHazardDamage.isPlayer1Weapon = true;
                        }
                        else if (transform.root.tag == "Player2")
                        {
                            triggerHazardDamage.isPlayer2Weapon = true;
                        }
                    }

                    //transform.LookAt(new Vector3(m_cursor.transform.position.x, transform.position.y, m_cursor.transform.position.z));
                    transform.position = targetPos;
                    Destroy(m_cursor);
                    m_botMove.isGrabbed = false;
                    m_cursorOut = false;

                    gameObject.layer = 17;

                    transform.GetChild(5).gameObject.SetActive(false);

                    foreach (MeshRenderer mesh in transform.GetChild(0).GetComponentsInChildren<MeshRenderer>())
                    {
                        mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0.5f);
                    }

                    StartCoroutine(BecomeCorporeal());
                }
            }
            else
            {
                m_cursor = Instantiate(m_cursorPrefab);
                m_cursor.transform.position = transform.position + new Vector3(0, 3, 0);
                m_botMove.isGrabbed = true;
                m_cursorOut = true;
            }
        }

        if (m_cursorOut)
        {
            float cursorMoveX = Input.GetAxisRaw(m_botMove.pHorizontal) * m_cursorSpeed * Time.deltaTime;
            float cursorMoveY = Input.GetAxisRaw(m_botMove.pVertical) * m_cursorSpeed * Time.deltaTime;
            m_cursor.transform.Translate(cursorMoveX, 0, cursorMoveY);
        }
    }

    IEnumerator BecomeCorporeal()
    {
        while (GetInsideEnemy(transform.position) != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        foreach (MeshRenderer mesh in transform.GetChild(0).GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 1.0f);
        }

        m_damageTrigger = null;
        gameObject.layer = 0;
        transform.GetChild(5).gameObject.SetActive(true);
    }

    Transform GetInsideEnemy(Vector3 position)
    {
        Collider[] collisions = Physics.OverlapBox(position, GetComponents<BoxCollider>()[0].size / 2, transform.rotation, m_enemyMask);
        Transform enemy = null;

        foreach (Collider collider in collisions)
        {
            if ((transform.root.tag == "Player1" && collider.transform.root.tag == "Player2") || (transform.root.tag == "Player2" && collider.transform.root.tag == "Player1"))
            {
                enemy = collider.transform.root;
                break;
            }
        }

        return enemy;
    }

    bool GetInsideWall(Vector3 position)
    {
        Collider[] collisions = Physics.OverlapBox(position, GetComponents<BoxCollider>()[0].size / 2, transform.rotation, m_botMove.groundLayer);

        foreach (Collider collider in collisions)
        {
            if (collider.transform.root.tag != "Player1" && collider.transform.root.tag != "Player2")
            {
                return true;
            }
        }

        return false;
    }
}