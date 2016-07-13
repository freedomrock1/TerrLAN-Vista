﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.IO;
using UnityEngine.UI;


public class GUIcontroller : MonoBehaviour {//attach to canvas of GUI

    public GameObject player;
    public GameObject scrollView;
    public GameObject button;
    float yOffset;
    GameObject placingObject;
    FileInfo[] fileInfo;
    GameObject[] buttonList;


	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        RefreshFileList();
        yOffset = 0;
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
                BoxCollider[] colliderList;
                colliderList = tempPlaced.GetComponentsInChildren<BoxCollider>();
                foreach (BoxCollider b in colliderList)
                {
                    b.enabled = true;
                }
                Destroy(placingObject);
                placingObject = null;
                yOffset = 0;
            }
            else
            {
                Ray ray = new Ray(player.GetComponentInChildren<Camera>().transform.position, player.GetComponentInChildren<Camera>().transform.forward);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                placingObject.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                float mouse = Input.GetAxis("Mouse ScrollWheel");
                if (mouse != 0)
                {
                    if (mouse > 0)
                    {
                        yOffset += 0.1f;
                    }
                    else if (mouse < 0)
                    {
                        yOffset -= 0.1f;
                    }
                }
                placingObject.transform.position = new Vector3(placingObject.transform.position.x, placingObject.transform.position.y + yOffset, placingObject.transform.position.z);
            }
        }
	}

    void LoadResource(int index, FileInfo[] fileInfo)
    {
        if(fileInfo[index].Extension == "")
        {
            string path = fileInfo[index].DirectoryName + "/" + fileInfo[index].Name;
            AssetBundle temp = AssetBundle.LoadFromFile(path);
            Object[] objList;
            objList = temp.LoadAllAssets();
            foreach (GameObject g in objList)
            {
                placingObject = Instantiate(g);
                BoxCollider[] colliderList;
                colliderList = placingObject.GetComponentsInChildren<BoxCollider>();
                foreach(BoxCollider b in colliderList)
                {
                    b.enabled = false;
                }
            }
        }
        else
        {
            GameObject temp = Resources.Load(fileInfo[index].Name.Replace(fileInfo[index].Extension, null)) as GameObject;
            placingObject = Instantiate(Resources.Load(fileInfo[index].Name.Replace(fileInfo[index].Extension, null)) as GameObject);
            BoxCollider[] colliderList;
            colliderList = placingObject.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider b in colliderList)
            {
                b.enabled = false;
            }
        }
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

    void RefreshFileList()
    {
        string[] fileInfoStrings = Directory.GetFiles(Application.dataPath + "/Resources");
        FileInfo[] fileInfo = new FileInfo[fileInfoStrings.Length];
        for (int i = 0; i < fileInfoStrings.Length; i++)
        {
            fileInfo[i] = new FileInfo(fileInfoStrings[i]);
        }
        buttonList = new GameObject[fileInfo.Length];
        int actualLength = -1;
        for (int p = 0; p < fileInfo.Length; p++)
        {
            if (fileInfo[p].Extension != ".meta" && fileInfo[p].Extension != ".manifest" && fileInfo[p].Extension != ".unitypackage")
            {
                actualLength++;
                buttonList[p] = Instantiate(button);
                buttonList[p].transform.parent = scrollView.transform;
                buttonList[p].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75 - buttonList[p].GetComponent<RectTransform>().sizeDelta.y * actualLength);
                buttonList[p].GetComponentInChildren<Text>().text = fileInfo[p].Name;
                //string temp = fileInfo[p].Name.Replace(fileInfo[p].Extension, null);
                int temp = p;
                buttonList[p].GetComponent<Button>().onClick.AddListener(delegate { LoadResource(temp, fileInfo); });
                buttonList[p].gameObject.SetActive(!buttonList[p].gameObject.active);
            }
        }
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x, (actualLength + 5) * button.GetComponent<RectTransform>().sizeDelta.y);
    }
}
