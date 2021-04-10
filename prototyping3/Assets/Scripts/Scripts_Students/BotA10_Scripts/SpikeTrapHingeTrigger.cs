
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class SpikeTrapHingeTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject spikeTrap;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.CompareTag("Player1") || other.transform.root.CompareTag("Player2"))
            {
                spikeTrap.GetComponent<SpikeTrapHinge>().TriggerOn();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.CompareTag("Player1") || other.transform.root.CompareTag("Player2"))
            {
                spikeTrap.GetComponent<SpikeTrapHinge>().TriggerOff();
            }
        }
    }
}
