package edu.louisville.cse640.controllers;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class PlayersController
{
    private Connection dbConnection = null;
    private int     id     = 0;

    public void setTeamId(int id)
    {
        this.id = id;
    }
    
    public PlayersController(Connection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    public boolean getPlayer(int id)
    {
        System.out.println("I will look for player");
        boolean rc = false;
        String template = "SELECT * FROM players WHERE id = '" + id + "'";
        System.out.println(template);
        try
        {
            Statement s = dbConnection.createStatement();
            ResultSet rs = s.executeQuery(template);
            if (rs.next())
            {
                System.out.println("Found player");
                rc = true;
            }
            else
            {
                System.out.println("Did not find player");
            } // end if else
        }
        catch (SQLException e)
        {
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
    public ResultSet getPlayersList()
    {
        ResultSet rs = null;
        String template = "SELECT * FROM players";
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

