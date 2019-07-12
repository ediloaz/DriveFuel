@Imports MvcFlash.Core.Extensions
@imports MVC4Base

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
<body class="page-loading">
    <!-- page loading spinner -->
    <div class="pageload">
        <div class="pageload-inner">
            <div class="sk-rotating-plane"></div>
        </div>
    </div>

    <div class="app layout-fixed-header">
        <!-- sidebar panel -->
        <div class="sidebar-panel offscreen-left">
            <div class="brand">
                <!-- toggle offscreen menu -->
                <div class="toggle-offscreen">
                    <a href="javascript:;" class="visible-xs hamburger-icon" data-toggle="offscreen" data-move="ltr">
                        <span></span>
                        <span></span>
                        <span></span>
                    </a>
                </div>
                <!-- /toggle offscreen menu -->
                <!-- logo -->
                <a href="@Url.Action("Index", "Home")" class="brand-logo" style="margin-top:2px;">
                    @*<span>FUEL</span>*@
                    <img src="/Images/logonm.png" alt="Fuel Logo" class="brand-icon" style="width: 80px;margin-left: 2em;">
                </a>
                <a href="@Url.Action("Index", "Home")" class="small-menu-visible brand-logo">F</a>
                <!-- /logo -->
            </div>
            <!-- main navigation -->
            <nav role="navigation">
                @Html.Action("Menu", "Account")
            </nav>
            <!-- /main navigation -->
        </div>
        <!-- /sidebar panel -->
        <!-- content panel -->
        <div class="main-panel">
            <!-- top header -->
            <div class="header navbar">
                <div class="brand visible-xs">
                    <!-- toggle offscreen menu -->
                    <div class="toggle-offscreen">
                        <a href="javascript:;" class="hamburger-icon visible-xs" data-toggle="offscreen" data-move="ltr">
                            <span></span>
                            <span></span>
                            <span></span>
                        </a>
                    </div>
                    <!-- /toggle offscreen menu -->
                    <!-- logo -->
                    <a class="brand-logo">
                        <span>REACTOR</span>
                    </a>
                    <!-- /logo -->
                </div>
                <!--<ul class="nav navbar-nav hidden-xs">
                    <li>
                        <a href="javascript:;" class="small-sidebar-toggle ripple" data-toggle="layout-small-menu">
                            <i class="icon-toggle-sidebar"></i>
                        </a>
                    </li>
                    <li class="navbar-form search-form hide">
                        <input type="search" class="form-control search-input" placeholder="Start typing...">
                        <div class="search-predict hide">
                            <a href="#">Searching for 'purple rain'</a>
                            <div class="heading">
                                <span class="title">People</span>
                            </div>
                            <ul class="predictive-list">
                                <li>
                                    <a class="avatar" href="#">
                                        <img src="images/face1.jpg" class="img-circle" alt="">
                                        <span>Tammy Carpenter</span>
                                    </a>
                                </li>
                                <li>
                                    <a class="avatar" href="#">
                                        <img src="images/face2.jpg" class="img-circle" alt="">
                                        <span>Catherine Moreno</span>
                                    </a>
                                </li>
                                <li>
                                    <a class="avatar" href="#">
                                        <img src="images/face3.jpg" class="img-circle" alt="">
                                        <span>Diana Robertson</span>
                                    </a>
                                </li>
                                <li>
                                    <a class="avatar" href="#">
                                        <img src="images/face4.jpg" class="img-circle" alt="">
                                        <span>Emma Sullivan</span>
                                    </a>
                                </li>
                            </ul>
                            <div class="heading">
                                <span class="title">Page posts</span>
                            </div>
                            <ul class="predictive-list">
                                <li>
                                    <a class="avatar" href="#">
                                        <img src="images/unsplash/img2.jpeg" class="img-rounded" alt="">
                                        <span>The latest news for cloud-based developers </span>
                                    </a>
                                </li>
                                <li>
                                    <a class="avatar" href="#">
                                        <img src="images/unsplash/img2.jpeg" class="img-rounded" alt="">
                                        <span>Trending Goods of the Week</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </li>
                </ul>-->
                <ul class="nav navbar-nav navbar-right hidden-xs">
                    <li>
                        <a href="javascript:;" class="ripple" data-toggle="dropdown">
                            <i class="icon-bell"></i>
                        </a>
                        <ul class="dropdown-menu notifications">
                            <li class="notifications-header">
                                <p class="text-muted small">You have 3 new messages</p>
                            </li>
                            <li>
                                <ul class="notifications-list">
                                    <!--<li>
                                    <a href="javascript:;">
                                        <div class="notification-icon">
                                            <div class="circle-icon bg-success text-white">
                                                <i class="icon-bulb"></i>
                                            </div>
                                        </div>
                                        <span class="notification-message"><b>Sean</b> launched a new application</span>
                                        <span class="time">2s</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="javascript:;">
                                        <div class="notification-icon">
                                            <div class="circle-icon bg-danger text-white">
                                                <i class="icon-cursor"></i>
                                            </div>
                                        </div>
                                        <span class="notification-message"><b>Removed calendar</b> from app list</span>
                                        <span class="time">4h</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="javascript:;">
                                        <div class="notification-icon">
                                            <div class="circle-icon bg-primary text-white">
                                                <i class="icon-basket"></i>
                                            </div>
                                        </div>
                                        <span class="notification-message"><b>Denise</b> bought <b>Urban Admin Kit</b></span>
                                        <span class="time">2d</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="javascript:;">
                                        <div class="notification-icon">
                                            <div class="circle-icon bg-info text-white">
                                                <i class="icon-bubble"></i>
                                            </div>
                                        </div>
                                        <span class="notification-message"><b>Vincent commented</b> on an item</span>
                                        <span class="time">2s</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="javascript:;">
                                        <span class="notification-icon">
                                            <img src="images/face3.jpg" class="avatar img-circle" alt="">
                                        </span>
                                        <span class="notification-message"><b>Jack Hunt</b> has <b>joined</b> mailing list</span>
                                        <span class="time">9d</span>
                                    </a>
                                </li>-->
                                </ul>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <a href="javascript:;" class="ripple" data-toggle="dropdown">
                            <img src="/images/avatar.jpg" class="header-avatar img-circle" alt="user" title="user">
                            <span>@User.Identity.Name</span>
                            <span class="caret"></span>
                        </a>
                        <ul class="dropdown-menu">
                            <!--<li>
                            <a href="javascript:;">Settings</a>
                        </li>
                        <li>
                            <a href="javascript:;">Upgrade</a>
                        </li>
                        <li>
                            <a href="javascript:;">
                                <span class="label bg-danger pull-right">34</span>
                                <span>Notifications</span>
                            </a>
                        </li>
                        <li role="separator" class="divider"></li>
                        <li>
                            <a href="javascript:;">Help</a>
                        </li>-->
                            <li>
                                <a href="@Url.Action("Manage", "Account")">
                                    <i class="fa fa-key"></i>
                                    <span>Cambiar Password</span>
                                </a>
                            </li>
                            <li role="separator" class="divider"></li>
                            <li>
                                @Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With {.id = "logoutForm"})
                                @Html.AntiForgeryToken()
                                @<a href="javascript:document.getElementById('logoutForm').submit()" class="btn btn-danger btn-group btn-group-justified">
                                    <i class="fa fa-sign-out fa-fw"></i> Salir
                                </a>
                                End Using
                            </li>
                        </ul>
                    </li>
                    @*<li>
                        <a href="javascript:;" class="ripple" data-toggle="layout-chat-open">
                            <i class="icon-user"></i>
                        </a>
                    </li>*@
                </ul>
            </div>
            <!-- /top header -->
            <!-- main area -->
            <div class="main-content">
                <div class="page-title">
                    <div class="title bold">@ViewData("Title")</div>
                    <div class="sub-title">@ViewData("Descripcion")</div>
                </div>
                <div id="page-alert">
                    @Html.Flash()
                </div>
                @RenderBody()
            </div>
            <!-- /main area -->
        </div>
        <!-- /content panel -->
    </div>
    
    <div id="resources" style="display:none;">
        <i id="loadingImage" class="fa fa-refresh fa-spin"></i>
    </div>

    <script>
        var urls = {};
        var loadingImage = null;
    </script>
    @Scripts.Render("~/bundles/modernizr")
    @Scripts.Render("~/bundles/template")
    @Scripts.Render("~/bundles/blockUI")
    @Scripts.Render("~/bundles/Util")
    @RenderSection("scripts", required:=False)

    <script type="text/javascript">
        $(document).ready(function () {
            $("form").on("submit", function () {
                if ($(this).valid !== undefined) {
                    if ($(this).valid) {
                        showLoading("Cargando");
                    }
                }
            });
            loadingImage = document.getElementById("loadingImage");
            $.blockUI.defaults.baseZ = 1200;
        });
    </script>
</body>
</html>
