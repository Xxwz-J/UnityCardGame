using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SlotScript : MonoBehaviour, IDropHandler
{
    // 配置项：在 Inspector 中设置场景名称（或使用常量）
    [SerializeField] private string gameSceneName = "PlayScenes";

    // 缓存场景验证结果（优化性能）
    private bool _isSceneValid;

    void Start()
    {
        // 启动时预验证场景有效性
        _isSceneValid = SceneExists(gameSceneName);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        ItemDrag item = dropped.GetComponent<ItemDrag>();
        if (item == null) return;

        HandleItemAction(item.itemType);
    }

    private void HandleItemAction(ItemDrag.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemDrag.ItemType.NewGame:
                ExecuteNewGame();
                break;

            case ItemDrag.ItemType.Continue:
                Debug.Log("触发继续游戏事件");
                // 建议添加存档加载逻辑
                break;

            case ItemDrag.ItemType.Sound:
                ToggleSound();
                break;

            case ItemDrag.ItemType.Exit:
                QuitApplication();
                break;
        }
    }

    private void ExecuteNewGame()
    {
        Debug.Log("触发新游戏事件");

        // 版本1：直接场景跳转
        if (_isSceneValid)
        {
            // 重置游戏状态（按需添加）
            // PlayerPrefs.DeleteAll();
            // GameManager.Instance.ResetGame();

            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError($"场景加载失败: {gameSceneName} 不存在于构建设置");
            // 可以触发UI错误提示
        }

        // 版本2：异步加载（需要配合加载界面）
        // StartCoroutine(LoadSceneAsync());
    }

    #region 场景加载辅助方法
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (System.IO.Path.GetFileNameWithoutExtension(scenePath) == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneName);
        operation.allowSceneActivation = false;

        // 模拟加载过程（实际项目需连接真实进度）
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // 更新进度条（需要引用UI组件）
            // loadingBar.fillAmount = progress;

            if (progress >= 0.9f)
            {
                // 等待任意按键或自动继续
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    #endregion

    #region 其他功能模块
    private void ToggleSound()
    {
        Debug.Log("触发声音设置事件");
        // 实际实现示例：
        // AudioListener.volume = AudioListener.volume > 0 ? 0 : 1;
    }

    private void QuitApplication()
    {
        Debug.Log("触发退出游戏事件");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}