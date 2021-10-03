using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInput : MonoBehaviour
{
    public static Vector2Int mousePos;

    private void Update() {
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.RoundToInt(p.x);
        mousePos.y = Mathf.RoundToInt(p.y);
    }

}
