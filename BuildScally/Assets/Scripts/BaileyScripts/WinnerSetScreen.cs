using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class WinnerSetScreen : MonoBehaviour
{
    class ScoreList
    {
        public List<PlayerScore> list;
    }
    class PlayerScore
    {
        public string charName;
        public float score;
        public Image icon;
        public string prefabName;

        public PlayerScore(Image a_Icon, float a_score, string a_charName, string a_prefabName)
        {
            icon = a_Icon;
            score = a_score;
            charName = a_charName;
            prefabName = a_prefabName;
        }

    }
    public List<GameObject> winnerSpots = new List<GameObject>();
    public List<Image> images = new List<Image>();
    public List<TextMeshProUGUI> names = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> scores = new List<TextMeshProUGUI>();
    public List<Transform> playerSpots = new List<Transform>();
    public List<GameObject> playerResultsPrefab = new List<GameObject> ();
    public List<GameObject> playerWinnerPrefab = new List<GameObject>();
    public List<GameObject> playerLoserPrefab = new List<GameObject>();
    public Button playAgain;


    GameObject shell;
    GameObject bob;
    GameObject aB;
    GameObject lori;


    ScoreList scoreList = new ScoreList();
    ScoreList newscoreList = new ScoreList();

    public Camera resultCam;

    public void AddPlayers(Image a_Icon, float a_score, string a_charName, string a_prefabName)
    {
        //adds all the playhers to a list with their index and score
        if(scoreList.list == null)
        {
            scoreList.list = new List<PlayerScore>();
        }
        PlayerScore playerScore = new PlayerScore(a_Icon, a_score,a_charName,a_prefabName);
        scoreList.list.Add(playerScore);
    }

    private void Start()
    {
        //Test();
    }

    void Test()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
                shell = Instantiate(playerResultsPrefab[0], playerSpots[i].position, playerSpots[i].rotation, transform);
            if (i == 1)
                lori = Instantiate(playerResultsPrefab[1], playerSpots[i].position, playerSpots[i].rotation, transform);
            if (i == 2)
                bob = Instantiate(playerResultsPrefab[2], playerSpots[i].position, playerSpots[i].rotation, transform);
            if (i == 3)
                aB = Instantiate(playerResultsPrefab[3], playerSpots[i].position, playerSpots[i].rotation, transform);
        }

        Instantiate(playerLoserPrefab[2], shell.transform.position,shell.transform.rotation *= Quaternion.Euler(0,-90,0), resultCam.transform);
        Instantiate(playerLoserPrefab[2], lori.transform.position, lori.transform.rotation *= Quaternion.Euler(0, -90, 0), resultCam.transform);
        Instantiate(playerLoserPrefab[2], bob.transform.position, bob.transform.rotation *= Quaternion.Euler(0, -90, 0), resultCam.transform);
        Instantiate(playerLoserPrefab[2], aB.transform.position, aB.transform.rotation *= Quaternion.Euler(0, -90, 0), resultCam.transform);

        Destroy(aB.gameObject);Destroy(bob.gameObject);Destroy(lori.gameObject);Destroy(shell.gameObject);
    }

    public void SortScores(int playerCount)
    {
        newscoreList = scoreList;
        //spawns the playhers location based on their character they have chosen 
        for(int i = 0; i < playerCount; i ++)
        {
            if(scoreList.list[i].prefabName.Contains("Shell"))
                shell = Instantiate(playerResultsPrefab[0], playerSpots[i].position, playerSpots[i].rotation, resultCam.transform);
            if (scoreList.list[i].prefabName.Contains("Lori"))
                lori =Instantiate(playerResultsPrefab[1], playerSpots[i].position, playerSpots[i].rotation, resultCam.transform);
            if (scoreList.list[i].prefabName.Contains("Bob"))
                bob = Instantiate(playerResultsPrefab[2], playerSpots[i].position, playerSpots[i].rotation, resultCam.transform);
            if (scoreList.list[i].prefabName.Contains("Angry"))
                aB = Instantiate(playerResultsPrefab[3], playerSpots[i].position, playerSpots[i].rotation, resultCam.transform);
        }



        newscoreList.list = scoreList.list.OrderBy(x => x.score).ToList();
        newscoreList.list.Reverse();

        // spawns in the animation and prefab
        for (int i = 0; i < playerCount; i++)
        {
            if(i == 0) // Winner Animation 
            {
                if (newscoreList.list[i].prefabName.Contains("Shell"))
                {
                    Instantiate(playerWinnerPrefab[0], shell.transform.position, shell.transform.rotation, resultCam.transform);
                    Destroy(shell.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Lori"))
                {
                    Instantiate(playerWinnerPrefab[1], lori.transform.position, lori.transform.rotation, resultCam.transform);
                    Destroy(lori.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Bob"))
                {
                    Instantiate(playerWinnerPrefab[2], bob.transform.position, bob.transform.rotation, resultCam.transform);
                    Destroy(bob.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Angry"))
                {
                    Instantiate(playerWinnerPrefab[3], aB.transform.position, aB.transform.rotation, resultCam.transform);
                    Destroy(aB.gameObject);
                }
            }
            else // Loser Animation
            {
                if (newscoreList.list[i].prefabName.Contains("Shell"))
                {
                    Instantiate(playerLoserPrefab[0], shell.transform.position, shell.transform.rotation, resultCam.transform);
                    Destroy(shell.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Lori"))
                {
                    Instantiate(playerLoserPrefab[1], lori.transform.position, lori.transform.rotation, resultCam.transform);
                    Destroy(lori.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Bob"))
                {
                    Instantiate(playerLoserPrefab[2], bob.transform.position, bob.transform.rotation *= Quaternion.Euler(0, -90, 0), resultCam.transform);
                    Destroy(bob.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Angry"))
                {
                    Instantiate(playerLoserPrefab[3], aB.transform.position, aB.transform.rotation, resultCam.transform);
                    Destroy(aB.gameObject);
                }
            }

            SetUi(i, newscoreList.list[i].icon, newscoreList.list[i].score, newscoreList.list[i].charName);
        }
        // selects the play again button
        if (playAgain != null)
            playAgain.Select();
    }
    //sets the ui for the player in each position
    void SetUi(int a_spot, Image a_Icon, float a_score, string a_charName)
    {
        winnerSpots[a_spot].SetActive(true);
        images[a_spot].sprite = a_Icon.sprite;
        names[a_spot].text = a_charName;
        scores[a_spot].text = a_score.ToString();
    }

}

