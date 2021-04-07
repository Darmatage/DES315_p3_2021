using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{
    [SerializeField] private float risingSpeed;
    [SerializeField] private float timeBeforeRising;
    [SerializeField] private float maxRiseAmount;

    private float _startHeight, _maxHeight;
    
    private void Awake()
    {
        _startHeight = transform.position.y;
        _maxHeight = _startHeight + maxRiseAmount;
        
        StartCoroutine(DoLavaRising());
    }

    private IEnumerator DoLavaRising()
    {
        yield return new WaitForSeconds(timeBeforeRising);

        while (transform.position.y <= _maxHeight)
        {
            var newPosition = transform.position;
            newPosition.y += risingSpeed * Time.deltaTime;
            transform.position = newPosition;
            
            yield return new WaitForEndOfFrame();
        }

        transform.position = new Vector3(transform.position.x, _maxHeight, transform.position.z);
    }
}
