using UnityEngine;
using System.Collections;

public class buttonScripts : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoToScene(string s)
    {
        Application.LoadLevel(s);
    }

    public void SetDestinationCity(string s)
    {
        info infoObject = GameObject.Find("info").GetComponent<info>();
        infoObject.city = s;
        Application.LoadLevel("City Map");
    }
}
