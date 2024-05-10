<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Add New Event</title>
</head>
<body>
<div class="teams-list">
	<jsp:include page="/TeamsServlet"></jsp:include>
</div>
    <h2>Add Event Form</h2>
    <form action="../AddEventServlet" method="post">
        <label for="name">Event Name:</label>
        <input type="text" id="name" name="name" required><br>

        <label for="eventDate">Event Date (yyyy-mm-dd):</label>
        <input type="text" id="eventDate" name="eventDate" required><br>

        <label for="location">Location:</label>
        <input type="text" id="location" name="location" required><br>

        <label for="organizer">Organizer:</label>
        <input type="text" id="organizer" name="organizer" required><br>

        <label for="hostTeam">Host Team (ID number):</label>
        <input type="text" id="hostTeam" name="hostTeam" required><br>

        <label for="winner">Winner:</label>
        <input type="text" id="winner" name="winner" required><br>

        <button type="submit">Add Event</button>
    </form>
</body>
</html>
