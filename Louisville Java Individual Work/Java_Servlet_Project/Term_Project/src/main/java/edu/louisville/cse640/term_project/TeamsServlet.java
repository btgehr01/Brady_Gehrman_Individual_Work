package edu.louisville.cse640.term_project;

import java.io.IOException;
import java.io.PrintWriter;
import java.sql.Connection;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import edu.louisville.cse640.controllers.ConnectionPool;
import edu.louisville.cse640.controllers.TeamsController;

/**
 * Servlet implementation class TeamsServlet
 */
@WebServlet("/TeamsServlet")
public class TeamsServlet extends HttpServlet {
	private static Connection connection       = null;
    private TeamsController   tc;
	private static final long serialVersionUID = 1L;
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public TeamsServlet() {
        super();
        // TODO Auto-generated constructor stub
    }

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		System.out.println("About to connect");
        ConnectionPool pool = ConnectionPool.getInstance("jdbc/BTGEHR01");
        connection = pool.getConnection();
            if (connection != null)
            {
            	System.out.println("Connection was obtained succesfully");
            	tc = new TeamsController(connection);
            	response.setContentType("text/html");
                PrintWriter out = response.getWriter();
                if (tc.getTeamsWithPlayers() != null)
                {
                	out.println("<h2>University of Louisville Esport Teams</h2>");
                    out.println("<table border=\"1\">");
                    out.println("<tr><th>Team ID</th><th>Team</th><th>Players</th></tr>");

                    tc.getTeamsWithPlayers().forEach((teamName, players) -> {
                        out.println("<tr>");
                        int teamId = players.get(0).getTeamId();
                        out.println("<td>" + teamId + "</td>");
                        out.println("<td>" + teamName + "</td>");
                        out.println("<td>");

                        players.forEach(player -> out.println(player.getName() + " (" + player.getPlayerRole() + ") " + "| "));

                        out.println("</td>");
                        out.println("</tr>");
                    });

                    out.println("</table>");
                }
                else
                {
                	System.out.println("Fetch was Null");
                } 
                pool.freeConnection(connection);
            }
            else
            {
                System.out.println("Connection is null");
            }
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
