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
    AudioSource m_audioSourceTeleport;
    AudioSource m_audioSourceTeleportStart;
    AudioSource m_audioSourceTeleportFail;
    AudioSource m_audioSourceTeleportDamage;

    [HideInInspector]
    public bool m_cursorOut = false;
    GameObject m_cursor;

    CameraFollow m_cameraFollow;

    public GameObject damageParticlesPrefab;
    public GameObject teleportParticlesPrefab;

    void Start()
    {
        m_button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        m_button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        m_button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        m_button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        m_botMove = GetComponent<BotSplincher_Move>();
        m_audioSourceTeleport = GetComponents<AudioSource>()[0];
        m_audioSourceTeleportStart = GetComponents<AudioSource>()[1];
        m_audioSourceTeleportFail = GetComponents<AudioSource>()[2];
        m_audioSourceTeleportDamage = GetComponents<AudioSource>()[3];
        m_cameraFollow = transform.root.GetComponentInChildren<CameraFollow>();
    }

    void Update()
    {
        Vector3 targetPos = Vector3.zero;

        if (m_cursorOut)
        {
            targetPos = new Vector3(m_cursor.transform.position.x, transform.position.y, m_cursor.transform.position.z);

            if (GetInsideWall(targetPos))
            {
                m_cursor.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0, 0);
            }
            else if (GetInsideEnemy(targetPos) != null && Time.time * 4 - Mathf.Floor(Time.time * 4) < 0.5f)
            {
                m_cursor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
            }
            else
            {
                m_cursor.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0, 1);
            }
        }

        if (Input.GetButtonDown(m_button1))
        {
            if (m_cursorOut)
            {
                Destroy(m_cursor);
                m_botMove.isCursorOut = false;
                m_cursorOut = false;
                m_cameraFollow.playerObj = transform;

                if (!GetInsideWall(targetPos))
                {
                    Transform enemy = GetInsideEnemy(targetPos);

                    //transform.LookAt(new Vector3(m_cursor.transform.position.x, transform.position.y, m_cursor.transform.position.z));

                    GameObject teleportParticlesA = Instantiate(teleportParticlesPrefab, transform.position, transform.rotation);
                    StartCoroutine(destroyParticles(teleportParticlesA));

                    transform.position = targetPos;

                    GameObject teleportParticlesB = Instantiate(teleportParticlesPrefab, transform.position, transform.rotation);
                    StartCoroutine(destroyParticles(teleportParticlesB));

                    gameObject.layer = 17;

                    transform.GetChild(5).gameObject.SetActive(false);

                    foreach (MeshRenderer mesh in transform.GetChild(0).GetComponentsInChildren<MeshRenderer>())
                    {
                        mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0.5f);
                    }

                    m_audioSourceTeleport.pitch = Random.Range(0.8f, 1.2f);
                    m_audioSourceTeleport.Play();

                    StartCoroutine(BecomeCorporeal());

                    if (enemy != null)
                    {
                        /*m_damageTrigger = Instantiate(m_damageTriggerPrefab);
                        BotSplincher_DamageBox m_triggerDamageBox = m_damageTrigger.GetComponent<BotSplincher_DamageBox>();
                        m_triggerDamageBox.m_targetPos = enemy.position;
                        m_damageTrigger.transform.position = enemy.position + (targetPos - enemy.position).normalized * 3;
                        m_triggerDamageBox.m_startPos = m_damageTrigger.transform.position;
                        m_damageTrigger.transform.rotation = transform.rotation;*/

                        Vector3 dmgTargetPos = enemy.position;
                        Vector3 dmgStartPos = enemy.position + (targetPos - enemy.position).normalized * 3;
                        SplinchEnemy(enemy.gameObject, dmgTargetPos, dmgStartPos, 40);
                        m_audioSourceTeleportDamage.Play();

                        /*HazardDamage triggerHazardDamage = m_damageTrigger.GetComponent<HazardDamage>();

                        if (transform.root.tag == "Player1")
                        {
                            triggerHazardDamage.isPlayer1Weapon = true;
                        }
                        else if (transform.root.tag == "Player2")
                        {
                            triggerHazardDamage.isPlayer2Weapon = true;
                        }*/
                    }
                }
                else
                {
                    m_audioSourceTeleportFail.Play();
                }
            }
            else
            {
                m_cursor = Instantiate(m_cursorPrefab);
                m_cursor.transform.position = transform.position + new Vector3(0, 3, 0);
                m_botMove.isCursorOut = true;
                m_cursorOut = true;
                m_cameraFollow.playerObj = m_cursor.transform;
                m_audioSourceTeleportStart.pitch = Random.Range(0.8f, 1.2f);
                m_audioSourceTeleportStart.Play();
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
                enemy = collider.transform.root.GetComponentInChildren<BotBasic_Damage>().transform;
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

    void SplinchEnemy(GameObject enemy, Vector3 targetPos, Vector3 startPos, float speed)
    {
        GameHandler gameHandler = FindObjectOfType<GameHandler>();

        gameObject.layer = 17;
        Vector3 knockback = (targetPos - startPos).normalized * speed * 100;
        knockback.y = 0;
        enemy.GetComponent<Rigidbody>().AddForce(knockback);
        GetComponent<Rigidbody>().AddForce(-knockback);
        BotBasic_Damage targetDamage = enemy.GetComponent<BotBasic_Damage>();

        if (targetDamage.shieldPowerBottom <= 0 || targetDamage.shieldPowerFront <= 0 || targetDamage.shieldPowerTop <= 0 || targetDamage.shieldPowerBottom <= 0 || targetDamage.shieldPowerLeft <= 0 || targetDamage.shieldPowerRight <= 0)
        {
            gameHandler.TakeDamage(targetDamage.transform.root.tag, 1.0f);
        }

        if (targetDamage.shieldPowerBack > 0)
        {
            targetDamage.shieldPowerBack -= 1;
            StartCoroutine(ShieldHitDisplay(targetDamage.shieldBackObj));

            if (targetDamage.shieldPowerBack <= 0)
            {
                targetDamage.shieldPowerBack = 0;
                gameHandler.PlayerShields(targetDamage.transform.root.tag, "Back");
            }
        }
        else
        {
            targetDamage.dmgParticlesBack.SetActive(true);
        }

        if (targetDamage.shieldPowerFront > 0)
        {
            targetDamage.shieldPowerFront -= 1;
            StartCoroutine(ShieldHitDisplay(targetDamage.shieldFrontObj));

            if (targetDamage.shieldPowerFront <= 0)
            {
                targetDamage.shieldPowerFront = 0;
                gameHandler.PlayerShields(targetDamage.transform.root.tag, "Front");
            }
        }
        else
        {
            targetDamage.dmgParticlesFront.SetActive(true);
        }

        if (targetDamage.shieldPowerTop > 0)
        {
            targetDamage.shieldPowerTop -= 1;
            StartCoroutine(ShieldHitDisplay(targetDamage.shieldTopObj));

            if (targetDamage.shieldPowerTop <= 0)
            {
                targetDamage.shieldPowerTop = 0;
                gameHandler.PlayerShields(targetDamage.transform.root.tag, "Top");
            }
        }
        else
        {
            targetDamage.dmgParticlesTop.SetActive(true);
        }

        if (targetDamage.shieldPowerBottom > 0)
        {
            targetDamage.shieldPowerBottom -= 1;
            StartCoroutine(ShieldHitDisplay(targetDamage.shieldBottomObj));

            if (targetDamage.shieldPowerBottom <= 0)
            {
                targetDamage.shieldPowerBottom = 0;
                gameHandler.PlayerShields(targetDamage.transform.root.tag, "Bottom");
            }
        }

        if (targetDamage.shieldPowerLeft > 0)
        {
            targetDamage.shieldPowerLeft -= 1;
            StartCoroutine(ShieldHitDisplay(targetDamage.shieldLeftObj));

            if (targetDamage.shieldPowerLeft <= 0)
            {
                targetDamage.shieldPowerLeft = 0;
                gameHandler.PlayerShields(targetDamage.transform.root.tag, "Left");
            }
        }
        else
        {
            targetDamage.dmgParticlesLeft.SetActive(true);
        }

        if (targetDamage.shieldPowerRight > 0)
        {
            targetDamage.shieldPowerRight -= 1;
            StartCoroutine(ShieldHitDisplay(targetDamage.shieldRightObj));

            if (targetDamage.shieldPowerRight <= 0)
            {
                targetDamage.shieldPowerRight = 0;
                gameHandler.PlayerShields(targetDamage.transform.root.tag, "Right");
            }
        }
        else
        {
            targetDamage.dmgParticlesRight.SetActive(true);
        }

        GameObject damageParticles = Instantiate(damageParticlesPrefab, (transform.position + enemy.transform.position) / 2.0f, enemy.transform.rotation);
        StartCoroutine(destroyParticles(damageParticles));
    }

    IEnumerator ShieldHitDisplay(GameObject shieldObj)
    {
        shieldObj.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        shieldObj.SetActive(false);
    }

    IEnumerator destroyParticles(GameObject particles)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(particles);
    }
}