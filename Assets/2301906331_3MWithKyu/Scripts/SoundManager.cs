using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SoundManager>();
            return instance;
        }
    }

    public AudioSource buttonSound;
    public AudioSource bounceSound;
    public AudioSource falseBtnSound;
    public AudioSource winSound;
    public AudioSource coinSound;
    public AudioSource equipSound;
    public AudioSource tapSound;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
    }

    private void Start()
    {   
        DontDestroyOnLoad(gameObject);
    }
}
