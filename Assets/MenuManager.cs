using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public Image fade;

    private void Start()
    {
        fade.DOFade(1, 0f).SetUpdate(true);
        fade.gameObject.SetActive(true);

        Sequence startSequence = DOTween.Sequence();

        startSequence.SetUpdate(true).AppendInterval(1f).Append(fade.DOFade(0f, 3f)).OnComplete(() => fade.gameObject.SetActive(false));
    }

    public void StartGame()
    {
        fade.DOFade(0, 0f).SetUpdate(true);
        fade.gameObject.SetActive(true);

        Sequence startSequence = DOTween.Sequence();

        startSequence.SetUpdate(true).Append(fade.DOFade(1f, 3f)).AppendInterval(1f).OnComplete(() => SceneManager.LoadScene(1));
    }

    public void Option()
    {

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
