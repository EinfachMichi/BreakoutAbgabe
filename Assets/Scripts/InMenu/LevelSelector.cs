 using System;
 using InGame;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;

 namespace InMenu
{
    public class LevelSelector : MonoBehaviour
    {
        public Level[] levels;
        [SerializeField] private LevelUI[] levelUIs;
        [SerializeField] private Sprite goldenStar;
        [SerializeField] private Sprite silverStar;

        private void Start()
        {
            if (SaveSystem.instance.activeSave.firstLoad)
            {
                SaveSystem.instance.activeSave.levels = levels;
                SaveSystem.instance.activeSave.firstLoad = false;
                SaveSystem.instance.Save();
            }
            levels = SaveSystem.instance.activeSave.levels;
            LoadInfos();
        }

        private void LoadInfos()
        {
            foreach (Level level in SaveSystem.instance.activeSave.levels)
            {
                if(!level.isUnlocked) continue;
                
                LoadLevelInfo(level);
            }
        }
        
        private void LoadLevelInfo(Level level)
        {
            int index = level.level - 1;
            int temp = SaveSystem.instance.activeSave.levels[index].stars;
            for (int i = 0; i < 3; i++)
            {
                if (temp > i)
                {
                    levelUIs[index].stars[i].sprite = goldenStar;
                }
                else
                {
                    levelUIs[index].stars[i].sprite = silverStar;
                }
            }
            levelUIs[index].lockedFilter.SetActive(false);
        }

        public void LoadLevelButton(int index)
        {
            if (!levels[index - 1].isUnlocked) return;
            SceneManager.LoadScene(index);
        }
    }
}

 [Serializable]
 public class Level
 {
     public int level;
     public bool isUnlocked;
     public int stars;
 }
 
 [Serializable]
 public class LevelUI
 {
     public int level;
     public Image[] stars;
     public GameObject lockedFilter;
 }