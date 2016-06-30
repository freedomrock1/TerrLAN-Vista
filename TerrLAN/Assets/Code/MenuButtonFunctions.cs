using UnityEngine;
using System.Collections;

public class MenuButtonFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadResource(string path)
    {
        GameObject temp = Resources.Load(path) as GameObject;
        this.gameObject.transform.GetComponentInParent<GUIcontroller>().PlacingObject = temp;
        this.gameObject.transform.GetComponentInParent<GUIcontroller>().ToggleGUI();
    }
}
