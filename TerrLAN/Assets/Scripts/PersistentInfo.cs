using UnityEngine;
using System.Collections;

public class PersistentInfo : MonoBehaviour {

	public string city;
	public float latitude;
	public float longitude;
	public int buildingNum;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
