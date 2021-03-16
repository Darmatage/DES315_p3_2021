using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, ps.main.duration);
    }
}
