using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class infoTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text text = this.gameObject.GetComponent<Text>();
        GameObject info = GameObject.Find("info");
        text.text = "City: " + info.GetComponent<info>().city + "\n" + "Building: " + info.GetComponent<info>().building + "\nClick anywhere to proceed to lan-vista";
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel("lan-vista");
        }
	}
}
