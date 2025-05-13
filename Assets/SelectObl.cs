using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectObl : MonoBehaviour, IPointerClickHandler
{
    public bool isSel = false;
    private playhand pla;
    private gamecontroller con;
    public void Awake()
    {
        pla = GetComponent<playhand>();
        con = pla.controller.GetComponent<gamecontroller>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pla.isPlaced)
        {
            if (isSel)
            {
                Image image = GetComponent<Image>();
                if (image != null)
                {
                    image.color = Color.white;
                    Debug.Log(Color.white);
                }
                isSel = false;
                con.isUsed[pla.idoftarget] = false;
            }
            else
            {
                Image image = GetComponent<Image>();
                if(image!=null)
                {
                    image.color = new Color(1, 0, 0, 0.5f);
                    Debug.Log(new Color(1, 0, 0, 0.5f));
                }
                isSel = true;
                con.isUsed[pla.idoftarget] = true;
            }
        }
    }
}
