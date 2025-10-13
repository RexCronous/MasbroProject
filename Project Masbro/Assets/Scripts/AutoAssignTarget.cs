using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class AutoAssignTarget : MonoBehaviour
{
    public CinemachineCamera cineCam;
    private CinemachineConfiner2D confiner2D;
    private Transform playerTransform;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayer();
    }

    void Update()
    {
        // Jika player hilang (mati / reload scene), cari lagi
        if (playerTransform == null)
            AssignPlayer();
    }

    private void AssignPlayer()
    {
        // Cari player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        // Cari confiner di scene baru
        // GameObject confinerObj = GameObject.FindGameObjectWithTag("CameraConfiner");
        // if (confinerObj != null)
        //     confiner2D = confinerObj.GetComponent<CinemachineConfiner2D>();

        // Update target Cinemachine
        if (cineCam != null && playerTransform != null)
        {
            cineCam.Target.TrackingTarget = playerTransform;
            cineCam.Target.LookAtTarget = playerTransform;
        }

        // Update confiner bounding shape
        // if (confiner2D != null)
        // {
        //     confiner2D.InvalidateBoundingShapeCache();
        // }
    }
}
