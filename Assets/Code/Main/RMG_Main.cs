using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class should be duplicated since everything in it will be customized per game
public class RMG_Main : MonoBehaviour
{
    // Game screen views
    [SerializeField]
    private RMG_StartScreenView _StartScreenView;
    [SerializeField]
    private RMG_GameScreenView _GameScreenView;
    [SerializeField]
    private HelpScreenView _HelpScreenView;
    [SerializeField]
    private CreditsScreenView _CreditScreenView;
    [SerializeField]
    private PokerDeckDefinition _DeckDefinition;
 
    // Game screens
    private RMG_StartScreen _StartScreen;
    private RMG_GameScreen _GameScreen;
    private HelpScreen _HelpScreen;
    private CreditsScreen _CreditsScreen;

    private PokerDeckFactory _DeckFactory = new PokerDeckFactory();
    private StateDirector _GameScreenDirector;
    private GameScreenTags _ScreenStates = new GameScreenTags();
    private Dood _Dood = Dood.Instance;
    private PokerDeck _Deck;

    private void Awake()
    {
        _Deck = _DeckFactory.Build_DeckFromDefinition(_DeckDefinition);
        InitGameScreens(BuildGameScreens());
    }

    private void Start()
    {
        Dood.IsLogging = true;
        GameStart();
    }

    private void Update()
    {
        _GameScreenDirector.OnUpdate();
    }

    private IState[] BuildGameScreens()
    {
        _StartScreen = new RMG_StartScreen(_StartScreenView);
        _GameScreen = new RMG_GameScreen(_GameScreenView, _Deck);
        _HelpScreen = new HelpScreen(_HelpScreenView);
        _CreditsScreen = new CreditsScreen(_CreditScreenView);

        _StartScreen.ValidTransitions = new string[] { _GameScreen.Tag, _HelpScreen.Tag, _CreditsScreen.Tag };
        _GameScreen.ValidTransitions = new string[] { _StartScreen.Tag, _HelpScreen.Tag, _CreditsScreen.Tag };
        _HelpScreen.ValidTransitions = new string[] { _StartScreen.Tag, _GameScreen.Tag };
        _CreditsScreen.ValidTransitions = new string[] { _StartScreen.Tag, _GameScreen.Tag };

        IState[] gameStates = new IState[] { _StartScreen, _GameScreen, _HelpScreen, _CreditsScreen };

        return gameStates;
    }

    private void InitGameScreens(IState[] gameStates)
    {
        if (gameStates == null)
        {
            Debug.LogError("Error: Can't init game screens.");
        }

        _GameScreenDirector = new StateDirector(gameStates);
    }

    private void GameStart()
    {
        _GameScreenDirector.SetCurrentState(_ScreenStates.Start);
        _GameScreenDirector.IsActive = true;
    }
}
