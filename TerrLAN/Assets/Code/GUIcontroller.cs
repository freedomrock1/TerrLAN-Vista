using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class GUIcontroller : MonoBehaviour {//attach to canvas of GUI

    public GameObject player;


	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            for(int i=0;i<this.gameObject.transform.childCount;i++)
            {
                this.gameObject.transform.GetChild(i).gameObject.SetActive(!this.gameObject.transform.GetChild(i).gameObject.activeSelf);
            }
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else Time.timeScale = 0;
            if(Cursor.lockState == CursorLockMode.Locked)
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
	}
}
