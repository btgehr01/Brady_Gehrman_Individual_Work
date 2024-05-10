package edu.louisville.cse640.term_project;

import java.io.IOException;
import java.sql.Connection;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import edu.louisville.cse640.controllers.ConnectionPool;
import edu.louisville.cse640.controllers.UsersController;


/**
 * Servlet implementation class Login
 */
@WebServlet("/Login")
public class Login extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private static Connection connection       = null;
    private UsersController   uc;
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public Login() {
        super();
        // TODO Auto-generated constructor stub
    }

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String url = "/Assignment_03/Welcome.jsp";
        String user = request.getParameter("username");
        String password = request.getParameter("pw");
        System.out.println("About to connect");
        if (user == null || user.length() == 0)
        {
            url = "/Assignment_03/Login.jsp?login_error=Username must not be empty";
        }
        else
        {
            ConnectionPool pool = ConnectionPool.getInstance("jdbc/BTGEHR01");
            connection = pool.getConnection();
            if (connection != null)
            {
            	System.out.println("Connection was obtained succesfully");
                uc = new UsersController(connection);
                if (uc.findUser(user, password) == true)
                {
                	request.getSession().setAttribute("logged_in_user", user);
                }
                else
                {
                    url = "/Assignment_03/Login.jsp?login_error=Incorrect Username or Password";
                } 
                pool.freeConnection(connection);
            }
            else
            {
                System.out.println("Connection is null");
            }
        }
        response.sendRedirect(request.getContextPath() + url);
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
