﻿using System;

public class Joueur
{
	//Attributs
	private String nom;
	private sealed int id;
	private static int nbJoueurs = 1;
	private int bourse;
	private Main main;
    private Mise mise;
    //Constructeurs
	public Joueur(String nom, int bourse, Main main) {
        this.id = nbJoueurs;
        nbJoueurs++;
        this.nom = nom;
        this.bourse = bourse;
        this.main = main;
        this.mise = new Mise(this.id);
    }
    public Joueur(String nom, int bourse)
    {
        this.id = nbJoueurs;
        nbJoueurs++;
        this.bourse = bourse;
        this.main = new Main();
        this.mise = new Mise();
    }
    public Joueur()
    {

    }
    //Méthodes
    public void setNom(String nom)//Attribut un nouveau nom au joueur;
    {
        this.nom = nom;
    }
    public void setBourse(int bourse)//Attribut un nouveau montant pour la bourse;
    {
        this.bourse = bourse;
    }
    public void setMain(Main main)//Attribut une nouvelle Main au joueur;
    {
        this.main = main;
    }
    public void setMise(Mise m)//Attribut une nouvelle Mise au joueur;
    {
        this.mise = m;
    }
    public String getNom()//Retourne le nom du joueur;
    {
        return this.nom;
    }
    public int getID()//Retourne l'ID du joueur;
    {
        return this.id;
    }
    public int getBourse()//Retourne le montant de la bourse du joueur;
    {
        return this.nom;
    }
    public Main getMain()//Retourne la Main du joueur;
    {
        return this.main;
    }
    public Mise getMise()//Retourne la Mise du joueur;
    {
        return this.mise;
    }
    public bool equals(Joueur j)//Retourne true si les joueurs ont des attributs identiques;
    {
        if ((this.nom == j.getNom) && (this.id == j.getID) && (this.bourse == j.getBourse) && (this.main.equals(j.getMain)) && (this.mise.equals(j.getMise)))
            return true;
        return false;
    }
    public bool estLeJoueur(int id)//Retourne true si le joueur a le paramètre pour ID;
    {
        if (this.id == id)
           return true;
        return false;
    }

    /* A faire:
     * -Miser
     * */
}
