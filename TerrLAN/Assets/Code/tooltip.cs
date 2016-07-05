using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tooltip : MonoBehaviour {

    public string tooltipText;
    GameObject text;

	// Use this for initialization
	void Start () {
        text = new GameObject();
        text.AddComponent<Text>();
        text.GetComponent<Text>().text = tooltipText;
        text.transform.parent = this.gameObject.transform;
        text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.GetComponent<Text>().resizeTextForBestFit = true;
    }
	
	// Update is called once per frame
	void Update () {
	    if(RectTransformUtility.RectangleContainsScreenPoint(this.GetComponent<RectTransform>(),Input.mousePosition))
        {
            text.gameObject.SetActive(true);
            text.transform.position = new Vector3(Input.mousePosition.x + (text.GetComponent<RectTransform>().sizeDelta.x/1.5f), Input.mousePosition.y, Input.mousePosition.z);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
	}
}
