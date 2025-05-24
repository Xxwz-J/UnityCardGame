using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectObl : MonoBehaviour, IPointerClickHandler
{
    public Sprite sprite;
    public bool isSel = false;
    public playhand pla;
    public gamecontroller con;
    public void Awake()
    {
        pla = GetComponent<playhand>();
        con = pla.controller.GetComponent<gamecontroller>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pla.isPlaced)
        {
            if(gameObject.GetComponent<CardDisplay>().card.cardID==14)
            {
                con.ShowText(4);
                return;
            }
            if (isSel)
            {
                Transform child = transform.Find("CoverImage");
                if (child != null)
                {
                    Destroy(child.gameObject);
                }

                isSel = false;
                con.isUsed[pla.idoftarget] = false;
            }
            else
            {
                GameObject cover = new GameObject("CoverImage");
                cover.transform.SetParent(transform);
                cover.transform.localPosition = Vector3.zero;
                cover.AddComponent<Image>().sprite = sprite;
                cover.AddComponent<DelSacrifice>().se = this;
                isSel = true;
                con.isUsed[pla.idoftarget] = true;
            }
        }
    }
}
