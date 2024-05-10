package edu.louisville.cse640.term_project;

import java.io.IOException;
import java.sql.Connection;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import edu.louisville.cse640.controllers.ConnectionPool;
import edu.louisville.cse640.controllers.EventsController;

/**
 * Servlet implementation class DeleteEventServlet
 */
@WebServlet("/DeleteEventServlet")
public class DeleteEventServlet extends HttpServlet {
	private static Connection connection       = null;
    private EventsController   ec;
	private static final long serialVersionUID = 1L;
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public DeleteEventServlet() {
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
		            
		            String eventIdParam = request.getParameter("eventId");
		            int eventId = 0;
		            
		            if (eventIdParam != null) {
		                
		                eventId = Integer.parseInt(eventIdParam);

		            }
		            
		            try {
		               
		                ec.deleteEvent(eventId);

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
