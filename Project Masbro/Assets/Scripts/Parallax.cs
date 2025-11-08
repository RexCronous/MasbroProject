using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxMultiplier = 0.1f;

    private Material parallaxMaterial;
    private Transform cameraParallax;
    private float lastCameraParallaxX;
    private float lastCameraParallaxY;
    void Start()
    {
        SetParallaxCameraParallax();
    }

    void Update()
    {
        if (cameraParallax == null) {
            SetParallaxCameraParallax();
            return;
        }
        float deltaX = cameraParallax.position.x - lastCameraParallaxX;
        float deltaY = cameraParallax.position.y - lastCameraParallaxY;
        parallaxMaterial.mainTextureOffset += new Vector2(deltaX * parallaxMultiplier * 0.005f, deltaY * parallaxMultiplier * 0.005f);
        lastCameraParallaxX = cameraParallax.position.x;
        lastCameraParallaxY = cameraParallax.position.y;
    }
    
    public void SetParallaxCameraParallax()
    {
        parallaxMaterial = GetComponent<Renderer>().material;
        if (GameObject.FindGameObjectWithTag("MainCamera") == null) return;
        cameraParallax = GameObject.FindGameObjectWithTag("MainCamera").transform;
        lastCameraParallaxX = cameraParallax.position.x;
        lastCameraParallaxY = cameraParallax.position.y;
    }
}
