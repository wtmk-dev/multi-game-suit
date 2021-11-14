using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PokerDeckDefinition 
{
    public List<Sprite> Spades;
    public List<Sprite> Hearts;
    public List<Sprite> Clubs;
    public List<Sprite> Diamonds;
    public Sprite Joker, Back, Platinum;

    public int JokerCount = 2;
    public int StanderedDeckSize = 52;
    public int CardsPerSuit = 13;    
}
