using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;//加载场景时需引用

public class UIManager : MonoBehaviour
{
    public static UIManager uiMagr;
    public Text number_txt;//分数
    public Button refreshBtn;//重新开始按钮
    public Transform dieUI;//游戏失败面板

    void Start()
    {
        //初始化
        uiMagr = this;
        dieUI.gameObject.SetActive(false);
        refreshBtn.onClick.AddListener(Refresh);

    }

    void Update()
    {
        //按下esc 退出程序
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //修改分数值
    public void SetNumber(int value) 
    {
        number_txt.text = value.ToString();
    }

    //显示死亡UI
    public void DieUI() 
    {
        dieUI.gameObject.SetActive(true);
    }

    //刷新
    public void Refresh() 
    {
        //重新加载场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
