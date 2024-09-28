# Unity_2D_Snake_game

## Introduction to gameplay:
  In this classic snake game, players can explore an alien environment by controlling a small snake. After eating food, the snake's body will grow, but players must be careful of the terrifying skull and venomous fungus. Touching the terrifying skull will reduce the length of the snake by 1, while eating the venomous fungus will cause the snake's operation direction to reverse for a certain period of time. As the length of the snake increases, the snake's movement speed will gradually increase, and there will be greater challenges.

## Introduction to Worldview:
  The game is set in an alien world where a player-controlled snake is on a survival expedition. In this world, the snake must avoid a black hole (edge) to survive, while collecting food and props to increase its strength. The black hole symbolizes danger, and once the snake touches the edge, the game ends. The game's art style uses a unified pixel art to create a retro and unique atmosphere.

## Design implementation concept:
  The game design aims to combine classic snake gameplay with innovative props, providing a simple and easy-to-understand but challenging experience. By adding several different prop combinations, it enhances strategy, encourages players to respond flexibly to changes, and tests their skills. As the snake grows longer, the setting of faster movement speed also increases the game's tension and fun, and more elements to explore are hoped to be added in the future.

## Overall code framework explanation:
**Project Structure:**
```
snake_game/Assets/
│
├── 01_Scenes/
│   ├── GameScene (Main game logic)
│   └── MainMenu (Start navigation)
├── 02_Scripts/
│   └── snake/
├── 03_Sprites/
├── Font/
├── Prefabs/
└── Resources/
```
- **Game Engine:** Unity
- **Programming Language:** C#

**Key Modules:**
1. **Scenes:**
   - `GameScene`: Main game logic
   - `MainMenu`: Managed by `MainMenuManager.cs`
2. **Scripts:**
   - `BombData.cs`: Manages bomb logic
   - `FoodData.cs`: Manages food logic
   - `PlayerMovement.cs`: Handles player movement
   - `PoisonMushroomData.cs`: Manages poison mushroom effects
   - `UIManager.cs`: Manages UI components

3. **Sprites:** Graphic resources
4. **Font:** Custom fonts
5. **Prefabs:** Game object prefabs

## Explanation of the implementation details of each part's functions.

### Main page UI logic:
```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//定义各种按钮
public class MainMenuManager : MonoBehaviour
{
    public Button startGameButton;
    public Button settingsButton;
    public Button quitButton;
    public GameObject settingsPanel;

    private void Start()
    {
        startGameButton.onClick.AddListener(StartNewGame);
        settingsButton.onClick.AddListener(ShowSettings);
        quitButton.onClick.AddListener(QuitGame);

        // 初始时隐藏帮助和设置面板
        settingsPanel.SetActive(false);
    }
    //以下设置相关按钮作用函数，呈现在UI上
    public void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    //关闭逻辑
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
```

### Food Logic
```csharp
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
```

### Bomb Logic
```csharp
using System.Collections;
using UnityEngine;

public class BombData : MonoBehaviour
{
    public float duration = 5f; // 炸弹持续时间
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        StartCoroutine(ExplodeAfterDuration());
    }

    private IEnumerator ExplodeAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject); // 销毁炸弹
    }
    //检测碰撞触发
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) // 使用 CompareTag 方法
        {
            playerMovement.ReduceLength(); // 调用玩家减少长度的方法
            Destroy(gameObject); // 销毁炸弹
        }
    }
}
```

### Poison Mushroom Logic
```csharp
using UnityEngine;
using System.Collections;

public class PoisonMushroomData : MonoBehaviour
{
    public float duration = 5f; // 毒蘑菇效果持续时间
    public float lifetimeMin = 5f; // 毒蘑菇最短存在时间
    public float lifetimeMax = 15f; // 毒蘑菇最长存在时间

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }
    //检测碰撞触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ApplyPoisonMushroomEffect(duration);
                Destroy(gameObject); // 销毁道具
            }
        }
    }
    //触发后自动销毁
    private IEnumerator AutoDestroy()
    {
        float lifetime = Random.Range(lifetimeMin, lifetimeMax);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
```

### Game scene UI logic:
```csharp
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

```

### Snake player logic:
```csharp
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
```
---
## Existing shortcomings:
- The interface design of the game is relatively simple, and further optimization can be considered to add more details, achievements, and encouragement.
- The challenge and expansion still need to be strengthened. More props, levels, and levels can be added in subsequent versions to increase the richness of the game.
## If Yor Think it can be helpfuf please start! Thanks very very much!
