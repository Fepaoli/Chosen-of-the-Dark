using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InGameCamera : MonoBehaviour
{
    private Vector3 movement;
    int minZoom = 1;
    int maxZoom = 7;
    int sensitivity = 5;
    public float zoom;
    // Update is called once per frame
    void Update()
    {
        //Camera move
        movement = new Vector3 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);
        gameObject.transform.position += movement*Time.deltaTime*5;

        //Camera zoom
        zoom = Camera.main.orthographicSize;
        zoom -= Input.GetAxisRaw("Mouse ScrollWheel")*sensitivity;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Camera.main.orthographicSize = zoom;
    }
}
