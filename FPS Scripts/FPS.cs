using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {

	public AudioClip debugMenu;
	bool isDebug = false;

	int frameCount = 0;
	float dt = 0.0f;
	float fps = 0.0f;
	float updateRate = 4.0f;

	private void Update () {
		if(Input.GetKeyDown(KeyCode.F3)){
			if(isDebug == true){
				isDebug = false;
			}else{
				isDebug = true;
				GetComponent<AudioSource>().PlayOneShot(debugMenu, 1);
			}
		}
		StartCoroutine(getFPS());
	}

	private IEnumerator getFPS(){
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1.0/updateRate){
			fps = frameCount / dt;
			frameCount = 0;
			dt -= 1.0f/updateRate;
		}
		yield return true;
	}

	private void OnGUI(){
		if(isDebug){
			GUI.Label(new Rect(20, Camera.main.pixelHeight - 65, 100, 20), "FPS: " + fps);
			GUI.Label(new Rect(20, Camera.main.pixelHeight - 50, 100, 20), "Block Count: " + Place.blockCount);
		}
	}
}
