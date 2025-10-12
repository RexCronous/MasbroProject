using UnityEngine;
using Unity.Cinemachine;

public class AutoAssignPlayer : MonoBehaviour
{
    public CinemachineCamera cineCam;
    private Transform playerTransform;

    void Update()
    {
        // Jika player belum ditemukan, cari dengan tag "Player"
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy)
        {
            playerTransform = null;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            if (player != null)
            { 
                playerTransform = player.transform;

                // Set Cinemachine target ke player
                if (cineCam != null)
                {
                    cineCam.Target.TrackingTarget = playerTransform;
                    cineCam.Target.LookAtTarget = playerTransform;
                }
            }
        }
    }
}
