using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PokerDeckDefinition 
{
    public List<Sprite> _Spades;
    public List<Sprite> _Hearts;
    public List<Sprite> _Clubs;
    public List<Sprite> _Diamonds;
    public Sprite _Joker, _Back, _Platinum;

    public int JokerCount = 2;
    public int StanderedDeckSize = 52;
    public int CardsPerSuit = 13;
}
