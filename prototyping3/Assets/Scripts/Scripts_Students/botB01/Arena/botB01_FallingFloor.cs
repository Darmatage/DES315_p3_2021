using UnityEngine;

namespace Scripts_Students.botB01.Arena
{
    public class botB01_FallingFloor : MonoBehaviour
    {
        [Range(0, 10)] public float FallTime;
        private float fallTimer;
        [Range(0, 10)] public float FallDelay;
        private int state = 0;

        private Vector3 spawnPos;
        private void Start()
        {
            fallTimer = 0.0f;
            spawnPos = transform.position;
        }

        private void OnTriggerStay(Collider other)
        {
            if (state == 0 && other.transform.root.tag.Contains("Player"))
                IncreaseState();
        }

        private void Update()
        {
            if (state == 1)
            {
                GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.5f);
                Invoke(nameof(IncreaseState), FallDelay);
            }

            else if (state >= 2)
            {
                fallTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(spawnPos, new Vector3(spawnPos.x, 0, spawnPos.z), 
                    Mathf.Pow(fallTimer / FallTime, 2));
            }
        }

        private void IncreaseState()
        {
            state++;
        }
    }
}