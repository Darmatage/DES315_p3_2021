using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSplincher_Teleport : MonoBehaviour
{
    public GameObject m_cursorPrefab;
    public float m_cursorSpeed;

	//grab axis from parent object
	public string m_button1;
	public string m_button2;
	public string m_button3;
	public string m_button4; // currently boost in player move script

    BotBasic_Move m_botMove;

    bool m_cursorOut = false;
    GameObject m_cursor;

    void Start()
    {
		m_button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
		m_button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
		m_button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        m_button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        m_botMove = GetComponent<BotBasic_Move>();
    }

    void Update()
    {
		if (Input.GetButtonDown(m_button1))
        {
			if (m_cursorOut)
            {
                transform.LookAt(new Vector3(m_cursor.transform.position.x, transform.position.y, m_cursor.transform.position.z));
                transform.position = new Vector3(m_cursor.transform.position.x, transform.position.y, m_cursor.transform.position.z);
                Destroy(m_cursor);
                m_botMove.isGrabbed = false;
                m_cursorOut = false;
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
}
