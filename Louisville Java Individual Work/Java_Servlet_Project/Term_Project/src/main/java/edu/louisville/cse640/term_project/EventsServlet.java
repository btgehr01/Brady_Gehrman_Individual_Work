package edu.louisville.cse640.term_project;

import java.io.IOException;
import java.io.PrintWriter;
import java.sql.Connection;
import java.util.List;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import edu.louisville.cse640.controllers.ConnectionPool;
import edu.louisville.cse640.controllers.Event;
import edu.louisville.cse640.controllers.EventsController;

/**
 * Servlet implementation class EventsServlet
 */
@WebServlet("/EventsServlet")
public class EventsServlet extends HttpServlet {
	private static Connection connection       = null;
    private EventsController   ec;
	private static final long serialVersionUID = 1L;
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public EventsServlet() {
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
        if (connection != null) {
            System.out.println("Connection was obtained successfully");
            ec = new EventsController(connection);
            response.setContentType("text/html");
            PrintWriter out = response.getWriter();
            
            List<Event> eventsList = ec.getEventsList();

            if (eventsList != null) {
                out.println("<table border=\"1\">");
                out.println("<tr><th>Event Id</th><th>Name</th><th>Date</th><th>Location</th><th>Organizer</th><th>Host Team</th><th>Winner</th><th>Edit</th><th>Delete</th></tr>");

                for (Event event : eventsList) {
                    out.println("<tr>");
                    out.println("<td>" + event.getId() + "</td>");  // Added line to display eventId
                    out.println("<td>" + event.getName() + "</td>");
                    out.println("<td>" + event.getEventDate() + "</td>");
                    out.println("<td>" + event.getLocation() + "</td>");
                    out.println("<td>" + event.getOrganizer() + "</td>");
                    out.println("<td>" + event.getHostTeam() + "</td>");
                    out.println("<td>" + event.getWinner() + "</td>");
                    out.println("<td><a href=\"editEvent.jsp?eventId=" + event.getId() +
                    	    "&name=" + event.getName() +
                    	    "&eventDate=" + event.getEventDate() +
                    	    "&location=" + event.getLocation() +
                    	    "&winner=" + event.getWinner() + "\"><button>Edit</button></a></td>");
                    out.println("<td><a href=\"deleteEvent.jsp?eventId=" + event.getId() + "\"><button>Delete</button></a></td>");
                    out.println("</tr>");
                }
                out.println("</table>");
            } else {
                System.out.println("Fetch was Null");
            } 
            pool.freeConnection(connection);
        } else {
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
