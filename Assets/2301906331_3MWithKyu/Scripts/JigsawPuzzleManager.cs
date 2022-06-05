using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class JigsawPuzzleManager : MonoBehaviour
{
    private static JigsawPuzzleManager instance;
    public static JigsawPuzzleManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<JigsawPuzzleManager>();
            return instance;
        }
    }

    [SerializeField] private JigsawPuzzleAsset[] puzzleSets;
    [SerializeField] private GameObject[] jigsawPiecesPrefab;
    [SerializeField] private GameObject[] jigsawSlots;
    [SerializeField] private GameObject jigsawPieceBox;
    [SerializeField] private GameObject jigsawPanelRoot;
    [SerializeField] private Animator fadeAC;
    [SerializeField] private Button backBtn;

    [Header("Score Panel")]
    [SerializeField] private int defaultScore = 20;
    [SerializeField] private Animator scorePanel;
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private Image completedJigsawImage;
    [SerializeField] private TMP_Text messageTxt;
    [SerializeField] private Button nextJigsawBtn, homeBtn;

    private JigsawPuzzleAsset set;
    private int solvedPuzzle = 0;
    private const int completedPuzzle = 16;

    private void Start()
    {
        // Button Sound
        backBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        homeBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });
        nextJigsawBtn.onClick.AddListener(delegate { SoundManager.Instance.buttonSound.Play(); });

        backBtn.onClick.AddListener(delegate { StartCoroutine(ILoadScene("Lobby")); });
        homeBtn.onClick.AddListener(delegate { StartCoroutine(ILoadScene("Lobby")); });
        nextJigsawBtn.onClick.AddListener(delegate { StartCoroutine(ILoadScene("JigsawPuzzle")); });

        PlayerPrefs.SetInt("AFTER_PLAY", 1);

        solvedPuzzle = 0;
        GeneratePiecesToBox();
    }

    void GeneratePiecesToBox()
    {
        set = GetRandomPuzzleSet();
        int len = jigsawPiecesPrefab.Length;
        
        // Instantiate Pieces
        GameObject temp;
        List<GameObject> pieces = new List<GameObject>();
        JigsawPiece piece;
        for (int i = 0; i < len; i++)
        {
            temp = Instantiate(jigsawPiecesPrefab[i]);
            temp.name = "piece (" + (i + 1) + ")";

            piece = temp.AddComponent<JigsawPiece>();
            piece.SetSprite(set.jigsawPieces[i]);
            piece.SetRoot(jigsawPanelRoot);

            pieces.Add(temp);
        }

        // Randomize Pieces
        System.Random random = new System.Random();
        var arr = pieces.OrderBy(x => random.Next()).ToArray();
        pieces.Clear();
        pieces.AddRange(arr);

        for (int i = 0; i < len; i++)
        {
            pieces[i].transform.SetParent(jigsawPieceBox.transform);
            pieces[i].transform.localScale = Vector3.one;
            pieces[i].transform.localPosition = Vector3.zero;
        }
    }

    JigsawPuzzleAsset GetRandomPuzzleSet()
    {
        int val = Random.Range(0, puzzleSets.Length);
        if (val == PlayerPrefs.GetInt("PUZZLE_SET", puzzleSets.Length - 1)) return GetRandomPuzzleSet();

        PlayerPrefs.SetInt("PUZZLE_SET", val);
        return puzzleSets[val];
    }

    public void StepSolvePuzzle()
    {
        solvedPuzzle++;
        if (solvedPuzzle == completedPuzzle)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SoundManager.Instance.winSound.Play();

        messageTxt.text = set.message;
        completedJigsawImage.sprite = set.completedImage;
        scoreTxt.text = "+" + defaultScore;
        GameManager.Instance.AddStars(defaultScore);

        scorePanel.gameObject.SetActive(true);
    }

    IEnumerator ILoadScene(string sceneName)
    {
        fadeAC.Play("fade_out", 0, -1f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
