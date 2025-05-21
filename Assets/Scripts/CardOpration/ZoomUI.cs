using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ZoomUI : MonoBehaviour,IPointerEnterHandler ,IPointerExitHandler 
{
    public float zoomSize;

    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData pointerEventData) {
        transform.localScale = new Vector2(zoomSize, zoomSize);

    }
    public void OnPointerExit(PointerEventData pointerEventData) {
        transform.localScale = Vector2.one;
    }


}
