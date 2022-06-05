using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CostumeProvider : MonoBehaviour
{
    private static CostumeProvider instance;
    public static CostumeProvider Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CostumeProvider>();
            return instance;
        }
    }

    [SerializeField] private string costumeJSON = "costumes.json";
    public List<Costume> costumes = new List<Costume>();
    private int chosenSkinID;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
    }

    private void Start()
    {
        LoadCostumeData();
        chosenSkinID = PlayerPrefs.GetInt("SKIN", 0);
        
        DontDestroyOnLoad(gameObject);
    }

    public Sprite GetChosenCostume()
    {
        for (int i = 0; i < costumes.Count; i++)
        {
            if (i == chosenSkinID) return costumes[i].costume;
        }
        return null;
    }

    public int GetChosenCostumeID()
    {
        return chosenSkinID;
    }

    public void SetChosenCostumeID(int ID)
    {
        PlayerPrefs.SetInt("SKIN", ID);
        chosenSkinID = ID;
    }

    public void LoadCostumeData()
    {
        for (int i = 0; i < costumes.Count; i++)
        {
            if (i == 0)
            {
                costumes[i].unlocked = true;
            } else
            {
                costumes[i].unlocked = PlayerPrefs.GetInt("CS_Data" + i, 0) == 0 ? false : true;
            }
        }
    }

    public void SaveCostumeData()
    {
        for (int i = 0; i < costumes.Count; i++)
        {
            if (i == 0) costumes[i].unlocked = true;
            else PlayerPrefs.SetInt("CS_Data" + i, costumes[i].unlocked ? 1 : 0);
        }
    }
}
