using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public Sprite btn1, btn2, btn3, ruin;
    public TextMesh myText;
    public int myx, myy;//记录当前块左下角位置
    public int myxR = 0, myyR = 0;//当前块向右，向下长度

    SpriteRenderer myimg;
    bool iscover;

    private void Start() {
        myimg = GetComponent<SpriteRenderer>();
        myimg.color = new Color(1, 1, 1, 1);
        iscover = true;
    }

    public void uncover() {
        if(iscover) {
            iscover = false;
            myimg.color = new Color(0, 0, 0, 0);
            Game.ins.calSave++;
        }
    }

    public void beruin() {
        if(iscover) {
            iscover = false;
            myimg.sprite = ruin;
            myimg.color = new Color(1, 1, 1, 1);
            Game.ins.calRuin++;
        }
    }

    public void changeTx(string s) {//开始前ui中的XYN用
        if(!turning) StartCoroutine(turnAround());
        myText.text = s;
    }

    bool turning = false;
    IEnumerator turnAround() {
        turning = true;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        Vector3 to = new Vector3(0, 90, 0), deta = to - transform.localEulerAngles;
        while(deta.magnitude > 0.5f) {
            transform.localEulerAngles += deta / 3;
            deta = to - transform.localEulerAngles;
            yield return new WaitForSeconds(0.02f);
        }
        to = new Vector3(0, 0, 0); deta = to - transform.localEulerAngles;
        while(deta.magnitude > 0.5f) {
            transform.localEulerAngles += deta / 3;
            deta = to - transform.localEulerAngles;
            yield return new WaitForSeconds(0.02f);
        }
        transform.localEulerAngles = new Vector3(0, 0, 0);
        turning = false;
    }

    private void Update() {
        if(iscover) {
            Vector2 mp = MyInput.mousePos;
            if(myx <= mp.x && mp.x <= myx + myxR && myy <= mp.y && mp.y <= myy + myyR) {
                myimg.sprite = btn2;
                bool p1 = Input.GetMouseButton(0), p3 = Input.GetMouseButtonUp(0);
                bool p2 = Input.GetMouseButton(1), p4 = Input.GetMouseButtonUp(1);
                if(p1 || p2) myimg.sprite = btn3;
                if(!Game.isstart && (p3 || p4)) {
                    if(!turning) StartCoroutine(turnAround());
                } else {
                    if(p3) Game.ins.openCover(myx, myy);
                    if(p4) Game.ins.sweepCover(myx, myy);
                }
            } else {
                myimg.sprite = btn1;
            }

        } else {
            int gt = Game.ins.getNum(myx, myy);
            if(gt == 0) myText.text = "";
            else {
                if(gt <= 3) myText.color = new Color(1, 0, 0, 1);
                else myText.color = new Color(0, 0, 0, 1);
                myText.text = gt.ToString();
            }
        }
    }

}
