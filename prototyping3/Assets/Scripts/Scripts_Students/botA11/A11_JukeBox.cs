using System.Collections.Generic;
using UnityEngine;

namespace Amogh
{
    public class A11_JukeBox : MonoBehaviour, A11_IAttack
    {
        public AudioClip[] allClips;

        public AudioSource source;
        public GameObject discoBall;

        public MeshRenderer bodyMesh;
        private Material bodyMat;
        
        private int currIndex;
        private List<GameObject> allBalls;
        void Start()
        {
            bodyMat = bodyMesh.material;
            allBalls = new List<GameObject>();
        }
        
        void Update()
        {

        }

        private void SpawnDiscoBall()
        {
            GameObject currBall = Instantiate(discoBall, transform.position + Vector3.up * 2f + Random.onUnitSphere, Random.rotationUniform);
            var rb = currBall.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.down * 2f);
            rb.AddTorque(Vector3.left);
            //Destroy(currBall, 1.5f);
            
            allBalls.Add(currBall);
        }
        
        public void ButtonDown()
        {
            currIndex = Random.Range(0, allClips.Length);
            //++currIndex;
            //currIndex %= allClips.Length;
            
            source.clip = allClips[currIndex];
            source.Play();
            
            bodyMat.SetColor("_EmissionColor", Color.white);

            InvokeRepeating(nameof(SpawnDiscoBall), 0.1f, 0.25f);
        }
        
        public void ButtonHeld()
        {
            
        }

        public void ButtonUp()
        {
            source.Stop();
            CancelInvoke(nameof(SpawnDiscoBall));
            
            bodyMat.SetColor("_EmissionColor", Color.clear);

            foreach (var ball in allBalls)
            {
                Destroy(ball);
            }
            allBalls.Clear();
        }
        
    }
}