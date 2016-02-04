using System;
using UnityEngine;
using System.Collections.Generic;

public class GUISelection : MonoBehaviour
{
	public RectTransform selectionBox;
	private Vector2 initialClickPosition = Vector2.zero;
    public static List<GameObject> g = new List<GameObject>();

	public Vector2 currentMousePosition;
    public Vector2 difference;
    public Vector2 startPoint;

    public Rect r;

    public static bool selectionIsMade = false;

    void Update()
	{
        if(g.Count > 0)
        {
            selectionIsMade = true;
        }
        else
        {
            selectionIsMade = false;
        }

		if (GlobalValues.editMode && !selectionIsMade)
        {

            if (Input.GetMouseButtonDown(0))
			{
                g.Clear();
				initialClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				selectionBox.anchoredPosition = initialClickPosition;
            }

            if (Input.GetMouseButton(0))
			{
                currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                difference = currentMousePosition - initialClickPosition;
                startPoint = initialClickPosition;

                if (difference.x < 0)
				{
					startPoint.x = currentMousePosition.x;
					difference.x = -difference.x;
				}
				if (difference.y < 0)
				{
					startPoint.y = currentMousePosition.y;
					difference.y = -difference.y;
				}
				
				selectionBox.anchoredPosition = startPoint;
				selectionBox.sizeDelta = difference;

            }
			
            
			if (Input.GetMouseButtonUp(0))
			{
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("buildBlock"))
                {
                    var screenCoordinates = Camera.main.WorldToScreenPoint(go.transform.position);
                    screenCoordinates.z = 0;

                    r = new Rect(selectionBox.position.x, selectionBox.position.y, selectionBox.rect.width, selectionBox.rect.height);

                    if (r.Contains(screenCoordinates, true))
                    {
                        g.Add(go);
                    }
                }

                initialClickPosition = Vector2.zero;
				selectionBox.anchoredPosition = Vector2.zero;
				selectionBox.sizeDelta = Vector2.zero;
            }
        }
    }

}