using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
	
    public static bool isMoving;

    public bool rightClick;
    public bool middleClick;

	void Awake(){
		Screen.SetResolution(1920, 1080, true);
	}

    void Update()
    {
        if (!GlobalValues.editMode)
        {
            if(rightClick)
            {
                transform.RotateAround(Vector3.zero, Vector3.up, Input.GetAxis("Mouse X") * 1f);
            }
            if(middleClick)
            {
                transform.Translate(new Vector3(-Input.GetAxis("Mouse X") * 0.5f, -Input.GetAxis("Mouse Y") * 0.5f, 0), Space.Self);
            }

            if (Input.GetMouseButtonUp(2))
            {
                middleClick = false;
            }

            if (Input.GetMouseButtonDown(2))
            {
                middleClick = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                rightClick = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                isMoving = true;
                rightClick = true;
            }
            else
            {
                isMoving = false;
            }

            Camera.main.transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * 5.0F), Space.Self);            
        }
    }
}
