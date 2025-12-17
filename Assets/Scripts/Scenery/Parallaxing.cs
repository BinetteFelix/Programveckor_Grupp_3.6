using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float distanceFromCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * distanceFromCamera;
        float movement = cam.transform.position.x * (1 - distanceFromCamera);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}