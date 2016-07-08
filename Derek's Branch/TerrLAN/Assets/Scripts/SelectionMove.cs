using UnityEngine;
using System.Collections;

public class SelectionMove : MonoBehaviour {

	//How fast the object is to move
	public float speed = 5;
	//Floats to hold the values of movement
	float hDir, vDir;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Assign the horizontal movement
		hDir = Input.GetAxis ("Horizontal");
		//Assign the vertical movement
		vDir = Input.GetAxis ("Vertical");

		//Add the change in direction to the position of the object
		transform.position += new Vector3 (hDir, 0, vDir) * Time.deltaTime;
	}
}
