using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Camera cam;
    public Transform player;
  
    private Vector2 startPos;
    private Vector2 Travel => (Vector2)cam.transform.position - startPos;

    private float startZPos;
    private float DistanceFromPlayer => transform.position.z - player.position.z;
    private float ClippingPlane => (cam.transform.position.z + (DistanceFromPlayer > 0 ? cam.farClipPlane : cam.nearClipPlane));
    private float parallaxFactor => Mathf.Abs(DistanceFromPlayer) / ClippingPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        startZPos = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 updatePosition = startPos + Travel * parallaxFactor;
        transform.position = new Vector3(updatePosition.x, updatePosition.y, startZPos);
    }
}