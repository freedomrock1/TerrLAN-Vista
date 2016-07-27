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
}
