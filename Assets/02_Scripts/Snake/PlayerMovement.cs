using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//方向类型
public enum DirectionType
{
    Horizontal, Vertical
}

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 direction = new Vector3(1, 0, 0);//移动方向
    public float currentTimespeed; //当前移动速度时间（时间越小 速度越快 这里我通过时间来加快速度）
    public float timerSpeedRatio;//每次吃到食物所减去的时间（即：贪吃蛇加速）
    public int maxMushroomsOnScreen = 2; // 场上最大蘑菇数量
    float timer;//用于时间计算


    bool isDie = false;//是否死亡
    public GameObject bombPrefab; // 炸弹预制体
    public GameObject poisonMushroomPrefab; // 毒蘑菇预制体
    public float minMushroomSpawnTime = 5f; // 最小蘑菇生成时间
    public float maxMushroomSpawnTime = 10f; // 最大蘑菇生成时间
    public float minBombSpawnTime = 5f; // 最小生成时间
    public float maxBombSpawnTime = 10f; // 最大生成时间
    int foodnumber = 0;//吃掉的食物数量
    private bool isReversed = false;
    //食物 食物对象池 
    public FoodData foodPrefab;
    public Transform foodPool;
    //贪吃蛇容器 用于存储吃掉的食物变成的身体
    public Transform snakePool;


    //游戏地图
    public Transform GameBG;

    void Start()
        {
            //初始化timer
            timer = currentTimespeed;
            //初始化位置
            transform.position = new Vector3(0.5f, 0.5f, 1);
            StartCoroutine(SpawnBombs()); // 启动生成炸弹的协程
            StartCoroutine(SpawnMushrooms()); // 启动生成蘑菇的协程
        }

    void Update()
        {
            //玩家移动
            float h = Input.GetAxis("Horizontal");
            float V = Input.GetAxis("Vertical");

            //判断移动方向
            if (h != 0)
            {
                SetDirection(DirectionType.Horizontal, h);
            }
            if (V != 0)
            {
                SetDirection(DirectionType.Vertical, V);
            }

            //未死亡 执行移动
            if (!isDie)
            {
                Move();
            }
        }

    //修改方向
    private void SetDirection(DirectionType directionType, float _direction)
        {
            //判断水平的左右移动 或 垂直的上下移动
            float value = _direction > 0 ? 1f : -1f;

            //判断方向类型  设置移动方向 
            switch (directionType)
            {
                case DirectionType.Horizontal:
                    if (direction.x==0)
                    {
                        direction = new Vector2(value, 0);
                    }
                    break;
                case DirectionType.Vertical:
                    if (direction.y==0)
                    {
                        direction = new Vector2(0, value);
                    }
                    break;
                default:
                    break;
            }
        }

    //移动
    private void Move()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                Vector3 newPosition = transform.position + (isReversed ? -direction : direction);
                GetComponent<FoodData>().Move(newPosition);
                timer = currentTimespeed;
            }
        }

    //检测
    void OnTriggerEnter2D(Collider2D col)
        {
            //判断是否触发了墙或者是自己
            if (col.transform.tag.Equals("Wall") || col.transform.tag.Equals("Player"))
            {
                //死亡
                isDie = true;
                UIManager.uiMagr.DieUI();
            }

            //食物
            if (col.transform.tag.Equals("Food"))
            {
                //判断贪吃蛇有没有身体
                if (snakePool.childCount > 0)
                {
                    col.transform.GetComponent<FoodData>().SetDataInt(snakePool.GetChild(snakePool.childCount - 1).GetComponent<FoodData>());
                }

                if (snakePool.childCount <= 0)
                {
                    col.transform.GetComponent<FoodData>().SetDataInt(GetComponent<FoodData>());
                }
                col.transform.SetParent(snakePool);
                if (col.transform== snakePool.GetChild(0))
                {
                    col.transform.tag = "Untagged";
                }
                else
                {
                    col.transform.tag = "Player";
                }

                //创建食物
                CreatFood();
                //加速
                currentTimespeed -= timerSpeedRatio;
                if (currentTimespeed<=0.1f)
                {
                    currentTimespeed = 0.1f;
                }
                //UI
                foodnumber++;
                UIManager.uiMagr.SetNumber(foodnumber);
            }
            // 炸弹
            if (col.transform.CompareTag("Bomb")) // 检查是否是炸弹
            {
                if (snakePool.childCount <= 0) // 检查是否没有身体
                {
                    isDie = true; // 设置死亡状态
                    UIManager.uiMagr.DieUI(); // 调用死亡UI
                }
                else
                {
                    ReduceLength(); // 调用减少长度的方法
                    Destroy(col.gameObject); // 销毁炸弹
                    foodnumber--; // 减少分数
                    if (foodnumber < 0) // 确保分数不为负
                    {
                        foodnumber = 0;
                    }
                    UIManager.uiMagr.SetNumber(foodnumber); // 更新UI
                }
            }
        
        }

    //  生成毒蘑菇的协程
    private IEnumerator SpawnMushrooms()
        {
            while (true)
            {
                float waitTime = Random.Range(minMushroomSpawnTime, maxMushroomSpawnTime);
                yield return new WaitForSeconds(waitTime);

                // 检查当前场上的蘑菇数量
                int currentMushroomCount = GameObject.FindGameObjectsWithTag("PoisonMushroom").Length;

                if (currentMushroomCount < maxMushroomsOnScreen)
                {
                    CreateMushroom();
                }
            }
        }

    //  生成毒蘑菇
    private void CreateMushroom()
        {
            Vector2 pos = new Vector2(Random.Range(-GameBG.localScale.x / 2, GameBG.localScale.x / 2), 
                                    Random.Range(-GameBG.localScale.y / 2, GameBG.localScale.y / 2));
            Instantiate(poisonMushroomPrefab, pos, Quaternion.identity);
        }

    public void ApplyPoisonMushroomEffect(float duration)
        {
            StartCoroutine(HandlePoisonMushroom(duration));
        }
    //  HandlePoisonMushroom 处理毒蘑菇
    private IEnumerator HandlePoisonMushroom(float duration)
        {
            ReverseDirection();
            yield return new WaitForSeconds(duration);
            ReverseDirection();
        }
    //  ReverseDirection 转换方向
    public void ReverseDirection()
        {
            isReversed = !isReversed;
            direction = -direction;
        }
    // 生成炸弹的协程
    private IEnumerator SpawnBombs()
        {
            while (true)
            {
                float waitTime = Random.Range(minBombSpawnTime, maxBombSpawnTime);
                yield return new WaitForSeconds(waitTime);
                CreateBomb();
            }
        }

    // 创建炸弹
    private void CreateBomb()
        {
            Vector2 pos = new Vector2(Random.Range(-GameBG.localScale.x / 2, GameBG.localScale.x / 2), 
                                    Random.Range(-GameBG.localScale.y / 2, GameBG.localScale.y / 2));
            Instantiate(bombPrefab, pos, Quaternion.identity);
        }

    // 减少长度
    public void ReduceLength()
        {
            if (snakePool.childCount > 0)
            {
                Destroy(snakePool.GetChild(snakePool.childCount - 1).gameObject); // 销毁最后一个身体部分
            }
        }

    //创建食物
    public void CreatFood()
        {
            //基础值
            float basePos = 0.5f;
            int h_value = (int)(GameBG.localScale.x / 2);
            int v_value = (int)(GameBG.localScale.y / 2);
            int h = Random.Range(-h_value, h_value + 1);
            int v = Random.Range(-v_value, v_value + 1);

            Vector2 pos = new Vector2(h + 0.5f, v + 0.5f);
            if (pos.x > 15)
            {
                pos = new Vector2(h_value - 0.5f, pos.y);
            }
            if (pos.y > 15)
            {
                pos = new Vector2(pos.x, v_value - 0.5f);
            }

            GetFood(0).position = pos;
        }

    //获取食物
    public Transform GetFood(int index)
        {
            Transform food;
            if (index <= foodPool.childCount)
            {
                food = Instantiate(foodPrefab.transform, foodPool);
            }
            else
            {
                food = foodPool.GetChild(index);
            }
            return food;
        }
}