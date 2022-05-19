using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    public GameObject refe;
    public Button[] buttonArray = new Button[16];
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        refe = GameObject.Find("Player");
        GameObject.Find("rightPanel/upperPanel/Slider").GetComponent<Slider>().maxValue = (100 + (refe.GetComponent<MovementController>().LVL - 1) * 20);
        GameObject.Find("rightPanel/upperPanel/Slider").GetComponent<Slider>().value = refe.GetComponent<MovementController>().XP;
        GameObject.Find("rightPanel/upperPanel/LVLtext").GetComponent<Text>().text = "LVL " + refe.GetComponent<MovementController>().LVL;
        GameObject.Find("rightPanel/lowerPanel/AvailablePointsPanel/Text1").GetComponent<Text>().text =refe.GetComponent<MovementController>().SkillPoints.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 15; i++)
        {
            int tempI = i;
            //buttonArray[i].GetComponent<Button>().onClick.AddListener(() => AddSkill(i));
            buttonArray[i].GetComponent<Button>().onClick.AddListener(() => AddSkill(tempI));
            Debug.Log(buttonArray[i].transform.gameObject.name + ": " + i);
        }
        for (int i = 0; i <= 15; i++)
        {
            GameObject.Find(buttonArray[i].gameObject.name + "/TextBG/Text").GetComponent<Text>().text = refe.GetComponent<MovementController>().stats[i].ToString() + "/10";
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("rightPanel/upperPanel/Slider").GetComponent<Slider>().maxValue = (100 + (refe.GetComponent<MovementController>().LVL - 1) * 20);
        GameObject.Find("rightPanel/upperPanel/Slider").GetComponent<Slider>().value = refe.GetComponent<MovementController>().XP;
        GameObject.Find("rightPanel/upperPanel/LVLtext").GetComponent<Text>().text = "LVL " + refe.GetComponent<MovementController>().LVL;
        GameObject.Find("rightPanel/lowerPanel/AvailablePointsPanel/Text1").GetComponent<Text>().text = refe.GetComponent<MovementController>().SkillPoints.ToString();
        if(Input.GetKeyDown(KeyCode.F))
        {
            //Cursor.visible = false;
            Cursor.lockState=CursorLockMode.Locked;
            Destroy(GameObject.Find("StatsPanel"));
        }
    }
    public void AddSkill(int param)
    {
        Debug.LogWarning(param);
        if(refe.GetComponent<MovementController>().SkillPoints>0 && refe.GetComponent<MovementController>().stats[param]<10)
        {
            Debug.Log("Zakupiono skilla o id: " + param);
            refe.GetComponent<MovementController>().SkillPoints -= 1;
            refe.GetComponent<MovementController>().stats[param] += 1;
            GameObject.Find(buttonArray[param].gameObject.name + "/TextBG/Text").GetComponent<Text>().text = refe.GetComponent<MovementController>().stats[param].ToString() + "/10";
            UpdateSkill(param);
        }
    }
    public void UpdateSkill(int param)
    {
        if(param==0)
        {
            refe.GetComponent<MovementController>().speed = (20 + refe.GetComponent<MovementController>().stats[0] * 10);
        }
    }
}
