using UnityEngine;
using System.Collections;

public class GridToggle : MonoBehaviour {

    public GameObject g;

    public void Awake()
    {
        g = transform.gameObject;
        g.SetActive(false);
    }

    public void ToggleGrid()
    {
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
