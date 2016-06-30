using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.IO;
using UnityEngine.UI;


public class GUIcontroller : MonoBehaviour {//attach to canvas of GUI

    public GameObject player;
    public GameObject panel;
    public GameObject button;
    GameObject placingObject;
    FileInfo[] fileInfo;
    GameObject[] buttonList;


	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        string[] fileInfoStrings = Directory.GetFiles(Application.dataPath + "/Resources");
        FileInfo[] fileInfo = new FileInfo[fileInfoStrings.Length];
        for(int i=0;i<fileInfoStrings.Length;i++)
        {
            fileInfo[i] = new FileInfo(fileInfoStrings[i]);
        }
        buttonList = new GameObject[fileInfo.Length];
        for(int p=0;p<fileInfo.Length;p++)
        {
            buttonList[p] = Instantiate(button);
            buttonList[p].transform.parent = panel.transform;
            buttonList[p].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75 - buttonList[p].GetComponent<RectTransform>().sizeDelta.y*p);
            buttonList[p].GetComponentInChildren<Text>().text = fileInfo[p].Name;
            buttonList[p].GetComponent<Button>().onClick.AddListener(delegate { LoadResource(buttonList[p].GetComponentInChildren<Text>().text); });
            buttonList[p].gameObject.SetActive(!buttonList[p].gameObject.active);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGUI();
        }
        if(placingObject != null)
        {
            Ray ray = new Ray(player.GetComponentInChildren<Camera>().transform.position, player.GetComponentInChildren<Camera>().transform.forward);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            placingObject.transform.position = hit.transform.position;
        }
	}

    void LoadResource(string path)
    {
        Debug.Log("Resources loaded");
        GameObject temp = Resources.Load(path) as GameObject;
        placingObject = temp;
        ToggleGUI();
    }

    public void ToggleGUI()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(!this.gameObject.transform.GetChild(i).gameObject.activeSelf);
        }
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else Time.timeScale = 0;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = !Cursor.visible;
        player.GetComponent<FirstPersonController>().enabled = !player.GetComponent<FirstPersonController>().enabled;
    }

    public GameObject PlacingObject
    {
        get { return placingObject; }
        set { placingObject = value; }
    }

}
