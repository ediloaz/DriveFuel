<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1, maximum-scale=1">
    <title>@ViewBag.Title</title>
    <link href="/Images/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <link href="http://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700&amp;subset=latin" rel="stylesheet">

    @Styles.Render("~/Content/template")
    @Styles.Render("~/Content/base")
    

    @RenderSection("styles", required:=False)

    <script type="text/javascript">
        paceOptions = {
            ajax: false, // disabled
            document: true, // disabled
            eventLag: false // disabled
        };
    </script>

    @Scripts.Render("~/bundles/modernizr")
</head>
<body>
    @RenderBody()
    @Scripts.Render("~/bundles/template")
    @RenderSection("scripts", required:=False)
</body>
</html>
