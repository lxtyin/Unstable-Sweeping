using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Game : MonoBehaviour
{
    public static Game ins;
    public static bool isstart = false;
    public static int xSet, ySet, nSet;//设定的地图大小，地雷数量
    public int calRuin, calCut, calSave;
    public GameObject back, cover, coverBox;
    public GameObject flash, warn, endMenu;
    public Mine mine, cutline;
    public Light2D potLight;
    Cover[, ] mp = new Cover[80, 80];
    int[,] state = new int[80, 80];//0为覆盖，1为揭开，2为地雷
    float[,] rest = new float[80, 80];
    bool[,] exposed = new bool[80, 80];//周围8格有无被挖开

    private void Awake() {
        ins = this;
    }
    public void Start() {
        xSet = 16;//初始界面图大小
        ySet = 8;
        nSet = 35;
        StartCoroutine(MyCamera.ins.moveTo(new Vector2(xSet / 2.0f + 0.5f, ySet / 2.0f + 0.5f), Mathf.Max(xSet / 4.0f, ySet / 2.0f) + 1));
        StartCoroutine(flashLight());
    }

    bool firstopen;
    public void startGame() {
        for(int i = 0; i < coverBox.transform.childCount; i++) {
            Destroy(coverBox.transform.GetChild(i).gameObject);
        }
        calCut = calSave = calRuin = 0;
        isstart = true;
        firstopen = false;
        for(int i = 0; i <= xSet + 1; i++) state[i, 0] = state[i, ySet + 1] = -1;
        for(int j = 0; j <= ySet + 1; j++) state[0, j] = state[xSet + 1, j] = -1;
        for(int i = 1; i <= xSet; i++) {
            for(int j = 1; j <= ySet; j++) {
                Instantiate(back, new Vector3(i, j, 0), Quaternion.identity).transform.SetParent(coverBox.transform);
                Cover g = Instantiate(cover, new Vector3(i, j, 0), Quaternion.identity).GetComponent<Cover>();
                g.transform.parent = coverBox.transform;
                mp[i, j] = g;
                state[i, j] = 0;
                exposed[i, j] = false;
                g.myx = i;
                g.myy = j;
            }
        }
        StartCoroutine(MyCamera.ins.moveTo(new Vector2(xSet / 2.0f + 0.5f, ySet / 2.0f + 0.5f), Mathf.Max(xSet / 4.0f, ySet / 2.0f) + 1));
        StartCoroutine(flashLight());
    }

    void postMine(int x, int y) {//以x, y为第一次点的位置
        for(int i = x - 1; i <= x + 1; i++) {
            for(int j = y - 1; j <= y + 1; j++) {
                if(state[i, j] != -1) state[i, j] = 1;
            }
        }
        for(int i = 1; i <= nSet; i++) {
            int px = Random.Range(1, xSet + 1), py = Random.Range(1, ySet + 1);
            while(state[px, py] != 0) {
                px = Random.Range(1, xSet);
                py = Random.Range(1, ySet);
            }
            state[px, py] = 2;
            rest[px, py] = Random.Range(5.0f, 9.0f);
        }
        for(int i = x - 1; i <= x + 1; i++) {
            for(int j = y - 1; j <= y + 1; j++) {
                if(state[i, j] != -1) state[i, j] = 0;
            }
        }
    }

    public void ruinCenter(int x, int y) {
        for(int i = x - 1; i <= x + 1; i++) {
            for(int j = y - 1; j <= y + 1; j++) {
                if(i == x && j == y) continue;
                if(state[i, j] == 1) continue;
                if(state[i, j] == 0) {
                    mp[i, j].beruin();
                    openCover(i, j);
                }
                if(state[i, j] == 2) {
                    mp[i, j].beruin();
                    state[i, j] = 1;
                    Instantiate(mine).Exploade(i, j);
                }
            }
        }
    }

    public void openCover(int x, int y) {
        if(state[x, y] == 1) return;
        if(state[x, y] == 2) {
            mp[x, y].beruin();
            state[x, y] = 1;
            Instantiate(mine).Exploade(x, y);
        } else if(state[x, y] == 0) {
            if(firstopen == false) {
                firstopen = true;
                postMine(x, y);
            }
            state[x, y] = 1;
            mp[x, y].uncover();
            if(getNum(x, y) == 0) {
                for(int i = x - 1; i <= x + 1; i++) {
                    for(int j = y - 1; j <= y + 1; j++) {
                        openCover(i, j);
                    }
                }
            }
        }
    }

    public void sweepCover(int x, int y) {//扫雷
        Instantiate(cutline, new Vector3(x, y, 0), Quaternion.identity);
        if(state[x, y] == 2) {
            Instantiate(mine).Becut(x, y);
            state[x, y] = 0;
            calCut++;
            openCover(x, y);
        } else if(state[x, y] == 0) {
            Instantiate(warn, new Vector3(x, y, 0), Quaternion.identity);
            openCover(x, y);
            for(int i = x - 1; i <= x + 1; i++) {
                for(int j = y - 1; j <= y + 1; j++) {
                    if(state[i, j] == 2) {
                        if(exposed[i, j]) {
                            rest[i, j] -= 1;
                            if(rest[i, j] <= 0) {
                                openCover(i, j);
                            }
                        }
                    }
                }
            }
        }
    }

    public int getNum(int x, int y) {
        int r = 1009;
        for(int i = x - 1; i <= x + 1; i++) {
            for(int j = y - 1; j <= y + 1; j++) {
                if(i == x && j == y) continue;
                if(state[i, j] == 2) {
                    r = Mathf.Min(r, (int)(0.99f + rest[i, j]));
                }
                if(!exposed[i, j]) exposed[i, j] = true;//被其他东西getnum时即暴露
            }
        }
        if(r == 1009) return 0;
        return r;
    }

    void gameover() {
        isstart = false;
        for(int i = 0; i < coverBox.transform.childCount; i++) {
            Destroy(coverBox.transform.GetChild(i).gameObject);
        }
        endMenu.SetActive(true);
        endMenu.GetComponent<UIevent>().setScore(xSet, ySet, nSet, calSave, calCut);
        xSet = 16;//初始界面图大小
        ySet = 8;
        nSet = 35;
        StartCoroutine(MyCamera.ins.moveTo(new Vector2(xSet / 2.0f + 0.5f, ySet / 2.0f + 0.5f), Mathf.Max(xSet / 4.0f, ySet / 2.0f) + 1));
        StartCoroutine(flashLight());
    }

    private void Update() {

        if(isstart){
            if(calSave + calRuin == xSet * ySet) {
                isstart = false;
                Invoke("gameover", 1);
            }
            for(int i = 1; i <= xSet; i++) {
                for(int j = 1; j <= ySet; j++) {
                    if(state[i, j] == 2) {
                        if(exposed[i, j]) {
                            rest[i, j] -= Time.deltaTime;
                            if(rest[i, j] <= 0) {
                                openCover(i, j);
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator flashLight() {
        potLight.transform.position = new Vector3(-4, ySet + 4, 0);
        potLight.pointLightOuterRadius = Mathf.Sqrt(xSet * xSet + ySet * ySet);
        flash.transform.position = new Vector3(0, ySet + 10, 0);
        for(int i = 1; i <= 100; i++) {
            flash.transform.position -= new Vector3(0, 1, 0);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
