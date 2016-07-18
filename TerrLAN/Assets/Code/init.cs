using UnityEngine;
using System.Collections;

public class init : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // load building
        GameObject instance = Instantiate(Resources.Load("Building view", typeof(GameObject))) as GameObject;
        instance.transform.position = new Vector3(transform.position.x , 0.15f, transform.position.z );
        instance.transform.localScale = new Vector3(2, 2, 2);
        // load network 


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
