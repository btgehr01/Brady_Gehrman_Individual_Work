package edu.louisville.cse640.controllers;

import java.io.PrintWriter;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;

public class SQLUtil {
    public static String getHtmlTable(ResultSet results) throws SQLException
    {
        StringBuffer htmlRows = new StringBuffer();
        ResultSetMetaData metaData = results.getMetaData();
        int columnCount = metaData.getColumnCount();
        htmlRows.append("<table cellpadding=\"5\" border=\"1\">");
        htmlRows.append("<tr>");
        for (int i = 1; i <= columnCount; i++)
            htmlRows.append("<td><b>" + metaData.getColumnName(i) + "</td>");
        htmlRows.append("</tr>");
        while (results.next())
        {
            htmlRows.append("<tr>");
            for (int i = 1; i <= columnCount; i++)
                htmlRows.append("<td>" + results.getString(i) + "</td>");
        }
        htmlRows.append("</tr>");
        htmlRows.append("</table>");
        return htmlRows.toString();
    }
    /**
     * @param rs
     * @param out
     */
    public void tabulateResultSet(ResultSet rs, PrintWriter out)
    {
        int columnCount = 0;
        ResultSetMetaData metaData;
        out.println("<table cellpadding=\"5\" border=\"2\">");
        out.println("<tr>");
        try
        {
            metaData = rs.getMetaData();
            columnCount = metaData.getColumnCount();
            for (int i = 1; i <= columnCount; i++)
            {
                out.println("<td><b>" + metaData.getColumnName(i) + "</td>");
            }
            out.println("</tr>");
            while (rs.next())
            {
                out.println("<tr>");
                for (int i = 1; i <= columnCount; i++)
                {
                    out.println("<td>" + rs.getString(i) + "</td>");
                } // end for
            } // end while
        }
        catch (SQLException sqle)
        {
            System.out.println("Could not read result set");
            sqle.printStackTrace();
        }
        out.println("</tr>");
        out.println("</table>");
    }
}
