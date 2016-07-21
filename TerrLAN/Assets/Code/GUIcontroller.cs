using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System.IO;
using UnityEngine.UI;
using System.Windows.Forms;


public class GUIcontroller : MonoBehaviour {//attach to canvas of GUI

    public GameObject player;
    public GameObject scrollView; //scrollview object under a canvas for loading of buttons
    public GameObject button; //premake the buttons into prefabs, it just makes it easier
    private bool menu;
    float yOffset; //value used to adjust height of placed buildings. Likely to be scrapped when more reliable buildings that can place themselves properly are imported
    GameObject placingObject; //temp gameobject used for placing a new building
    FileInfo[] fileInfo;
    GameObject[] buttonList;


	// Use this for initialization
	void Start () {
        menu = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        RefreshFileList();
        yOffset = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(!menu)
        {//a hacked together way of keeping the dang mouse cursor locked when the menu is closed, since unity's cursorlockmode functions flat out don't work
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(UnityEngine.Screen.width/2, UnityEngine.Screen.height/2);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(placingObject != null)
                ToggleGUI();//opens gui, naturally
            else
            {
                placingObject = null;
                ToggleGUI();
            }
        }
        if(placingObject != null)
        {//run this code if we're putting a building down
            if(Input.GetMouseButton(0))//if you click, you place.
            {
                GameObject tempPlaced = Instantiate(placingObject);
                BoxCollider[] colliderList;
                colliderList = tempPlaced.GetComponentsInChildren<BoxCollider>();
                foreach (BoxCollider b in colliderList)
                {
                    if(b.gameObject.name.Contains("Model"))
                    {
                        b.enabled = false;
                    }
                    else b.enabled = true;
                }
                Destroy(placingObject);
                placingObject = null;
                yOffset = 0;
            }
            else//calculates position via ray and height via offset, changeable by scrolling the mousewheel
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

    void LoadResource(int index, FileInfo[] fileInfo)//loads a building from a file. This process will have to modified to use the WWW class when a website for loading files is established
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
        ToggleGUI();//automatically closes menu for placing of building
    }

    public void ToggleGUI()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {//toggles active state of all gui elements, hiding hud and showing disabled gui windows
            this.gameObject.transform.GetChild(i).gameObject.SetActive(!this.gameObject.transform.GetChild(i).gameObject.activeSelf);
        }
        if (Time.timeScale == 0)
        {//pauses time, for the most part
            Time.timeScale = 1;
        }
        else Time.timeScale = 0;
        if (menu)
        {//the code that was supposed to lock the cursor. peh.
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        else if(!menu)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        player.GetComponent<FirstPersonController>().enabled = !player.GetComponent<FirstPersonController>().enabled;//pauses fps to prevent looking around. might have to be modified with the use of other controllers on the player
        menu = !menu;
    }

    void RefreshFileList()//reloads all files in the resource directory. Ideally files will be loaded from a website for the final product, at which time this process will have to be changed as well
    {
        string[] fileInfoStrings = Directory.GetFiles(UnityEngine.Application.dataPath + "/Resources");
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
                buttonList[p].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { LoadResource(temp, fileInfo); });
                buttonList[p].gameObject.SetActive(!buttonList[p].gameObject.active);
            }
        }
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x, (actualLength + 5) * button.GetComponent<RectTransform>().sizeDelta.y);
    }
}
