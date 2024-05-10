package edu.louisville.cse640.controllers;

import java.lang.reflect.InvocationTargetException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class DatabaseConnectionController {
	
    private String     dbName       = null;
    private String     userId       = null;
    private String     password     = null;
    private String     host         = null;
    private String     url          = null;
    private String     driver       = null;
    private Connection dbConnection = null;

    /**
     * @param dbName
     */
    public DatabaseConnectionController(String dbName)
    {
        super();
        this.dbName = dbName;
        initializeDBConnectionParameters();
        establishDBConnection();
    }
    
    public void initializeDBConnectionParameters()
    {
        driver = "com.ibm.db2.jcc.DB2Driver";
        dbName = "COMPANY";
        userId = "inimam01";
        password = "Impact160";
        host = "jdbc:db2://db2.cecsresearch.org:50000/";
        url = host + dbName;
    }
    public void establishDBConnection()
    {
        // System.out.println("Connecting to " + dbName + " database using " +  driver + ".");
        // System.out.println("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        try
        {
            Class.forName(driver).getDeclaredConstructor().newInstance();
        }
        catch (InstantiationException e)
        {
            System.out.println("Driver class cannot be instantiated.\n");
        }
        catch (IllegalAccessException e)
        {
            System.out.println("Driver class cannot be accessed.\n");
        }
        catch (ClassNotFoundException e)
        {
            System.out.println("Driver class not found, please check the PATH" + " and CLASSPATH system variables to ensure they are correct.\n");
        }
        catch (IllegalArgumentException e)
        {
            System.out.println("IllegalArgumentException.\n");
            e.printStackTrace();
        }
        catch (InvocationTargetException e)
        {
            System.out.println("IllegalArgumentException.\n");
            e.printStackTrace();
        }
        catch (NoSuchMethodException e)
        {
            System.out.println("IllegalArgumentException.\n");
            e.printStackTrace();
        }
        catch (SecurityException e)
        {
            System.out.println("IllegalArgumentException.\n");
            e.printStackTrace();
        }
        try
        {
            dbConnection = DriverManager.getConnection(url, userId, password);
            // System.out.println("Connected to database ...");
        }
        catch (SQLException sqle)
        {
            System.out.println("Cannot get database connection: " + sqle.getMessage() + "\n");
            sqle.printStackTrace();
        }
    }
    
    public void disconnectFromDatabase()
    {
        if (dbConnection != null)
        {
            try
            {
                dbConnection.close();
                // System.out.println("closed connection");
            }
            catch (Exception sqle)
            {
                System.out.println("Error closing connection");
                sqle.printStackTrace();
            }
        }
    }
    
    public Connection getDbConnection()
    {
        return dbConnection;
    }
}
