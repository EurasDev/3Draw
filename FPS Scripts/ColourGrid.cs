using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColourGrid : MonoBehaviour {

    public static GameObject gO;
    public int ColourIndex = 0;

    void Awake()
    {
        gO = transform.gameObject;
        transform.gameObject.SetActive(false);

        Texture2D[] tx = new Texture2D[Place.tiles.Length];

        for (int i = 0; i < Place.tiles.Length; i++)
        {
            GameObject g = new GameObject();
            g.transform.parent = transform;
            g.name = "tile[" + i + "]";

            tx[i] = (Texture2D)Resources.Load("gui-blocks/" + (i + 1), typeof(Texture2D));
            g.AddComponent<Image>().sprite = Sprite.Create(tx[i], new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
            g.AddComponent<Button>().targetGraphic = g.GetComponent<Image>();
            g.AddComponent<GlobalValues>().i = ColourIndex++;
            g.GetComponent<Button>().onClick.AddListener(() => SetColour(g.GetComponent<GlobalValues>().i));
        }
    }

    public void SetColour(int i)
    {
        Place.blockType = i + 1;
        transform.gameObject.SetActive(false);
    }
}
