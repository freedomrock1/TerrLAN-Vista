using UnityEngine;
using System.Collections;

public class init : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // load building
        GameObject instance = Instantiate(Resources.Load("skyscraperCollider", typeof(GameObject))) as GameObject;
        instance.transform.position = new Vector3(transform.position.x , 0.4f, transform.position.z );
        // load network 


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
