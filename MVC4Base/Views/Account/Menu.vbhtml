<ul class="nav">
    <li>
        <a href="@Url.Action("Index", "Rutas")">
            <i class="icon-pointer"></i>
            <span>Rutas</span>
        </a>
    </li>
    <li>
        <a href="@Url.Action("Index", "Reportes")">
            <i class="icon-users"></i>
            <span>Reportes</span>
        </a>
    </li>
    <li>
        <a href="@Url.Action("Index","Usuarios")">
            <i class="icon-user"></i>
            <span>Usuarios</span>
        </a>
    </li>
    <li>
        <a href="@Url.Action("Index", "Grupos")">
            <i class="icon-users"></i>
            <span>Grupos</span>
        </a>
    </li>
    <li>
        <a href="@Url.Action("Index", "Formas")">
            <i class="icon-layers"></i>
            <span>Formas</span>
        </a>
    </li>
    <li>
        <a href="@Url.Action("Index", "Calendario")">
            <i class="icon-calendar"></i>
            <span>Calendario</span>
        </a>
    </li>
    <li>
        <a href="@Url.Action("Index", "Mensajes")">
            <i class="icon-bubbles"></i>
            <span>Mensajes</span>
        </a>
    </li>
    <li>
        <a href="@Url.Action("Index", "Noticias", New With {.estado = "todas"})">
            <i class="icon-globe"></i>
            <span>Noticias</span>
        </a>
    </li>
    <li class="menu-accordion">
        <a href="#">
            <i class="icon-graduation"></i>
            <span>Capacitaciones</span>
        </a>
        <ul class="sub-menu">
            <li>
                <a href="@Url.Action("Index", "Capacitacion", New With {.estado = "todas"})">
                    <span>Consulta</span>
                </a>
            </li>
            <li>
                <a href="@Url.Action("Create", "Capacitacion")">
                    <span>Crea Capacitación</span>
                </a>
            </li>
            <li>
                <a href="@Url.Action("Visitas", "Capacitacion")">
                    <span>Actividad Reciente</span>
                </a>
            </li>
            <li>
                <a href="@Url.Action("Inactividad", "Capacitacion")">
                    <span>Inactividad</span>
                </a>
            </li>
        </ul>
    </li>
    <li class="menu-accordion">
        <a href="#">
            <i class="icon-notebook"></i>
            <span>Administración</span>
        </a>
        <ul class="sub-menu">
            <li>
                <a href="@Url.Action("Index", "Clientes", New With {.estado = "todas"})">
                    <span>Clientes</span>
                </a>
            </li>
            <li>
                <a href="@Url.Action("Index", "Productos")">
                    <span>Productos</span>
                </a>
            </li>
        </ul>
    </li>
</ul>