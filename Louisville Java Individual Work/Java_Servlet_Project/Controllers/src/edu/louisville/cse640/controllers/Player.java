package edu.louisville.cse640.controllers;

public class Player {
    private int id;
    private String name;
    private String country;
    private int teamId;
    private String playerRole;

    public Player(int id, String name, String country, int teamId, String playerRole) {
        this.id = id;
        this.name = name;
        this.country = country;
        this.teamId = teamId;
        this.playerRole = playerRole;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getCountry() {
        return country;
    }

    public int getTeamId() {
        return teamId;
    }

    public String getPlayerRole() {
        return playerRole;
    }

   
}
