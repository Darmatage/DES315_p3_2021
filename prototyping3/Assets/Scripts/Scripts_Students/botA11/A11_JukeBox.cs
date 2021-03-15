using System.ComponentModel;
using UnityEngine;

namespace Amogh
{
    public class A11_JukeBox : MonoBehaviour, A11_IAttack
    {
        public AudioClip[] allClips;

        public AudioSource source;
        public GameObject discoBall;

        private GameObject currBall;
        private int currIndex;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void ButtonDown()
        {
            currIndex = Random.Range(0, allClips.Length);
            source.clip = allClips[currIndex];
            source.Play();
            //currBall = Instantiate(discoBall, transform.position, Quaternion.identity);
        }
        
        public void ButtonHeld()
        {
            
        }

        public void ButtonUp()
        {
            source.Stop();
            //Destroy(currBall);
        }
        
    }
}