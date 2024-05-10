<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Edit Event</title>
</head>
<body>
<div class="events-list">
	<h2>Esports Events</h2>
	<jsp:include page="/EventsServlet"></jsp:include>
</div>
    <h2>Edit Event Form</h2>
    <form action="../EditEventServlet" method="post">
        <input type="hidden" name="eventId" value="<%= request.getParameter("eventId") %>">

        <label for="name">Event Name:</label>
		<input type="text" id="name" name="name" value="<%= request.getParameter("name") %>" required><br>

		<label for="eventDate">Event Date:</label>
		<input type="text" id="eventDate" name="eventDate" value="<%= request.getParameter("eventDate") %>" required><br>

        <label for="location">Location:</label>
    	<input type="text" id="location" name="location" value="<%= request.getParameter("location") %>" required><br>
    
    	<label for="winner">Winner:</label>
    	<input type="text" id="winner" name="winner" value="<%= request.getParameter("winner") %>" required><br>

        <button type="submit">Edit Event</button>
    </form>
</body>
</html>
