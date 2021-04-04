using UnityEngine;

namespace Scripts_Students.botB01.Arena
{
    public class botB01_ArenaManager : MonoBehaviour
    {
        private void OnEnable()
        {
            GameHandler.onBattleStart += InitializeCamerasDelayed;
        }

        private void OnDisable()
        {
            GameHandler.onBattleStart -= InitializeCamerasDelayed;
        }
        
        private void InitializeCamerasDelayed()
        {
            Invoke(nameof(InitializeCameras), 0.1f);
        }
        private void InitializeCameras()
        {
            Debug.Log("Initialization");
            foreach (CameraFollow cam in FindObjectsOfType<CameraFollow>())
            {
                Debug.Log(cam.transform.name);
                cam.offsetCamera = new Vector3(0, 20, -15);
                cam.alsoFollowRotation = true;
            }
        }
    }
}