using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DelSacrifice : MonoBehaviour, IPointerClickHandler
{
    public SelectObl se;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnPointerClick(PointerEventData eventData)
    {
        se.isSel = false;
        se.con.isUsed[se.pla.idoftarget] = false;
        Destroy(gameObject);
    }
}
