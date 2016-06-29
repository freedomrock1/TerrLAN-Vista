using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour {

	Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player);
	}
}
