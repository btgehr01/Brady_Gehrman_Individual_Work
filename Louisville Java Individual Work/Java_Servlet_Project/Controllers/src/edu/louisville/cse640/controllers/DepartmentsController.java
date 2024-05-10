package edu.louisville.cse640.controllers;

import java.sql.Connection;
import java.sql.Date;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class DepartmentsController
{
    private Connection dbConnection = null;
    /**
     * 
     */
    public DepartmentsController(Connection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    public int updateDepartment(int dNumber, String newName)
    {
        int rc = 0;
        String template = "UPDATE DEPARTMENTS SET DNAME=? WHERE DNUMBER=?";
        try
        {
            PreparedStatement ps = dbConnection.prepareStatement(template);
            ps.setString(1, newName);
            ps.setInt(2, dNumber);
            rc = ps.executeUpdate();
        }
        catch (SQLException e)
        {
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
    /**
     * 
     */
    public int deleteDepartment(int dNumber)
    {
        int rc = 0;
        String template = "DELETE FROM DEPARTMENTS WHERE DNUMBER=?";
        try
        {
            PreparedStatement ps = dbConnection.prepareStatement(template);
            ps.setInt(1, dNumber);
            rc = ps.executeUpdate();
        }
        catch (SQLException e)
        {
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
    /**
     * 
     */
    public int insertDepartment(String dName, int dNumber, String mgrSSN, Date mgrStartDate)
    {
        int rc = 0;
        String template = "INSERT INTO DEPARTMENTS VALUES(?,?,?,?)";
        try
        {
            PreparedStatement ps = dbConnection.prepareStatement(template);
            ps.setString(1, dName);
            ps.setInt(2, dNumber);
            ps.setString(3, mgrSSN);
            ps.setDate(4, mgrStartDate);
            rc = ps.executeUpdate();
        }
        catch (SQLException e)
        {
            System.out.println(e.getMessage());
        } // end try catch
        return (rc);
    }
    public void listDepartments()
    {
        Statement st;
        try
        {
            st = dbConnection.createStatement();
            String query = "SELECT * FROM DEPARTMENTS";
            System.out.println("List of Current departments");
            ResultSet rs = st.executeQuery(query);
            while (rs.next())
            {
                System.out.println(rs.getString(1) + " " + rs.getString(2) + " " + rs.getString(3) + " " + rs.getString(4));
                
            } // end while

            System.out.println("..............................................");
        }
        catch (SQLException e)
        {
            e.printStackTrace();
        }
    }
}
