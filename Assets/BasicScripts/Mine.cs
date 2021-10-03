using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    int myx, myy;
    public void Exploade(int x, int y) {
        myx = x; myy = y;
        transform.position = new Vector3(x, y, 0);
        GetComponent<Animator>().SetInteger("exploade", 1);
    }

    public void Becut(int x, int y) {
        myx = x; myy = y;
        transform.position = new Vector3(x, y, 0);
        GetComponent<Animator>().SetInteger("exploade", 2);
    }

    public void getRuin() {
        //AudioController.Instance.PlayAudio("Exploation1");
        Game.ins.ruinCenter(myx, myy);
    }

    public void destruct() {
        Destroy(gameObject);
    }

}
