<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Louisville Esports Hub</title>
<link rel="stylesheet" type="text/css" href="../Styling/Welcome.css"> 
<script>
function loginButtonPress(){
	window.location.href = "./Login.jsp";
}
</script>
</head>
<body>
<%
	String buttonText = "Login";
	Object loginUserObj = request.getSession().getAttribute("logged_in_user");
	if(loginUserObj != null){
		buttonText = loginUserObj.toString();
	}
	String welcomeMessage = buttonText.equals("Login") ? "Welcome to the Louisville Cardinals Esports Hub" : "Welcome to the Louisville Cardinals Esports Hub. Thank you for logging in!";
%>
<div class="top-nav">
	<h1>Louisville Esports Hub</h1>
	<button onclick="loginButtonPress()" <%= buttonText.equals("Login") ? "" : "disabled" %>><%= buttonText%></button>
</div>

<div class="welcome-bio">
	<h2><%= welcomeMessage%></h2>
	<p>The go to spot for Louisville Esports Media. Here you can view all of our fantastic esports teams, student athletes, and even our upcoming events!</p>
</div>
<div class="teams-list">
	<jsp:include page="/TeamsServlet"></jsp:include>
</div>

<div class="events-list">
	<h2>Esports Events</h2>
	<jsp:include page="/EventsServlet"></jsp:include>
	<div class="buttons-row">
        <a href="addEvent.jsp"><button>Add Event</button></a>
    </div>
</div>

<div>
	<img src="../Images/EsportsLogo.png" alt="Welcome Image" class="welcome-img">
</div>
</body>
</html>