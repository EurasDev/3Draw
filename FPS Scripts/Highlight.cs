using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	Material preMat;
	public bool isEnabled = true;

	private void OnMouseEnter(){
		if(isEnabled){
			preMat = transform.GetComponent<Renderer>().material;
			transform.GetComponent<Renderer>().material = (Material)Resources.Load("Select", typeof(Material));
		}
	}
	
	private void OnMouseExit(){	
		if(isEnabled){
			transform.GetComponent<Renderer>().material = preMat;
		}
	}

}
