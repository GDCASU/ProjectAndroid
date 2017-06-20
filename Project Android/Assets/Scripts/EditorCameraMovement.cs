using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Developer:   Kyle Aycock
// Date:        6/16/2017
// Description: Simple camera movement script to make navigation in the Map Editor nicer

public class EditorCameraMovement : MonoBehaviour {

    public float speed;

    private GameObject canvas;

    void Update()
    {
        if (canvas == null)
            canvas = GameObject.Find("Canvas");
        else
        {

            foreach (InputField i in canvas.GetComponentsInChildren<InputField>())
                if (i.isFocused) return;
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            float moveZoom = Input.GetAxis("Mouse ScrollWheel");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * Time.deltaTime * (transform.position.y / 10f);
            movement += transform.forward * moveZoom;
            transform.position += movement * speed;
        }
    }
}
