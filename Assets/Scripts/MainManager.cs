using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;


    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text BestScoreNameText;
    public Text ScoreText;
    public GameObject GameOverText;

    private int bestScore;
    private string bestPlayerName;

    

    
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    public void Awake()
    {
        Load();
        if (Instance !=null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

    }


    // Start is called before the first frame update
    
    void Start()
    {
        BestScoreNameText.text = $"Name: {bestPlayerName} - Best Score :{bestScore}";

        Debug.Log(MenuUIHandler.playerName);
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if(m_Points > bestScore)
        {
            bestScore = m_Points;
            bestPlayerName = MenuUIHandler.playerName;
            Save();
        }
        BestScoreNameText.text = $"Name : {bestPlayerName} - Best Score:{bestScore}";
        Debug.Log("Game Over çalýþtý");


        m_GameOver = true;
        GameOverText.SetActive(true);
    }


    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestPlayerName;
    }
    public void Save()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestPlayerName = bestPlayerName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestScore = data.bestScore;
            bestPlayerName = data.bestPlayerName;
        }
    }
}
