using System;
using System.Collections;
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
	public class SpikeTrap : MonoBehaviour
	{
		[SerializeField] private GameObject spikes;
		[SerializeField] private GameObject theButton;
		[SerializeField] private float thrustAmount = 0.1f;
		[SerializeField] private float spikeWithdrawTime = 1.5f;
		private bool _spikeOut = false;
		private bool _buttonPressed = false;
		private float _theButtonUpPos;
		private Renderer _buttonRend;

		private void Start()
		{
			if (theButton == null) return;
			_theButtonUpPos = theButton.transform.position.y;
			_buttonRend = theButton.GetComponent<Renderer>();
		}
		
    	private void FixedUpdate()
    	{
		    // move Spikes
			if (!_spikeOut && _buttonPressed)
			{
				spikes.transform.Translate(0,thrustAmount, 0);
				_spikeOut = true;
				StartCoroutine(WithdrawSpikes());
			}
    	}
		
		public void OnTriggerEnter(Collider other)
		{
			if ((other.transform.root.gameObject.tag == "Player1") || (other.transform.root.gameObject.tag == "Player2"))
			{
				_buttonPressed = true;
				var position = theButton.transform.position;
				position = new Vector3(position.x, _theButtonUpPos - 0.4f, position.z);
				theButton.transform.position = position;
				var buttonRend = theButton.GetComponent<Renderer>();
				buttonRend.material.color = new Color(2.0f, 0.5f, 0.5f, 2.5f); 
			}
		}
		
		private void ButtonUp()
		{
			var position = theButton.transform.position;
			position = new Vector3(position.x, _theButtonUpPos, position.z);
			theButton.transform.position = position;
			_buttonRend.material.color = Color.white;
			_buttonPressed = false;
		}
		
		private IEnumerator WithdrawSpikes()
		{
			yield return new WaitForSeconds(spikeWithdrawTime);
			spikes.transform.Translate(0,-thrustAmount, 0);
			_spikeOut = false;
			ButtonUp();
		}	
	}
}
