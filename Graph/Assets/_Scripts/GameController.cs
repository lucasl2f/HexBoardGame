using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    #region Player
    public enum PlayerActive { Player1, Player2 };

    PlayerActive _playerActive;

    public PlayerActive playerActive
    {
        get
        {
            return _playerActive;
        }
    }

    [SerializeField]
    Text playerActiveText;
    #endregion

    #region Cards
    public enum Cards { NONE, Construction, Destruction, Modification, SayNo, Refresh }
    Cards _cardActive;

    public void SetCardActive(int cardIndex)
    {
        _cardActive = (Cards)cardIndex;
        cardActiveText.text = _cardActive.ToString() + " in action.";
    }

    public Cards cardActive
    {
        get
        {
            return _cardActive;
        }
    }

    [SerializeField]
    Text cardActiveText;
    #endregion

    public static GameController instance;

    void Awake()
    {
        instance = this;

        _playerActive = PlayerActive.Player1;
        playerActiveText.text = _playerActive.ToString() + " playing.";
    }

    public void ChangePlayer()
    {
        if (_playerActive == PlayerActive.Player2)
        {
            _playerActive = PlayerActive.Player1;
        }
        else
        {
            _playerActive = PlayerActive.Player2;
        }

        playerActiveText.text = _playerActive.ToString() + " playing.";
    }


}