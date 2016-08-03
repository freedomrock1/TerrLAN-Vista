using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WorldViewManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			SceneManager.LoadScene ("City Map");
		}
	}
}
