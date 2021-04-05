
using System.Collections;
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
	public class SpikeTrap : MonoBehaviour
	{
		[SerializeField] private GameObject spikes;
		[SerializeField] private float thrustAmount = 0.1f;
		[SerializeField] private float spikeWithdrawTime = 1.5f;
		[SerializeField] private float spikeDelayTime = 0.2f;
		private bool _spikeOut = false;
		
		public void OnTriggerEnter(Collider other)
		{
			if ((!other.transform.root.gameObject.CompareTag("Player1")) &&
			    (!other.transform.root.gameObject.CompareTag("Player2"))) return;
			if (_spikeOut) return;
			StartCoroutine(SpikesOut());
			StartCoroutine(WithdrawSpikes());
		}

		private IEnumerator SpikesOut()
		{
			_spikeOut = true;
			yield return new WaitForSeconds(spikeDelayTime);
			spikes.transform.Translate(0, thrustAmount, 0);
		}
		
		private IEnumerator WithdrawSpikes()
		{
			yield return new WaitForSeconds(spikeWithdrawTime);
			spikes.transform.Translate(0,-thrustAmount, 0);
			_spikeOut = false;
		}	
	}
}
