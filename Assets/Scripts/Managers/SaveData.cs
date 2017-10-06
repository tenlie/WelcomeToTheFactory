using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance { get; private set; }
    public static string SaveDate = "(non)";
    const float SaveDataVersion = 0.1f;
    const int NumOfStages = 6;

    public static int[] HiScore = new int[NumOfStages] { 0, 0, 0, 0, 0, 0 };
    public static int[] StageDifficulty = new int[NumOfStages] { 0, 0, 0, 0, 0, 0 };

    //설정 데이터 초기화
    public static string MusicOnOff = "ON";
    public static string SFXOnOff = "ON";
    public static string Language = "한국어";
    public static string Controls = "Vertical";
    public static string AutoRestartOnOff = "ON";
    public static string GooglePley = "ON";
    public static string Facebook = "OFF";

    void Awake()
    {
        Instance = this;
    }

    static void SaveDataHeader(string dataGroupName)
    {
        PlayerPrefs.SetFloat("SaveDataVersion", SaveDataVersion);
        SaveDate = System.DateTime.Now.ToString("G");
        PlayerPrefs.SetString("SaveDataDate", SaveDate);
        PlayerPrefs.SetString(dataGroupName, "on");
    }

    static bool CheckSaveDataHeader(string dataGroupName)
    {
        if (!PlayerPrefs.HasKey("SaveDataVersion"))
        {
            Debug.Log("SaveData.CheckData : No Save Data");
            return false;
        }
        if (PlayerPrefs.GetFloat("SaveDataVersion") != SaveDataVersion)
        {
            Debug.Log("SaveData.CheckData : Version Error");
            return false;
        }
        if (!PlayerPrefs.HasKey(dataGroupName))
        {
            Debug.Log("SaveData.CheckData : No Group Data");
            return false;
        }
        SaveDate = PlayerPrefs.GetString("SaveDataDate");
        return true;
    }

    //최고점수 저장
    public static bool SaveHiScore()
    {
        bool result = false;

        try
        {
            SaveDataHeader("SDG_HiScore");
            zFoxDataPackString hiscoreData = new zFoxDataPackString();
            for (int i = 0; i < 6; i++)
            {
                hiscoreData.Add("Stage" + i, HiScore[i]);
            }
            hiscoreData.PlayerPrefsSetStringUTF8("HiScoreData", hiscoreData.EncodeDataPackString());

            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SaveData.SaveHiScore : Failed (" + e.Message + ")");
        }

        return result;
    }

    //최고점수 로드
    public static bool LoadHiScore()
    {
        bool result = false;

        try
        {
            if (CheckSaveDataHeader("SDG_HiScore"))
            {
                zFoxDataPackString hiscoreData = new zFoxDataPackString();
                hiscoreData.DecodeDataPackString(hiscoreData.PlayerPrefsGetStringUTF8("HiScoreData"));
                for (int i = 0; i < 6; i++)
                {
                    HiScore[i] = (int)hiscoreData.GetData("Stage" + i);
                }
            }
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SaveData.LoadHiScore : Failed (" + e.Message + ")");
        }

        return result;
    }

    //스테이지 난이도 저장
    public static bool SaveStageDifficulty()
    {
        bool result = false;

        try
        {
            if (CheckSaveDataHeader("SDG_StageDifficulty"))
            {
                zFoxDataPackString stageDifficultyData = new zFoxDataPackString();
                stageDifficultyData.DecodeDataPackString(stageDifficultyData.PlayerPrefsGetStringUTF8("StageDifficultyData"));
                for (int i = 0; i < 6; i++)
                {
                    StageDifficulty[i] = (int)stageDifficultyData.GetData("Stage" + i);
                }
            }
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SaveData.LoadStageDifficulty : Failed (" + e.Message + ")");
        }

        return result;
    }

    //스테이지 난이도 로드
    public static bool LoadStageDifficulty()
    {
        bool result = false;

        try
        {
            SaveDataHeader("SDG_StageDifficulty");
            zFoxDataPackString stageDifficultyData = new zFoxDataPackString();
            for (int i = 0; i < 6; i++)
            {
                stageDifficultyData.Add("Stage" + i, StageDifficulty[i]);
            }
            stageDifficultyData.PlayerPrefsSetStringUTF8("LoadDifficultyData", stageDifficultyData.EncodeDataPackString());

            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SaveData.LoadStageDifficulty : Failed (" + e.Message + ")");
        }

        return result;
    }

    //설정 저장
    public static bool SaveOption()
    {
        bool result = false;

        try
        {
            SaveDataHeader("SDG_Option");
            PlayerPrefs.SetString("MusicOnOff", MusicOnOff);
            PlayerPrefs.SetString("SFXOnOff", SFXOnOff);
            PlayerPrefs.SetString("Language", Language);
            PlayerPrefs.SetString("Controls", Controls);
            PlayerPrefs.SetString("AutoRestartOnOff", AutoRestartOnOff);
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SaveData.SaveOption : Failed (" + e.Message + ")");
        }

        return result;
    }

    //설정 로드
    public static bool LoadOption()
    {
        bool result = false;

        try
        {
            if (CheckSaveDataHeader("SDG_Option"))
            {
                MusicOnOff = PlayerPrefs.GetString("MusicOnOff");
                SFXOnOff = PlayerPrefs.GetString("SFXOnOff");
                Language = PlayerPrefs.GetString("Language");
                Controls = PlayerPrefs.GetString("Controls");
                AutoRestartOnOff = PlayerPrefs.GetString("AutoRestartOnOff");
                result = true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SaveData.LoadOption : Failed (" + e.Message + ")");
        }

        return result;
    }

    //모든 데이터 초기화
    public static void DeleteAndInit(bool init)
    {
        PlayerPrefs.DeleteAll();

        if (init)
        {
            SaveDate = "(non)";
            for (int i = 0; i < NumOfStages; i++)
            {
                HiScore[i] = 0;
                StageDifficulty[i] = 0;
            }
        }
    }
}

