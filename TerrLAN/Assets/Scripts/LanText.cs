using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanText : MonoBehaviour {

	GameObject info;
	PersistentInfo myInfo;
	Text statusText;

	// Use this for initialization
	void Start () {
		info = GameObject.Find ("Info");
		myInfo = info.GetComponent<PersistentInfo> ();
		statusText = this.GetComponent<Text> ();

		if (myInfo.city == "") 
		{
			statusText.text = "Building: " + myInfo.buildingNum + '\n' + '\n' + "Latitude: " + myInfo.latitude  + '\n' + "Longitude: " + myInfo.longitude;
		} 
		else if(myInfo.city != "")
		{
			statusText.text = "Building: " + myInfo.buildingNum + '\n' + '\n' + "City: " + myInfo.city;
		}
	}

}
