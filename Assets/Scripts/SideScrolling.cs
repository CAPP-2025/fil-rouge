using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrolling : MonoBehaviour
{
    private new Camera camera;
    public Transform player;

    public float height = 6.5f;
    public float undergroundHeight = -9.5f;
    public float undergroundThreshold = 0f;
    public GameObject cameraSpace; //the limit of the camera

    private void Awake()
    {
        camera = GetComponent<Camera>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        // track the player
        Vector3 cameraPosition = transform.position;
        //clamp the camera to the border of the cameraSpace so the edge of the camera will be at the edge of the cameraSpace, cameraSpace is a SpriteRenderer with a width
        cameraPosition.x = Mathf.Clamp(player.position.x, cameraSpace.transform.position.x - cameraSpace.GetComponent<SpriteRenderer>().bounds.size.x / 2 + camera.orthographicSize * camera.aspect, cameraSpace.transform.position.x + cameraSpace.GetComponent<SpriteRenderer>().bounds.size.x / 2 - camera.orthographicSize * camera.aspect);
        //cameraPosition.x = player.position.x;
        transform.position = cameraPosition;
    }

    public void SetUnderground(bool underground)
    {
        // set underground height offset
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        transform.position = cameraPosition;
    }

}
