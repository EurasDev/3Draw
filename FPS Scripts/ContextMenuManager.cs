using UnityEngine;
using System.Collections;

public class ContextMenuManager : MonoBehaviour {

    public static GameObject g;

    public void Awake()
    {
        g = GameObject.FindGameObjectWithTag("context-menu");
        g.SetActive(false);
    }

    public static void ToggleMenu()
    {
        if(GlobalValues.editMode)
        {
            g.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            if (!g.activeInHierarchy)
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }
    }
}
