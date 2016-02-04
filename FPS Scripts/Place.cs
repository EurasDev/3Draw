using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.EventSystems;

public class Place : MonoBehaviour {

	#region variables
	RaycastHit hit;
	public int mapSize = 50;
	public static int blockCount;

	public Material floorMat;
	public Material blockMat;
	public Material[] blockMats;

	GameObject blockGroup;
	GameObject floorBlocks;
	GameObject buildBlocks;

	public GameObject[] blocks;
	public static Texture2D[] tiles = new Texture2D[20];

	#endregion

	#region initialisation
	private void Start (){
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i] = (Texture2D)(Resources.Load ("gui-blocks/" + (i + 1), typeof(Texture2D)));
		}

		blockGroup = new GameObject ("All Blocks");
		floorBlocks = new GameObject ("Floor Blocks");
		buildBlocks = new GameObject ("Building Blocks");
		floorBlocks.transform.parent = blockGroup.transform;
		buildBlocks.transform.parent = blockGroup.transform;
		blockGroup.tag = "blockGroup";

		for (int x = 0; x < mapSize; x++) {
			for (int y = 0; y < mapSize; y++) {
				GameObject block = GameObject.CreatePrimitive (PrimitiveType.Cube);

				block.transform.name = "Floor Block";
				block.transform.parent = floorBlocks.transform;
				block.GetComponent<Renderer> ().material = floorMat;
				block.AddComponent<Highlight> ();
				block.tag = "floorBlock";
				block.transform.localPosition = new Vector3 (x - (mapSize / 2), 0, y - (mapSize / 2));
			}
		}
	}
    #endregion

    #region main Update
    public bool placeBlock;
    public bool eraseBlock;

    private void Update (){
		if (Input.GetMouseButtonDown (0) && !GlobalValues.editMode && !CameraRotation.isMoving && !EventSystem.current.IsPointerOverGameObject())
        {
            placeBlock = !placeBlock;
		}
        if (Input.GetMouseButtonUp(1))
        {
            eraseBlock = !eraseBlock;
        }
    }

    void FixedUpdate()
    {
        if (placeBlock)
        {
            Build();
            placeBlock = !placeBlock;
        }
        if (eraseBlock)
        {
            Erase();
            eraseBlock = !eraseBlock;
        }
    }

    #endregion

    public static int blockType = 1;
	public static List<GameObject> lastBlocksPlaced = new List<GameObject>();
	
	#region build
	private void Build (){
		if (HitBlock ()) {
			GameObject block = GameObject.CreatePrimitive (PrimitiveType.Cube);
						
			block.transform.name = "Build Block";
			block.transform.parent = buildBlocks.transform;
			block.tag = "buildBlock";
			
			block.AddComponent<Highlight> ();
			block.AddComponent<GlobalValues> ();
            Material newMat = new Material(Shader.Find("Legacy Shaders/Self-Illumin/Bumped Diffuse"));
            newMat.color = new Color32(148, 148, 148, 255);
            block.GetComponent<Renderer>().material = newMat;
            block.GetComponent<GlobalValues> ().i = blockType;
			newMat.mainTexture = tex ();

			Vector3 cursorPosition = hit.transform.position + hit.normal;
			block.transform.position = cursorPosition;
			blockCount++;
			lastBlocksPlaced.Add (block);
		}
	}
	#endregion

	#region erase
	private void Erase (){ 
		if (HitBlock ()) {
			if (hit.transform.tag == "buildBlock") {
				Destroy (hit.transform.gameObject);
				blockCount--;

				GameObject block = hit.transform.gameObject;
				lastBlocksPlaced.Remove (block);
            }
            else if (hit.transform.tag != "buildBlock")
            {
                ContextMenuManager.ToggleMenu();
            }
        }
	}
	#endregion
	
	public bool HitBlock() { 
		Ray posRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		return Physics.Raycast(posRay, out hit, Mathf.Infinity);
	}

    public bool blockHighlighted = false;

	#region highlighting
	private void HighlightBlock ()
	{
		if (HitBlock ()) {
			if (hit.transform.tag != "floorBlock") {
				blockHighlighted = true;
				GameObject highlightCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				highlightCube.transform.position = hit.transform.position;
				highlightCube.transform.localScale = new Vector3 (1.1f, 1.1f, 1.1f);
				highlightCube.transform.tag = "highlightBlock";

				Color32 c = new Color32 (255, 255, 255, 30);
				Renderer r = highlightCube.transform.GetComponent<Renderer> ();
				r.material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				r.material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				r.material.SetInt ("_ZWrite", 0);
				r.material.DisableKeyword ("_ALPHATEST_ON");
				r.material.DisableKeyword ("_ALPHABLEND_ON");
				r.material.EnableKeyword ("_ALPHAPREMULTIPLY_ON");
				r.material.renderQueue = 3000;
				r.material.color = c;
			}
		}
	}
    #endregion

	#region save data
	public void Save (){
        ContextMenuManager.ToggleMenu();
        DirectoryInfo d = new DirectoryInfo (Application.persistentDataPath + @"\Data\");
		foreach (FileInfo file in d.GetFiles()) {
			file.Delete (); 
		}
						
		for (int i = 0; i < blockCount; i++) {
			blocks = GameObject.FindGameObjectsWithTag ("buildBlock");

			BinaryFormatter bf = new BinaryFormatter ();
			Directory.CreateDirectory (Application.persistentDataPath + @"\Data\");
			FileStream file = File.Create (Application.persistentDataPath + @"\Data\block-" + i + ".dat");

            worldData data = new worldData ();

			data._name = blocks [i].name;
			data._tag = blocks [i].tag;
			data.x = blocks [i].transform.position.x;
			data.y = blocks [i].transform.position.y;
			data.z = blocks [i].transform.position.z;
			data.bType = blocks [i].GetComponent<GlobalValues> ().i;
			bf.Serialize (file, data);
            
			file.Close ();
		}
	}
	#endregion

	#region load data
	public void Load (string dir){
        ContextMenuManager.ToggleMenu();
        DirectoryInfo d = new DirectoryInfo (Application.persistentDataPath + @"\" + dir + @"\");
		int count = d.GetFiles ().Length;

		for (int i = 1; i < count + 1; i++) {
			if (File.Exists (Application.persistentDataPath + @"\" + dir + @"\block-" + i + ".dat")) {			
				blockMats = new Material[count];
				blocks = new GameObject[count];
				blockMats [i] = new Material ((Material)Resources.Load ("colour_tile", typeof(Material)));

				BinaryFormatter bf = new BinaryFormatter ();	
				FileStream file = File.Open (Application.persistentDataPath + @"\" + dir + @"\block-" + i + ".dat", FileMode.Open);	

				worldData data = (worldData)bf.Deserialize (file);			
				file.Close ();			
			    
				blocks [i] = GameObject.CreatePrimitive (PrimitiveType.Cube);	
				blocks [i].name = data._name;
				blocks [i].tag = data._tag;
				blocks [i].transform.parent = buildBlocks.transform;

				float x = data.x;		
				float y = data.y;
				float z = data.z;

				blocks [i].transform.position = new Vector3 (x, y, z);

				blocks [i].AddComponent<Highlight> ();
				blocks [i].AddComponent<GlobalValues> ();
				blocks [i].GetComponent<Renderer> ().material = blockMats [i];
				blocks [i].GetComponent<GlobalValues> ().i = data.bType;
				blockType = data.bType;
				blockMats [i].mainTexture = tex ();
				blockCount++;
			}
		}
	}
    #endregion

    public string str;
    public void Export()
    {
        ContextMenuManager.ToggleMenu();
        Transform t;
        blocks = GameObject.FindGameObjectsWithTag("buildBlock");

        for (int i = 0; i < blockCount; i++)
        {
            t = blocks[i].transform;
            str += ObjExporter.MeshToString(blocks[i].GetComponent<MeshFilter>(), t);
        }
        ObjExporter.StringToOBJ(str, Application.persistentDataPath + @"\OBJ_Exports\" + "World-" + DateTime.Now.ToString("ddMMyy-HHmm") + ".obj");
    }
	
	#region textures
	private static Texture2D tex (){
		Texture2D texture;
		switch (blockType) {
		default:
			texture = tiles [0];
			break;
		case 2:
			texture = tiles [1];
			break;
		case 3:
			texture = tiles [2];
			break;
		case 4:
			texture = tiles [3];
			break;
		case 5:
			texture = tiles [4];
			break;
			
		case 6:
			texture = tiles [5];
			break;
		case 7:
			texture = tiles [6];
			break;
		case 8:
			texture = tiles [7];
			break;
		case 9:
			texture = tiles [8];
			break;
		case 10:
			texture = tiles [9];
			break;
			
		case 11:
			texture = tiles [10];
			break;
		case 12:
			texture = tiles [11];
			break;
		case 13:
			texture = tiles [12];
			break;
		case 14:
			texture = tiles [13];
			break;
		case 15:
			texture = tiles [14];
			break;
			
		case 16:
			texture = tiles [15];
			break;
		case 17:
			texture = tiles [16];
			break;
		case 18:
			texture = tiles [17];
			break;
		case 19:
			texture = tiles [18];
			break;
		case 20:
			texture = tiles [19];
			break;
		}
		return texture;
	}
	#endregion

}

#region world Data Class
[Serializable]
class worldData
{
	public float x;
	public float y;
	public float z;
	public string _name;
	public string _tag;
	public int bType;
}
#endregion

