using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                isSel = false;
                con.isUsed[pla.idoftarget] = false;
            }
            else
            {
                isSel = true;
                con.isUsed[pla.idoftarget] = true;
            }
        }
    }
}
