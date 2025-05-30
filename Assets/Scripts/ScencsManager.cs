using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScencsManager : MonoBehaviour
{

  

    public void ScencsLoad(string name)
    {
        switch(GetComponent<RectTransform>().anchoredPosition.x)
            { case -258:
                GlobalData.levelid = 1;
                break;
            case -105:
                GlobalData.levelid = 2;
                break;
            case 57:
                GlobalData.levelid = 3;
                break;
            case 261:
                GlobalData.levelid = 4;
                break;
        }
        SceneManager.LoadScene(name);
    }
}
