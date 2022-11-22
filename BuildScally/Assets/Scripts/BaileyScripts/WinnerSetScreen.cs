using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;

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


    GameObject shell;
    GameObject bob;
    GameObject aB;
    GameObject lori;


    ScoreList scoreList = new ScoreList();
    ScoreList newscoreList = new ScoreList();

    public void AddPlayers(Image a_Icon, float a_score, string a_charName, string a_prefabName)
    {
        if(scoreList.list == null)
        {
            scoreList.list = new List<PlayerScore>();
        }
        PlayerScore playerScore = new PlayerScore(a_Icon, a_score,a_charName,a_prefabName);
        scoreList.list.Add(playerScore);
    }

    //private void Start(){ Test(4); }

    public void Test(int playerCount)
    {
        newscoreList = scoreList;

        shell = Instantiate(playerResultsPrefab[0], playerSpots[0].position, playerSpots[0].rotation, transform);
        lori = Instantiate(playerResultsPrefab[1], playerSpots[1].position, playerSpots[1].rotation, transform);
        bob = Instantiate(playerResultsPrefab[2], playerSpots[2].position, playerSpots[2].rotation, transform);
        aB = Instantiate(playerResultsPrefab[3], playerSpots[3].position, playerSpots[3].rotation, transform);

        //Instantiate(playerWinnerPrefab[0], shell.transform.position, shell.transform.rotation, transform);
        //Destroy(shell.gameObject);
        //
        //Instantiate(playerWinnerPrefab[1], lori.transform.position, lori.transform.rotation, transform);
        //Destroy(lori.gameObject);
        //
        //Instantiate(playerWinnerPrefab[2], bob.transform.position, bob.transform.rotation, transform);
        //Destroy(bob.gameObject);
        //
        Instantiate(playerWinnerPrefab[3], aB.transform.position, shell.transform.rotation, transform);
        Destroy(aB.gameObject);


         Instantiate(playerLoserPrefab[0], shell.transform.position, shell.transform.rotation, transform);
         Destroy(shell.gameObject);

         Instantiate(playerLoserPrefab[1], lori.transform.position, lori.transform.rotation, transform);
         Destroy(lori.gameObject);

         Instantiate(playerLoserPrefab[2], bob.transform.position, bob.transform.rotation, transform);
         Destroy(bob.gameObject);

         //Instantiate(playerLoserPrefab[3], aB.transform.position, aB.transform.rotation, transform);
         //Destroy(aB.gameObject);
        for (int i = 0; i < playerCount; i++)
            SetUi(i, newscoreList.list[i].icon, newscoreList.list[i].score, newscoreList.list[i].charName);
    }

    public void SortScores(int playerCount)
    {
        newscoreList = scoreList;

        for(int i = 0; i < playerCount; i ++)
        {
            if(scoreList.list[i].prefabName.Contains("Shell"))
                shell = Instantiate(playerResultsPrefab[0], playerSpots[i].position, playerSpots[i].rotation, transform);
            if (scoreList.list[i].prefabName.Contains("Lori"))
                lori =Instantiate(playerResultsPrefab[1], playerSpots[i].position, playerSpots[i].rotation, transform);
            if (scoreList.list[i].prefabName.Contains("Bob"))
                bob = Instantiate(playerResultsPrefab[2], playerSpots[i].position, playerSpots[i].rotation, transform);
            if (scoreList.list[i].prefabName.Contains("Angry"))
                aB = Instantiate(playerResultsPrefab[3], playerSpots[i].position, playerSpots[i].rotation, transform);
        }



        newscoreList.list = scoreList.list.OrderBy(x => x.score).ToList();
        newscoreList.list.Reverse();


        for (int i = 0; i < playerCount; i++)
        {
            if(i == 0) // Winner Animation 
            {
                if (newscoreList.list[i].prefabName.Contains("Shell"))
                {
                    Instantiate(playerWinnerPrefab[0], shell.transform.position, shell.transform.rotation, transform);
                    //Instantiate  crown shell.transform + vectro3.up * 2
                    Destroy(shell.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Lori"))
                {
                    Instantiate(playerWinnerPrefab[1], lori.transform.position, lori.transform.rotation, transform);
                    Destroy(lori.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Bob"))
                {
                    Instantiate(playerWinnerPrefab[2], bob.transform.position, bob.transform.rotation, transform);
                    Destroy(bob.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Angry"))
                {
                    Instantiate(playerWinnerPrefab[3], aB.transform.position, shell.transform.rotation, transform);
                    Destroy(aB.gameObject);
                }
            }
            else // Loser Animation
            {
                if (newscoreList.list[i].prefabName.Contains("Shell"))
                {
                    Instantiate(playerLoserPrefab[0], shell.transform.position, shell.transform.rotation, transform);
                    Destroy(shell.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Lori"))
                {
                    Instantiate(playerLoserPrefab[1], lori.transform.position, lori.transform.rotation, transform);
                    Destroy(lori.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Bob"))
                {
                    Instantiate(playerLoserPrefab[2], bob.transform.position, bob.transform.rotation, transform);
                    Destroy(bob.gameObject);
                }

                if (newscoreList.list[i].prefabName.Contains("Angry"))
                {
                    Instantiate(playerLoserPrefab[3], aB.transform.position, aB.transform.rotation, transform);
                    Destroy(aB.gameObject);
                }
            }

            SetUi(i, newscoreList.list[i].icon, newscoreList.list[i].score, newscoreList.list[i].charName);
        }
    }

    void SetUi(int a_spot, Image a_Icon, float a_score, string a_charName)
    {
        winnerSpots[a_spot].SetActive(true);
        images[a_spot].sprite = a_Icon.sprite;
        names[a_spot].text = a_charName;
        scores[a_spot].text = a_score.ToString();
    }

}

