using UnityEngine;

namespace Scripts_Students.botB01.Arena
{
    public class botB01_SlidingBlock : MonoBehaviour
    {
        public Vector3 Point;
        private Vector3 startPoint;

        [Range(0, 2)] public float Offset;
        [Range(0, 5)] public float PushTime;
        [Range(0, 5)] public float PullTime;
        [Range(0, 5)] public float Delay;
        private float pushTimer = 0;
        private float pullTimer = 0;
        private float delayTimer = 0;

        private void Start()
        {
            startPoint = transform.position;
            delayTimer -= Offset;
        }

        private void Update()
        {
            if (delayTimer < Delay)
                delayTimer += Time.deltaTime;
            else if (pushTimer < PushTime)
            {
                pushTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPoint, Point, pushTimer / PushTime);
            }
            else if (pullTimer < PullTime)
            {
                pullTimer += Time.deltaTime;
                transform.position = Vector3.Lerp(Point, startPoint, pullTimer / PullTime);
            }
            else
                delayTimer = pushTimer = pullTimer = 0;
        }
    }
}