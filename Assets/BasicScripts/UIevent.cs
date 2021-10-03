using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIevent : MonoBehaviour {

    public Cover TX, TY, TN;//开始和结束界面都有UIEvent，用不同的txtytn
    public Cover Tsave, Tcut, Tassess;
    public GameObject startMenu, overMenu;

    public void setEasy() {
        Game.xSet = 12;
        Game.ySet = 8;
        Game.nSet = 20;
        TX.changeTx("X:" + Game.xSet.ToString());
        TY.changeTx("Y:" + Game.ySet.ToString());
        TN.changeTx("N:" + Game.nSet.ToString());
    }
    public void setHard() {
        Game.xSet = 32;
        Game.ySet = 20;
        Game.nSet = 150;
        TX.changeTx("X:" + Game.xSet.ToString());
        TY.changeTx("Y:" + Game.ySet.ToString());
        TN.changeTx("N:" + Game.nSet.ToString());
    }
    public void setRand() {
        Game.xSet = Random.Range(12, 48);
        Game.ySet = (int)(Game.xSet * Random.Range(0.5f, 1f));
        Game.nSet = (int)(Game.xSet * Game.ySet * Random.Range(0.15f, 0.4f));
        TX.changeTx("X:" + Game.xSet.ToString());
        TY.changeTx("Y:" + Game.ySet.ToString());
        TN.changeTx("N:" + Game.nSet.ToString());
    }
    public void startGame() {
        Game.ins.startGame();
        startMenu.SetActive(false);
    }
    public void setScore(int x, int y, int n, int save, int ct) {
        TX.changeTx("X:" + x.ToString());
        TY.changeTx("Y:" + y.ToString());
        TN.changeTx("N:" + n.ToString());
        int sv = Mathf.RoundToInt(save * 100f / x / y);
        Tsave.changeTx("Save " + sv.ToString() + "%");
        Tcut.changeTx("Cut " + ct.ToString() + "/" + n.ToString());
        string ass;
        sv += Mathf.RoundToInt(ct * 100f / n);
        sv /= 2;
        if(sv >= 90) ass = "S";
        else if(sv >= 80) ass = "A";
        else if(sv >= 60) ass = "B";
        else if(sv >= 30) ass = "C";
        else ass = "D";
        Tassess.changeTx(ass);
    }

    public void backToMenu() {
        startMenu.SetActive(true);
        Game.ins.Start();
        overMenu.SetActive(false);
    }

}
