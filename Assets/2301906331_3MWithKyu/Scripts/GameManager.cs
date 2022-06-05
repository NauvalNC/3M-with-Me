using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    [SerializeField] private int stars = 0;

    [Header("Star Panel")]
    [SerializeField] private TMP_Text starTxt;

    [Header("Dialogue Panel")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueTxt;
    [SerializeField] private float secondPerChar = 0.05f;
    public Queue<string> dialogues = new Queue<string>();
    private bool isDialoguePlaying = false;
    public bool isDialogueKeepActiveMode = false;

    [Header("Setting")]
    [SerializeField] private bool resetSaveData = false;

    void Start()
    {
        if (resetSaveData)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("STAR", 200);
        }

        stars = PlayerPrefs.GetInt("STAR", 0);
    }

    private void Update()
    {
        starTxt.text = GameManager.Instance.GetStars().ToString();
        ExecDialogue();
    }

    public void AddStars(int stars)
    {
        this.stars += stars;
        SaveStars();
    }

    public void SubstractStars(int stars)
    {
        this.stars -= stars;
        if (this.stars <= 0) this.stars = 0;
        SaveStars();
    }

    public int GetStars() { return stars; }

    private void SaveStars()
    {
        PlayerPrefs.SetInt("STAR", stars);
    }

    private void ExecDialogue()
    {
        if (dialogues.Count <= 0) return;
        else if (isDialoguePlaying == false)
        {
            dialoguePanel.SetActive(true);
            isDialoguePlaying = true;
            DisplayDialogue(dialogues.Peek());
        }
    }

    public void DisplayDialogue(string text)
    {
        SoundManager.Instance.bounceSound.Play();
        StartCoroutine(IDisplayDialogue(text));
    }

    IEnumerator IDisplayDialogue(string text)
    {
        dialogueTxt.text = "";

        int len = text.Length;
        for (int i = 0; i < len; i++)
        {
            dialogueTxt.text += text[i];
            yield return new WaitForSeconds(secondPerChar);
        }

        yield return new WaitForSeconds(0.8f);
        OnEndExecDialogue();
    }

    private void OnEndExecDialogue()
    {
        dialogues.Dequeue();
        isDialoguePlaying = false;
        if (dialogues.Count <= 0 && isDialogueKeepActiveMode == false)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void HideDialoguePanel() { dialoguePanel.SetActive(false); }
}
