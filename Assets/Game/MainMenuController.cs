using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tutorialScreen;
        [SerializeField]
        private GameObject _menu;
        private enum MainMenuState
        {
            Init,
            Tutorial
        }

        private MainMenuState _currentState = MainMenuState.Init;
        private void Start()
        {
            _tutorialScreen.SetActive(false);
            _menu.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space) && _currentState == MainMenuState.Init)
            {
                _menu.SetActive(false);
                _tutorialScreen.SetActive(true);
                StartCoroutine(GoToTutorialState());
            }
            if (Input.GetKeyUp(KeyCode.Space) && _currentState == MainMenuState.Tutorial)
            {
                SceneManager.LoadScene("SampleScene");   
            }
        }

        private IEnumerator GoToTutorialState()
        {
            yield return new WaitForSeconds(0.1f);
            _currentState = MainMenuState.Tutorial;
        }
    }
}