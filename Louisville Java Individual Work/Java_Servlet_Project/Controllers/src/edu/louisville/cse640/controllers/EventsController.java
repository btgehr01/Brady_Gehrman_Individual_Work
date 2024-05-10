package edu.louisville.cse640.controllers;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;

public class EventsController
{
    private Connection dbConnection = null;
    private int id = 0;

    public void setId(int id)
    {
        this.id = id;
    }
    
    public EventsController(Connection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    
    
    public int insertEvent(int id, String name, String event_date, String location, String organizer, int host, String winner)
    {
        int rc = 0;
        String template = "INSERT INTO events (id, name, event_date, location, organizer, host, winner)"
                + " VALUES(?, ?, ?, ?, ?, ?, ?)";
        System.out.println(template);
        try
        {
            PreparedStatement ps = dbConnection.prepareStatement(template);
            ps.setInt(1, id);
            ps.setString(2, name);
            ps.setString(3, event_date);
            ps.setString(4, location);
            ps.setString(5, organizer);
            ps.setInt(6, host);
            ps.setString(7, winner);
            rc = ps.executeUpdate();
        }
        catch (SQLException e)
        {
        	System.out.println("Error inserting event");
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
    
    public int updateEvent(int id, String name, String event_date, String location, String winner)
    {
        int rc = 0;
        String template = "UPDATE events SET name = ?, event_date = ?, location = ?, winner = ? WHERE id = ?";
        System.out.println(template);
        try
        {
            PreparedStatement ps = dbConnection.prepareStatement(template);
            ps.setString(1, name);
            ps.setString(2, event_date);
            ps.setString(3, location);
            ps.setString(4, winner);
            ps.setInt(5, id);
            rc = ps.executeUpdate();
        }
        catch (SQLException e)
        {
        	System.out.println("Error updating event");
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
    public int deleteEvent(int id)
    {
        int rc = 0;
        String template = "DELETE FROM events WHERE id=?";
        System.out.println(template);
        try
        {
            System.out.println("About to delete event " + id);
            PreparedStatement ps = dbConnection.prepareStatement(template);
            ps.setInt(1, id);
            rc = ps.executeUpdate();
        }
        catch (SQLException e)
        {
        	System.out.println("Error deleting event");
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
   
    public List<Event> getEventsList() {
        List<Event> eventsList = new ArrayList<>();

        try {
            String template = "SELECT * FROM events";
            Statement s = dbConnection.createStatement();
            ResultSet rs = s.executeQuery(template);

            while (rs.next()) {
                Event event = new Event(
                    rs.getInt("id"),
                    rs.getString("name"),
                    rs.getString("event_date"),
                    rs.getString("location"),
                    rs.getString("organizer"),
                    rs.getString("host"),
                    rs.getString("winner")
                );

                eventsList.add(event);
            }
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }

        return eventsList;
    }
    
    public ResultSet getEvent(String id)
    {
        ResultSet rs = null;
        String template = "SELECT * FROM events WHERE id=" + id;
        try
        {
            Statement s = dbConnection.createStatement();
            rs = s.executeQuery(template);
        }
        catch (SQLException e)
        {
            System.out.println(e.getMessage());
        } // end try catch
        return (rs);
    }
    public int getId()
    {
        return id;
    }

}

