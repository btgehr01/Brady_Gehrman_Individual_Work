package edu.louisville.cse640.term_project;

import java.io.IOException;
import java.sql.Connection;
import java.util.Random;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import edu.louisville.cse640.controllers.ConnectionPool;
import edu.louisville.cse640.controllers.EventsController;

/**
 * Servlet implementation class AddEventServlet
 */
@WebServlet("/AddEventServlet")
public class AddEventServlet extends HttpServlet {
	private static Connection connection       = null;
    private EventsController   ec;
	private static final long serialVersionUID = 1L;
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public AddEventServlet() {
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
		            
		            String name = request.getParameter("name");
		            String eventDate = request.getParameter("eventDate");
		            String location = request.getParameter("location");
		            String organizer = request.getParameter("organizer");
		            int hostTeam = Integer.parseInt(request.getParameter("hostTeam"));
		            String winner = request.getParameter("winner");
		            
		            Random random = new Random();
		            int randomId = random.nextInt(Integer.MAX_VALUE);
		            
		            try {
		            	System.out.println("Atempting event insert");
		                ec.insertEvent(randomId, name, eventDate, location, organizer, hostTeam, winner);

		                response.sendRedirect("Assignment_03/Welcome.jsp");
		            } finally {
		                pool.freeConnection(connection);
		            }
		        } else {
		            System.out.println("Connection is null");
		            response.sendRedirect("Assignment_03/error.jsp");
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
