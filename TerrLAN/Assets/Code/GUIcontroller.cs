using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.IO;
using UnityEngine.UI;


public class GUIcontroller : MonoBehaviour {//attach to canvas of GUI

    public GameObject player;
    public GameObject scrollView;
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
            if(fileInfo[p].Extension != ".meta")
            {
                buttonList[p] = Instantiate(button);
                buttonList[p].transform.parent = scrollView.transform;
                buttonList[p].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75 - buttonList[p].GetComponent<RectTransform>().sizeDelta.y * p);
                buttonList[p].GetComponentInChildren<Text>().text = fileInfo[p].Name;
                string temp = fileInfo[p].Name.Replace(fileInfo[p].Extension, null);
                buttonList[p].GetComponent<Button>().onClick.AddListener(delegate { LoadResource(temp); });
                buttonList[p].gameObject.SetActive(!buttonList[p].gameObject.active);
            }
        }
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x, (buttonList.Length+5) * button.GetComponent<RectTransform>().sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGUI();
        }
        if(placingObject != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GameObject tempPlaced = Instantiate(placingObject);
                Destroy(placingObject);
                placingObject = null;
            }
            Ray ray = new Ray(player.GetComponentInChildren<Camera>().transform.position, player.GetComponentInChildren<Camera>().transform.forward);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            placingObject.transform.position = hit.point;
        }
	}

    void LoadResource(string path)
    {
        Debug.Log("Resources loaded:" + path);
        GameObject temp = Resources.Load(path) as GameObject;
        placingObject = Instantiate(Resources.Load(path) as GameObject);
        ToggleGUI();
    }

    public void ToggleGUI()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(!this.gameObject.transform.GetChild(i).gameObject.activeSelf);
            //ToggleActiveGUI(this.gameObject.transform.GetChild(i).gameObject);
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

    void ToggleActiveGUI(GameObject element)
    {
        element.SetActive(!element.activeSelf);
        for (int i = 0; i < element.gameObject.transform.childCount; i++)
        {
            ToggleActiveGUI(this.gameObject.transform.GetChild(i).gameObject);
        }
    }
}
