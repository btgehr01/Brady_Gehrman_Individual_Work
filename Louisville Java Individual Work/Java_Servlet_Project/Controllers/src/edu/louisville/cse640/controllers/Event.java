package edu.louisville.cse640.controllers;

public class Event {
    private final int id;
    private final String name;
    private final String eventDate;
    private final String location;
    private final String organizer;
    private final String hostTeam;
    private final String winner;

    public Event(int id, String name, String eventDate, String location, String organizer, String hostTeam, String winner) {
        this.id = id;
        this.name = name;
        this.eventDate = eventDate;
        this.location = location;
        this.organizer = organizer;
        this.hostTeam = hostTeam;
        this.winner = winner;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getEventDate() {
        return eventDate;
    }

    public String getLocation() {
        return location;
    }

    public String getOrganizer() {
        return organizer;
    }

    public String getHostTeam() {
        return hostTeam;
    }

    public String getWinner() {
        return winner;
    }
}

