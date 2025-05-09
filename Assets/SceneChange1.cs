using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange1 : MonoBehaviour
{

    [Header("过渡设置")]
    public Image whiteFadeImage;  // 全屏白色Image
    public float fadeDuration = 1f;  // 过渡时间
    public void LoadSceneWithWhiteFade(string sceneName)
    {
        StartCoroutine(WhiteFadeTransition(sceneName));
    }

    IEnumerator WhiteFadeTransition(string sceneName)
    {
        // 激活白屏Image
        whiteFadeImage.gameObject.SetActive(true);

        // 白屏淡入
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            whiteFadeImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // 确保完全白色
        whiteFadeImage.color = Color.white;

        // 异步加载新场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            // 当加载进度达到90%时激活场景
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // 白屏淡出
        timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            whiteFadeImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // 隐藏白屏
        whiteFadeImage.gameObject.SetActive(false);
    }
}
