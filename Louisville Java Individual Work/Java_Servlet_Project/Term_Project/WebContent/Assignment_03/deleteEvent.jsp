<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Delete Event</title>
</head>
<body>
<div class="events-list">
	<h2>Esports Events</h2>
	<jsp:include page="/EventsServlet"></jsp:include>
</div>
    <h2>Delete Event Form</h2>
    <form action="../DeleteEventServlet" method="post">
        <input type="hidden" name="eventId" value="<%= request.getParameter("eventId") %>">

        <p>Are you sure you want to delete the event with Event Id: "<%= request.getParameter("eventId") %>" above?</p>

        <button type="submit">Delete Event</button>
    </form>
</body>
</html>
