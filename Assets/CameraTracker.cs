using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTracker : MonoBehaviour
{
    Camera cam;
    [SerializeField] float zoomTarget;
    [SerializeField] float speed = 2f;
    [SerializeField] Vector2Int zoomRange = new Vector2Int(5, 10);
    [SerializeField] Vector2Int maxBounds = new Vector2Int(30,30);
    void Start()
    {
        cam = GetComponent<Camera>();
        zoomTarget = cam.orthographicSize;
    }

    void Update()
    {
        var position = Player.instance.transform.position;

        // cam.aspect * cam.orthographicSize = width of camera
        // cam.orthographicSize = height of camera
// if camera is out of bounds, move it back in bounds
        if (position.x - cam.aspect * cam.orthographicSize < -maxBounds.x)
        {
            position.x = -maxBounds.x + cam.aspect * cam.orthographicSize;
        }
        if (position.x + cam.aspect * cam.orthographicSize > maxBounds.x+1)
        {
            position.x = maxBounds.x +1 - cam.aspect * cam.orthographicSize;
        }
        if (position.y - cam.orthographicSize < -maxBounds.y)
        {
            position.y = -maxBounds.y + cam.orthographicSize;
        }
        if (position.y + cam.orthographicSize > maxBounds.y +1)
        {
            position.y = maxBounds.y +1 - cam.orthographicSize;
        }

        cam.transform.position = new Vector3(position.x, position.y, -10);
        
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, Time.deltaTime*10);
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            zoomTarget -= context.ReadValue<Vector2>().y * speed * Time.deltaTime;
            zoomTarget = Mathf.Clamp(zoomTarget, zoomRange.x,zoomRange.y);
        }
    }
    
    void FixedUpdate()
    {
        
    }
}
