using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class infoTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text text = this.gameObject.GetComponent<Text>();
        GameObject info = GameObject.Find("info");
        if(Application.loadedLevelName == "prelan-vista")
            text.text = "City: " + info.GetComponent<info>().city + "\n" + "Building: " + info.GetComponent<info>().building + "\nClick anywhere to proceed to lan-vista";
        else
        {
            text.text = "City: " + info.GetComponent<info>().city + "\n" + "Building: " + info.GetComponent<info>().building;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(Application.loadedLevelName == "prelan-vista")
        {
            if (Input.GetMouseButtonDown(0))
            {
                Application.LoadLevel("lan-vista");
            }
        }
	}
}
