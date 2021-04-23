using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{
    [SerializeField] private float risingSpeed;
    [SerializeField] private float timeBeforeRising;
    [SerializeField] private float maxRiseAmount;
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private GameObject p1Parent;
    [SerializeField] private GameObject p2Parent;

    private GameObject _p1Bot, _p2Bot;
    private float _startHeight, _maxHeight;

    private void OnValidate()
    {
        if (!gameHandler)
            gameHandler = FindObjectOfType<GameHandler>();
        
        if (!p1Parent)
            p1Parent = GameObject.Find("PLAYER1_SLOT");
        
        if (!p2Parent)
            p2Parent = GameObject.Find("PLAYER2_SLOT");
    }

    private void Awake()
    {
        _startHeight = transform.position.y;
        _maxHeight = _startHeight + maxRiseAmount;
    }

    private void Update()
    {
        if (!_p1Bot && gameHandler.isGameTime)
        {
            _p1Bot = p1Parent.transform.GetChild(0).gameObject;
            _p2Bot = p2Parent.transform.GetChild(0).gameObject;
            StartCoroutine(DoLavaRising());
        }
    }

    private IEnumerator DoLavaRising()
    {
        yield return new WaitForSeconds(timeBeforeRising);

        var height = transform.lossyScale.y / 2f;
        
        while (transform.position.y <= _maxHeight)
        {
            var newPosition = transform.position;
            newPosition.y += risingSpeed * Time.deltaTime;

            var surfaceHeight = newPosition.y + height;
            
            if (surfaceHeight < GetHighestBotYPos())
                transform.position = newPosition;
            
            yield return new WaitForEndOfFrame();
        }

        transform.position = new Vector3(transform.position.x, _maxHeight, transform.position.z);
    }

    private float GetHighestBotYPos()
    {
        var p1Y = _p1Bot.transform.position.y;
        var p2Y = _p2Bot.transform.position.y;

        return (p1Y > p2Y ? p1Y : p2Y) - 5f;
    }
}
