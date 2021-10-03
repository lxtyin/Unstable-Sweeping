using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Warning : MonoBehaviour
{
    Transform mylight;
    void Start()
    {
        mylight = GetComponent<Transform>();
        StartCoroutine(warn());
    }

    IEnumerator warn() {
        mylight.localScale = new Vector3(0.5f, 0.5f, 1);
        Vector3 to = new Vector3(1, 1, 1), deta = to - mylight.localScale;
        while(deta.magnitude > 0.1f) {
            mylight.localScale += deta / 3;
            deta = to - mylight.localScale;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.3f);
        to = new Vector3(0, 0, 1); deta = to - mylight.localScale;
        while(deta.magnitude > 0.1f) {
            mylight.localScale += deta / 5;
            deta = to - mylight.localScale;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }

}
