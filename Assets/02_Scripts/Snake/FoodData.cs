using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : MonoBehaviour
{
    public Vector3 oldPos;//记录上一次移动
    public FoodData parentObj;//获取身体部位的上一个身体部位属性（用于贪吃蛇移动时，下一个方块移动到上一个方块的位置）
    public FoodData newObj;//记录下一个身体部位属性（用于当前身体移动后 调用下一个物体的移动方法）
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //获取上一个物体（食物被吃掉时调用）
    public void SetDataInt(FoodData obj) 
    {
        parentObj = obj;
        //设置上一个物体的newobj为自己
        parentObj.newObj = this;
        //食物被吃掉变成白色
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        //位置初始化
        transform.position = parentObj.oldPos;
    }

    //移动 （用于当前物体移动完后执行下一个物体的移动）
    public void Move() 
    {
        //记录上一次移动位置（这个值 下一个物体移动时所需）
        oldPos=transform.position;

        //修改位置
        transform.position = parentObj.oldPos;

        //执行下一个物体的移动命令
        if (newObj!=null)
        {
            newObj.Move();
        }
    }

    //移动（蛇的头部移动 需要传参）
    public void Move(Vector3 pos) 
    {
        oldPos = transform.position;
        //修改位置
        transform.position = pos;

        if (newObj != null)
        {
            newObj.Move();
        }
    }
}
