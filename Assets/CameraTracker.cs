using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTracker : MonoBehaviour
{
    Camera cam;
    [SerializeField] float zoomTarget;
    [SerializeField] float speed = 2f;
    void Start()
    {
        cam = GetComponent<Camera>();
        zoomTarget = cam.orthographicSize;
    }

    void Update()
    {
        var position = Player.instance.transform.position;
        cam.transform.position = new Vector3(position.x, position.y, -10);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, Time.deltaTime*10);
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            zoomTarget -= context.ReadValue<Vector2>().y * speed * Time.deltaTime;
            zoomTarget = Mathf.Clamp(zoomTarget, 5, 10);
        }
    }
    
    void FixedUpdate()
    {
        
    }
}
