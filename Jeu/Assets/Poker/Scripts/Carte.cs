﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Couleur//Enumération correspondant à la couleur d'une carte
{
    Trefle = 0,
    Carreau = 1,
    Coeur = 2,
    Pique = 3
};
public enum Valeur//Enumération correspondant à la valeur d'une carte
{
    Deux = 0,
    Trois = 1,
    Quatre = 2,
    Cinq = 3,
    Six = 4,
    Sept = 5,
    Huit = 6,
    Neuf = 7,
    Dix = 8,
    Vallee = 9,
    Dame = 10,
    Roi = 11,
    As = 12
};
public enum Combinaison//Enumération correspondant aux différentes combinaisons de cartes
{
    Hauteur = 0,
    Paire = 1,
    DoublePaire = 2,
    Brelan = 3,
    Quinte = 4,
    Couleur = 5,
    Full = 6,
    Carré = 7,
    QuinteFlush = 8,
    QuinteFlushRoyal = 9
}
public class Carte : MonoBehaviour
{ 
    //Attributs
    public Sprite cardFace;//Texture de la carte
    public Couleur couleur = Couleur.Trefle;//Couleur de la carte
    public Valeur valeur = Valeur.Deux;//Valeur de la carte

    // Start is called before the first frame update
    void Start()
    {
        assignationTextureIdentite();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void assignationTextureIdentite()//Permet d'affecter les bonnes textures et les bonnes valeurs/couleurs aux cartes
    {
        List<string> deck = Poker.generatedDeck();
        Poker poker = FindObjectOfType<Poker>();
        int i = 0;
        this.couleur = 0;
        this.valeur = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                this.cardFace = poker.cardFaces[i];
                for (int j = 0; j <= 12; j++)
                {
                    for (int k = 0; k <= 3; k++)
                    {
                        if (this.name == (this.valeur + j).ToString() + " de " + (this.couleur + k).ToString() )
                        {
                            this.valeur += j;
                            this.couleur += k;
                        }
                    }
                }
            }
            i++;
        }
        GetComponent<SpriteRenderer>().sprite = this.cardFace;
    }
    public Valeur getValeur()
    {
        return this.valeur;
    }
    public Couleur getCouleur()
    {
        return this.couleur;
    }
    public bool Equals(Carte c)
    {
        if (this.couleur == c.getCouleur() && this.valeur == c.getValeur()) return true;
        return false;
    }
    public bool superieurA(Carte c)
    {
        if (this.valeur > c.valeur) return true;
        return false;
    }
}
