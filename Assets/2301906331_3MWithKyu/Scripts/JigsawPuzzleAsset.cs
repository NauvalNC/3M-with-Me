using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Jigsaw Puzzle", menuName = "3M with Kyu/New Jigsaw Puzzle", order = 0)]
public class JigsawPuzzleAsset : ScriptableObject
{
    public Sprite[] jigsawPieces;
    public string message;
    public Sprite completedImage;
}
