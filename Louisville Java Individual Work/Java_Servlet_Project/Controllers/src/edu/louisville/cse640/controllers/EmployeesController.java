package edu.louisville.cse640.controllers;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class EmployeesController {

    private Connection dbConnection = null;
    /**
     * 
     */
    public EmployeesController(Connection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    public void listEmployees()
    {
        Statement st;
        try
        {
            st = dbConnection.createStatement();
            String query = "SELECT * FROM EMPLOYEES";
            System.out.println("The query being run is:   " + query);
            System.out.println("..............................................");
            ResultSet rs = st.executeQuery(query);
            while (rs.next())
            {
                System.out.println(rs.getString(1) + " " + 
                                   rs.getString(2) + " " + 
                                   rs.getString(3) + " " + 
                                   rs.getString(4));
            } // end while
            System.out.println("..............................................");
        }
        catch (SQLException e)
        {
            e.printStackTrace();
        }
    }
    
    public int deleteEmployee(String ssn)
    {
        int rc = 0;
        String template = "DELETE FROM EMPLOYEES WHERE SSN=?";
        try
        {
            PreparedStatement ps = dbConnection.prepareStatement(template);
            ps.setString(1, ssn);
            rc = ps.executeUpdate();
        }
        catch (SQLException e)
        {
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }

}
