using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Provides random words from a list stored in a JSON file. Can insert them into
 * sentence templates present in Card data.
 */
public class DialogueGenerator
{
    List<string> nouns;
    List<string> adjectives;
    System.Random rng;

    /* 
     * Contruct using TextAssets references defined in Inspector. 
    */
    public DialogueGenerator(TextAsset nounsAsset, TextAsset adjectivesAsset)
    {
        rng = new System.Random();
        try
        {
            adjectives = JsonUtility.FromJson<List<string>>(adjectivesAsset.text);
            nouns = JsonUtility.FromJson<List<string>>(adjectivesAsset.text);
        }
        catch (Exception e)
        {
            Debug.LogError("Can't create DialogueGenerator due to following error: \n"
                + e.Message);
            nouns = new List<string> {"NO DATA"};
            adjectives = new List<string> {"NO DATA"};
        }
 
    }

    /* 
     * Contruct using paths to word lists in the Assets/Resources folder.
     * Uses defaults "nouns.json" and "adjectives.json".
     */
    public DialogueGenerator(
    string nounsTextAssetPath = "nouns.json",
    string adjectivesTextAssetPath = "adjectives.json") : this
    (
        Resources.Load<TextAsset>(nounsTextAssetPath),
        Resources.Load<TextAsset>(adjectivesTextAssetPath)
    )
    {
    }

    /*
     * Returns a random noun from the list.
     */
    public string GetRandomNoun()
    {
        int randomNumber = rng.Next(nouns.Count);
        return nouns[randomNumber];
    }

    /*
     * Returns a random adjective from the list.
     */
    public string GetRandomAdjective()
    {
        int randomNumber = rng.Next(adjectives.Count);
        return adjectives[randomNumber];
    }

    /*
     * Replaces markers in the string 'template': (n) with 'noun', (a) with
     * 'adjective', (c) with 'opponentName'; then returns resulting string.
     * Used for creating dialogue from templates stored in Card data.
     */
    public string GetSentence(string template, string noun, string adjective, string opponentName)
    {
        return template.Replace("(n)", noun).Replace("(a)", adjective).Replace("(c)", opponentName);

    }

}
