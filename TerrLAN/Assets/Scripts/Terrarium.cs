using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terrarium : MonoBehaviour {

	//Variable to hold the current location of the player
	Transform player;
	//List of locations
	List<Vector3> locList;
	//List of model numbers
	List<int> modList;

	//Prefabs to be spawned at the locations
	public GameObject model1, model2, model3, model4, model5;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").transform;
		locList = new List<Vector3> ();
		modList = new List<int> ();

		string hold = PlayerPrefs.GetString ("LocSpawn");
		if (hold != null) {
			string[] holdList = hold.Split (',');
			int i = 0;
			while (i < holdList.Length) {
				float x, y, z;
				x = float.Parse(holdList [i]);
				i++;
				y = float.Parse(holdList [i]);
				i++;
				z = float.Parse(holdList [i]);
				i++;
				locList.Add (new Vector3(x, y, z));
				modList.Add (int.Parse(holdList[i]));
				i++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		//If the first button is pressed...
		if (Input.GetButtonDown ("Spawn1")) {
			locList.Add (player.transform.position);
			modList.Add (1);
		}
		//If the second button is pressed...
		if (Input.GetButtonDown ("Spawn2")) {
			locList.Add (player.transform.position);
			modList.Add (2);
		}
		//If the third button is pressed...
		if (Input.GetButtonDown ("Spawn3")) {
			locList.Add (player.transform.position);
			modList.Add (3);
		}
		//If the fourth button is pressed...
		if (Input.GetButtonDown ("Spawn4")) {
			locList.Add (player.transform.position);
			modList.Add (4);
		}
		//If the fifth button is pressed...
		if (Input.GetButtonDown ("Spawn5")) {
			locList.Add (player.transform.position);
			modList.Add (5);
		}

		//If the fifth button is pressed...
		if (Input.GetButtonDown ("SpawnAll")) {
			if (locList.Count != 0) {
				for (int i = 0; i < locList.Count; i++) {


					switch (modList [i]) {
					case 1:
						Instantiate (model1, locList [i], transform.rotation);
						break;
					case 2:
						Instantiate (model2, locList [i], transform.rotation);
						break;
					case 3:
						Instantiate (model3, locList [i], transform.rotation);
						break;
					case 4:
						Instantiate (model4, locList [i], transform.rotation);
						break;
					case 5:
						Instantiate (model5, locList [i], transform.rotation);
						break;
					}
				}
			}
		}
		//If the fifth button is pressed...
		if (Input.GetButtonDown ("SaveSpawn")) {
			string hold = "";
			for (int i = 0; i < locList.Count; i++) {
				hold += locList [i].x + "," + locList [i].y + "," + locList [i].z + "," + modList[i] + ",";
			}
			PlayerPrefs.SetString ("LocSpawn", hold);
		}
	}
}
