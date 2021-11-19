using System.Collections;
using System.Collections.Generic;

public class HoldemHandRussianToEnglish
{
    public Dictionary<string, string> RussianToEnglish { get{ return _RussianToEnglish; } }
    private Dictionary<string, string> _RussianToEnglish = new Dictionary<string, string>();

    public HoldemHandRussianToEnglish()
    {
        Build();
    }

    private void Build()
    {
        RussianToEnglish.Add("Стрит флеш (Трефы) с 8 старшей", "Straight flush (Clubs) with 8 high");
        RussianToEnglish.Add("Стрит флеш  (Черви) с A старшей", "Straight flush (Hearts) with A high");
        RussianToEnglish.Add("Старшая карта: T", "High Card: Ten");
        RussianToEnglish.Add("Старшая карта: J", "High Card: Jack");
        RussianToEnglish.Add("Старшая карта: Q", "High Card: Queen");
        RussianToEnglish.Add("Старшая карта: K", "High Card: King");
        RussianToEnglish.Add("Старшая карта: A", "High Card: Ace");
        RussianToEnglish.Add("Три карты, T", "Three Of a Kind: Ten");
        RussianToEnglish.Add("Три карты, A", "Three Of a Kind: Ace");
        RussianToEnglish.Add("Три карты, J", "Three Of a Kind: Jack");
        RussianToEnglish.Add("Три карты, Q", "Three Of a Kind: Queen");
        RussianToEnglish.Add("Три карты, K", "Three Of a Kind: King");
        RussianToEnglish.Add("Три карты, 9", "Three Of a Kind: 9");
        RussianToEnglish.Add("Три карты, 8", "Three Of a Kind: 8");
        RussianToEnglish.Add("Три карты, 7", "Three Of a Kind: 7");
        RussianToEnglish.Add("Три карты, 6", "Three Of a Kind: 6");
        RussianToEnglish.Add("Три карты, 5", "Three Of a Kind: 5");
        RussianToEnglish.Add("Три карты, 4", "Three Of a Kind: 4");
        RussianToEnglish.Add("Три карты, 3", "Three Of a Kind: 3");
        RussianToEnglish.Add("Три карты, 2", "Three Of a Kind: 2");
        RussianToEnglish.Add("Три карты, 1", "Three Of a Kind: 1");
        RussianToEnglish.Add("Пара, T", "Pair: Ten");
        RussianToEnglish.Add("Пара, A", "Pair: Ace");
        RussianToEnglish.Add("Пара, J", "Pair: Jack");
        RussianToEnglish.Add("Пара, Q", "Pair: Queen");
        RussianToEnglish.Add("Пара, K", "Pair: King");
        RussianToEnglish.Add("Пара, 9", "Pair: 9");
        RussianToEnglish.Add("Пара, 8", "Pair: 8");
        RussianToEnglish.Add("Пара, 7", "Pair: 7");
        RussianToEnglish.Add("Пара, 6", "Pair: 6");
        RussianToEnglish.Add("Пара, 5", "Pair: 5");
        RussianToEnglish.Add("Пара, 4", "Pair: 4");
        RussianToEnglish.Add("Пара, 3", "Pair: 3");
        RussianToEnglish.Add("Пара, 2", "Pair: 2");
        RussianToEnglish.Add("Пара, 1", "Pair: 1");
        RussianToEnglish.Add("Фулхаус, K и J", "Fullhouse: K & J");
        RussianToEnglish.Add("Флеш(Черви) с Q старшей", "Flush(Hearts) with Q high");
        RussianToEnglish.Add("Стрит, 10 старшая", "Straight: 10");
        
        
    }
}
