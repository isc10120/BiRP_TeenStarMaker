using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueSystem;
    DialogueRunner runner;
    GameManager gameManager;

    void Start()
    {
        runner = dialogueSystem.GetComponent<DialogueRunner>();
        gameManager = GameManager.Instance;

        runner.onDialogueComplete.AddListener(OnEndDialogue);
        runner.AddCommandHandler<int>("add_money", (v) => gameManager.AddStat(StatType.Money, v));
        runner.AddCommandHandler<int>("add_kpop", (v) => gameManager.AddStat(StatType.KPop, v));
        runner.AddCommandHandler<int>("add_trot", (v) => gameManager.AddStat(StatType.Trot, v));
        runner.AddCommandHandler<int>("add_popularity", (v) => gameManager.AddStat(StatType.Popularity, v));
        runner.AddCommandHandler<int>("add_singersongwriter", (v) => gameManager.AddStat(StatType.SingerSongwriter, v));
        runner.AddCommandHandler<int>("add_visual", (v) => gameManager.AddStat(StatType.Visual, v));
    }

    public void StartDialogue(string node)
    {
        dialogueSystem.SetActive(true);
        runner.StartDialogue(node);
    }
    
    private void OnEndDialogue()
    {
        dialogueSystem.SetActive(false);
    }
}
