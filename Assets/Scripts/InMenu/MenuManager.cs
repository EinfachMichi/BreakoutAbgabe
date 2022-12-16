using System;
using UnityEngine;

namespace InMenu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject levelSelectorPanel;
        [SerializeField] private GameObject optionsPanel;

        #region Buttons

        public void OptionsButton()
        {
            menuPanel.SetActive(false);
            optionsPanel.SetActive(true);
            levelSelectorPanel.SetActive(false);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
        
        public void LevelSelectorButton()
        {
            menuPanel.SetActive(false);
            optionsPanel.SetActive(false);
            levelSelectorPanel.SetActive(true);
        }

        public void BackToMenuButton()
        {
            menuPanel.SetActive(true);
            optionsPanel.SetActive(false);
            levelSelectorPanel.SetActive(false);
        }
        
        #endregion
    }
}