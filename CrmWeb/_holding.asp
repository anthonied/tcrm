<%
Dim objWinHttp, sHTML
Set objWinHttp = Server.CreateObject("WinHttp.WinHttpRequest.5.1")
objWinHttp.Open "GET", "http://www.host4group.co.za/H4Content/index.asp?srv=" & Request.ServerVariables("SERVER_NAME") & "&cid=" & request.querystring("cid") & "&ctg=" & request.querystring("ctg")
objWinHttp.Send
If objWinHttp.Status <> 200 Then
	Response.Write("Could not load the page. [" & objWinHttp.Status & " " & objWinHttp.StatusText & "]<br>Please try again later")
Else
	sHTML = objWinHttp.ResponseText
	Set objWinHttp = Nothing
End If
Response.Write sHTML
%>
