<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Login Page</title>
<link rel="stylesheet" type="text/css" href="../Styling/Login.css"> 
</head>
<body>
<%
		String login_error = "";
		Object error = request.getParameter("login_error");
		if (error != null)
			login_error = error.toString();
%>
<div class="login-page">
	<div class="login-modal">
		<h2>Louisville Esports Hub Login</h2>
			<form action="../Login" method="post">
				<table Style="border-collapse: separate; border-spacing: 4px;">
					<tr>
						<td>User Name:</td>
						<td><input name="username" type="text" size="20"></td>
					</tr>
					<tr>
						<td>Password:</td>
						<td><input name="pw" type="password" size="20"></td>
					</tr>
					<tr>
						<td>
						<td style="color: red"><%=login_error%></td>
						</tr>
					<tr>
						<td></td>
						<td><input type="submit" value="Login"></td>
					</tr>
			</table>
		</form>
	</div>
</div>
</body>
</html>