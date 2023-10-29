using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BackToTitleScreen : MonoBehaviour
{

    [SerializeField] Image fade;
    public void EndGame()
    {
        fade.DOFade(0, 0f).SetUpdate(true);
        fade.gameObject.SetActive(true);

        Sequence startSequence = DOTween.Sequence();

        startSequence.SetUpdate(true).Append(fade.DOFade(1f, 3f)).AppendInterval(1f).OnComplete(() => SceneManager.LoadScene(0));
    }
}
