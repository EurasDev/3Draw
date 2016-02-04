using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public static GameObject clearMenu;

    public void Awake()
    {
        clearMenu = GameObject.FindGameObjectWithTag("deleteBlocksMenu");
        clearMenu.SetActive(false);
    }

    public void EraseBlocks()
    {
        clearMenu.SetActive(true);
    }

    public void clearYesButton()
    {
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (go.name == "Build Block")
            {
                Destroy(go.transform.gameObject);
                Place.blockCount = 0;
                clearMenu.SetActive(false);
            }
        }
    }

    public void clearNoButton()
    {
        clearMenu.SetActive(false);
        return;
    }

    private int ObjectCount; 
    public void Undo()
    {
        ObjectCount = Place.lastBlocksPlaced.Count;
        if (Place.lastBlocksPlaced.Count > -1)
            {
                Destroy(Place.lastBlocksPlaced[ObjectCount - 1].transform.gameObject);
                Place.lastBlocksPlaced.RemoveAt(ObjectCount - 1);
                Place.blockCount--;
            }
    }

    public static bool isClicked;
    public GameObject g;

    public void SelectColour()
    {
        g = ColourGrid.gO;
        if (Place.tiles != null && !g.activeInHierarchy)
        {
            g.SetActive(true);
        }
        else
        {
            g.SetActive(false);
        }
    }

    public void EditMode()
    {
        GlobalValues.editMode = !GlobalValues.editMode;
    }
}


