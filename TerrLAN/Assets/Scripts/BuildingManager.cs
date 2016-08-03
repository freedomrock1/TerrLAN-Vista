using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BuildingManager : MonoBehaviour {

	public int buildingNum;

	GameObject info;
	info myInfo;

	// Use this for initialization
	void Start () {
		info = GameObject.Find ("info");
		myInfo = info.GetComponent<info> ();
	}

	void OnMouseDown()
	{
		SceneManager.LoadScene ("lan-vista");
		myInfo.buildingNum = buildingNum;
	}
}
