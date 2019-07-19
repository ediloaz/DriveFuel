@Code
    Layout = Nothing
End Code

<!DOCTYPE html>
<html>
<head>
    <title>@ViewData("Title")</title>
    @RenderSection("styles", required:=False)
</head>
<body>
    @RenderBody()
    <br/><br/>
    <img src="http://@Request.Url.Authority~/Images/logo.png" width=260>
    <br/>
    <a href="@ViewData("webPage")">@ViewData("webPage")</a>
    <br/>
    <font color= #0174DF><u><br/><p><b> Este mensaje ha sido generado de forma automática. No responder.</b></p></u></font>
    <font color= #04B404>Antes de imprimir este mensaje, asegúrese de que es realmente necesario. EL MEDIO AMBIENTE ES COSA DE TODOS.</font>
    <br/>
    <p style="font-size:10px">Aviso de Confidencialidad:Este mensaje de correo electrónico y cualquier archivo adjunto contiene información confidencial amparada por el secreto profesional, por lo tanto está exento de la obligación de divulgación de información bajo las leyes aplicables y está dirigido exclusivamente al destinatario original. Si recibe este mensaje por equivocación o si no es el destinatario original, cualquier divulgación, distribución, copia u otro uso o retención de esta comunicación o su contenido está prohibido. Si recibe este mensaje por error, le agradeceremos borre el original y todas las copias de este correo y sus archivos adjuntos de su ordenador de manera permanente. Gracias.</p>
    <p style="font-size:10px">Notice of Confidentiality: This e-mail transmission and any attachments that accompany it may contain information that is privileged, confidential or otherwise exempt from disclosure under applicable law and is intended solely for the use of the individual(s) to whom it was intended to be addressed. If you have received this e-mail by mistake, or you are not the intended recipient, any disclosure, dissemination, distribution, copying or other use or retention of this communication or its substance is prohibited. If you have received this communication in error, please delete the original and all copies of this e-mail and any attachments from your computer. Thank you.</p>
</body>
</html>
