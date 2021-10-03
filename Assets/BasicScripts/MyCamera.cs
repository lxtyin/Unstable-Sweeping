using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public static MyCamera ins;
    Camera myself;

    private void Awake() {
        ins = this;
        myself = GetComponent<Camera>();
    }

    public IEnumerator moveTo(Vector2 to, float siz) {
        myself.orthographicSize = 2;
        transform.position = new Vector3(to.x, to.y, -10);
        float dta = siz - myself.orthographicSize;
        while(dta > 0.1f) {
            myself.orthographicSize += dta / 5;
            dta = siz - myself.orthographicSize;
            yield return new WaitForSeconds(0.02f);
        }
        myself.orthographicSize = siz;
    }



}
