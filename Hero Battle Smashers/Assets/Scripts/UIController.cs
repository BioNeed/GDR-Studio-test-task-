using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private Text _enemyCountText;
    [SerializeField] private Image _enemyCountImage;
    [SerializeField] private Text _coinsCountText;
    [SerializeField] private Animator _screenAnimator;
    [SerializeField] private Text _endText;
    [SerializeField] private Text _playButtonText;
    [SerializeField] private FixedJoystick _fixedJoystick;

    private bool _tapToStart;
    private bool _tapToRestart;

    private int _startEnemyCount;

    private void Start()
    {
        _coinsCountText.text = "0";
        _tapToStart = true;
        _tapToRestart = false;
        _playButtonText.text = "Tap on the screen to play";
        _playButtonText.transform.parent.gameObject.SetActive(true);
    }

    public void SetEnemyStartCount(int count)
    {
        _startEnemyCount = count;
        _enemyCountText.text = count.ToString() + " enemies left";
        _enemyCountImage.fillAmount = 1; 
    }

    public void SetEnemyCount(int count)
    {
        _enemyCountText.text = count.ToString() + " enemies left";
        _enemyCountImage.fillAmount = (float) count / _startEnemyCount;
        Debug.Log($"{count} / {_startEnemyCount}");
    }

    public void SetCoinsCount(int count)
    {
        _coinsCountText.text = count.ToString();
    }

    public void UILose()
    {
        _screenAnimator.SetBool("End", true);
        _endText.text = "Lose";
        _tapToRestart = true;
        _playButtonText.text = "Tap on the screen to restart";
        _playButtonText.transform.parent.gameObject.SetActive(true);
    }

    public void UIWin()
    {
        _screenAnimator.SetBool("End", true);
        _endText.color = Color.green;
        _endText.text = "Win";
        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCount)
        {
            _playButtonText.text = "Tap on the screen to play next";
            _playButtonText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            _playButtonText.text = "Tap on the screen to restart";
            _playButtonText.transform.parent.gameObject.SetActive(true);
            _tapToRestart = true;
        }
    }

    public void PlayButton()
    {
        if (_tapToStart)
        {
            Time.timeScale = 1;
            _playButtonText.transform.parent.gameObject.SetActive(false);
            _fixedJoystick.gameObject.SetActive(true);
            _tapToStart = false;
        }
        else if (_tapToRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
}
