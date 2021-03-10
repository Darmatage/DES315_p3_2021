using System;
using System.Collections;
using UnityEngine;

namespace A09
{
    public class ExtendableBotPart : MonoBehaviour
    {
        public bool IsMoving { get; private set; }

        [SerializeField] private bool startExtended;
        [SerializeField] private bool isPlacedRetracted;
        [SerializeField] private Vector3 extendAmount;
        [SerializeField] private float moveTime;

        private Vector3 _extendedPos, _retractedPos;

        private void Awake()
        {
            var localPos = transform.localPosition;
            
            _extendedPos = isPlacedRetracted
                ? localPos + extendAmount
                : localPos;

            _retractedPos = isPlacedRetracted ? localPos : localPos - extendAmount;

            transform.localPosition = startExtended ? _extendedPos : _retractedPos;
        }

        public void Extend() => StartCoroutine(Move(_retractedPos, _extendedPos));

        public void Retract() => StartCoroutine(Move(_extendedPos, _retractedPos));

        private IEnumerator Move(Vector3 from, Vector3 to)
        {
            IsMoving = true;
            var timer = 0f;

            while (timer <= moveTime)
            {
                transform.localPosition = Vector3.Lerp(from, to, timer / moveTime);
                timer += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = to;
            IsMoving = false;
        }
    }
}