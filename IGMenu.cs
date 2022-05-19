using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IGMenu : MonoBehaviour
{
    GameObject igm;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !GameObject.Find("StatsPanel"))
        {
            if (!igm)
                CreateIGMenu();
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                Destroy(igm);
            }
        }
    }
    public void CreateIGMenu()
    {
        //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject GOB = new GameObject();
        igm = GOB;
        GOB.name = "In-Game Menu Background";
        igm.transform.SetParent(GameObject.Find("Canvas").transform);
        GOB.AddComponent<RectTransform>();
        GOB.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        GOB.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        GOB.GetComponent<RectTransform>().offsetMin = GOB.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        GOB.AddComponent<Image>();
        GOB.GetComponent<Image>().color = new Color32(0,0,0,160);
        GameObject GO = new GameObject();
        GO.name = "In-Game Menu";
        GO.transform.SetParent(GOB.transform);
        GO.AddComponent<RectTransform>();
        GO.AddComponent<Image>();
        GO.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        GO.GetComponent<RectTransform>().anchorMin = new Vector2(0.35f, 0.1f);
        GO.GetComponent<RectTransform>().anchorMax = new Vector2(0.65f, 0.9f);
        GO.GetComponent<RectTransform>().offsetMin = GO.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        GO.AddComponent<VerticalLayoutGroup>();
        GO.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
        GO.GetComponent<VerticalLayoutGroup>().childControlWidth = true;
        GO.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = true;
        GO.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
        GO.GetComponent<VerticalLayoutGroup>().spacing = 25;
        GO.GetComponent<VerticalLayoutGroup>().padding.left = GO.GetComponent<VerticalLayoutGroup>().padding.right = GO.GetComponent<VerticalLayoutGroup>().padding.bottom = GO.GetComponent<VerticalLayoutGroup>().padding.top = 10;
        CreateMenuButtons(GO);
    }
    public void CreateMenuButtons(GameObject Menu)
    {
        for(int i=0;i<=4;i++)
        {
            GameObject GOButton = new GameObject();
            GOButton.transform.SetParent(Menu.transform);
            GOButton.name = "Button " + i;
            GOButton.AddComponent<RectTransform>();
            GOButton.AddComponent<Button>();
            GOButton.AddComponent<Image>();
            GOButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            GameObject GOButtonText = new GameObject();
            GOButtonText.transform.SetParent(GOButton.transform);
            GOButtonText.name = "text";
            GOButtonText.AddComponent<Text>();
            GOButtonText.GetComponent<Text>().text = i + ": " + i;
        }

    }
}
