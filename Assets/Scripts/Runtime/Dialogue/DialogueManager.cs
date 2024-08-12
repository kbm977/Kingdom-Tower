using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using Runtime.Managers;
using Runtime.Signals.Game;
using Runtime.Signals.UI;

namespace Runtime
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject dialogueHolder, showPos, hidePos;
        [SerializeField] Dialogue currentDialogue;
        [SerializeField]
        private LoadingScreen loadingScreen;

        public TextMeshProUGUI nameText, dialogueText;
        private Queue<string> _sentencesQ;

        private int _levelCount;

        private void Start()
        {
            _sentencesQ = new Queue<string>();
            StartDialogue(currentDialogue);
        }

        public void StartDialogue(Dialogue dialogue)
        {
            currentDialogue = dialogue;
            dialogueHolder.transform.DOMove(showPos.transform.position, 0.25f);
            nameText.text = dialogue.speakerName;

            _sentencesQ.Clear();

            foreach(string sentence in dialogue.sentences)
            {
                _sentencesQ.Enqueue(sentence);
            }
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.UIClickSFX);
            if (_sentencesQ.Count == 0 )
            {
                EndDialogue();
                return;
            }
            string sentence = _sentencesQ.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            int gibberish = Random.Range(0, AudioLibrary.Instance.SpeakGibberish.Length);
            UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.SpeakGibberish[gibberish]);
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }
        private void EndDialogue()
        {
            _levelCount++;
            if (currentDialogue.endGame) { loadingScreen.LoadScene(0); return; }
            dialogueHolder.transform.DOMove(hidePos.transform.position, 0.25f);
            GameSignals.Instance.onChangeGameState?.Invoke(GameState.Build);
        }
    }
}
