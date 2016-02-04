using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectSelectionControl : MonoBehaviour {

    void OnMouseDrag()
    {
        Debug.Log("hi");
        if (GlobalValues.editMode && GUISelection.selectionIsMade)
        {
            foreach (GameObject go in GUISelection.g)
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, (go.transform.position.z - Camera.main.transform.position.z)));
                point.z = go.transform.position.z;
                go.transform.position = point;
            }
        }
    }

    void Update()
    {
        if (GlobalValues.editMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GUISelection.g.Clear();
                GUISelection.selectionIsMade = false;
            }
        }


        foreach (GameObject go in GUISelection.g)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                go.transform.Translate(Vector3.up);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                go.transform.Translate(Vector3.down);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                go.transform.Translate(Vector3.left);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                go.transform.Translate(Vector3.right);
        }
    }
}
