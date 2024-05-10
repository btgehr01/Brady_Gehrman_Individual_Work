package edu.louisville.cse640.controllers;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class TeamsController
{
    private Connection dbConnection = null;
    
    private int     id     = 0;

    public void setTeamId(int id)
    {
        this.id = id;
    }
   
    public TeamsController(Connection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    public Map<String, List<Player>> getTeamsWithPlayers() {
        Map<String, List<Player>> teamsWithPlayers = new HashMap<>();

        try {
            String template = "SELECT p.id, p.name, p.country, t.id AS team_id, t.name AS team_name, p.player_role " +
                         "FROM players p " +
                         "JOIN teams t ON p.team_id = t.id";
           
            	Statement s = dbConnection.createStatement();
                ResultSet rs = s.executeQuery(template);

                while (rs.next()) {
                    int playerId = rs.getInt("id");
                    String playerName = rs.getString("name");
                    String playerCountry = rs.getString("country");
                    int teamId = rs.getInt("team_id");
                    String teamName = rs.getString("team_name");
                    String playerRole = rs.getString("player_role");

                    Player player = new Player(playerId, playerName, playerCountry, teamId, playerRole);

                    if(teamsWithPlayers.get(teamName) != null) {
                    	teamsWithPlayers.get(teamName).add(player);
                    }else {
                    	teamsWithPlayers.put(teamName, new ArrayList<>());
                    	teamsWithPlayers.get(teamName).add(player);
                    }
                }
        } catch (SQLException e) {
        	System.out.println(e.getMessage());
        }

        return teamsWithPlayers;
    }
    
    public boolean getTeam(int id)
    {
        System.out.println("I will look for team");
        boolean rc = false;
        String template = "SELECT * FROM teams WHERE id = '" + id + "'";
        System.out.println(template);
        try
        {
            Statement s = dbConnection.createStatement();
            ResultSet rs = s.executeQuery(template);
            if (rs.next())
            {
                System.out.println("Found team");
                rc = true;
            }
            else
            {
                System.out.println("Did not find team");
            } // end if else
        }
        catch (SQLException e)
        {
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
    public ResultSet getTeamsList()
    {
        ResultSet rs = null;
        String template = "SELECT * FROM teams";
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
        return this.id;
    }

}

