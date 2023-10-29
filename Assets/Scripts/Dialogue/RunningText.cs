using System.Collections;
using UnityEngine;
using TMPro;

public class RunningText : MonoBehaviour {

    [SerializeField] private TMP_Text text;

    [SerializeField] private float characterPerSecond = 64;
    private WaitForSeconds shortDelay = new WaitForSeconds(0.15f);
    private WaitForSeconds longDelay = new WaitForSeconds(0.3f);

    private static readonly char[] terminals = { '.', ',', '?', '!' };

    public void SetDialogue(string str) {
        text.SetText(str);
        StartCoroutine(RunCoroutine());
    }


    private IEnumerator RunCoroutine() {
        string str = text.text;
        int stringLength = str.Length;
        float currentChar = 0;
        int currentTarget = 0;
        WaitForSeconds terminalWait = null;
        do {
            GetNextTerminal();
            do {
                currentChar = Mathf.MoveTowards(currentChar, currentTarget, characterPerSecond * Time.deltaTime);
                text.maxVisibleCharacters = (int)currentChar;
                yield return null;
            } while (currentChar < currentTarget);
            yield return terminalWait;

        } while (currentChar < stringLength);
        yield break;

        void GetNextTerminal() {
            int position = str.IndexOfAny(terminals, currentTarget);
            if (position < 0) {
                currentTarget = stringLength;
                return;
            }
            currentTarget = position + 1;
            switch (str[position]) {
                case ',':
                    terminalWait = shortDelay;
                    break;
                case '.':
                case '!':
                case '?':
                    terminalWait = longDelay;
                    break;
            }
        }
    }


}
