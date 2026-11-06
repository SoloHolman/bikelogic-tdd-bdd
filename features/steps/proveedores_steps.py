"""Pasos de los escenarios de proveedores."""
from behave import given, then, when

from bikelogic.proveedores.repositorio import RepositorioProveedoresEnMemoria
from bikelogic.proveedores.servicio import (
    MENSAJE_DESACTIVADO,
    MENSAJE_REGISTRADO,
    ServicioProveedores,
)


def _servicio(context):
    """Crea, una sola vez por escenario, el servicio de proveedores y sus dependencias."""
    if not hasattr(context, "servicio_proveedores"):
        context.nits_con_ordenes = set()
        context.repositorio_proveedores = RepositorioProveedoresEnMemoria()
        context.servicio_proveedores = ServicioProveedores(
            context.repositorio_proveedores,
            tiene_ordenes_activas=context.nits_con_ordenes.__contains__,
        )
    return context.servicio_proveedores


@given('que no existe un proveedor con NIT "{nit}"')
def paso_no_existe_proveedor(context, nit):
    _servicio(context)
    assert not context.repositorio_proveedores.existe(nit)


@given('que ya existe un proveedor con NIT "{nit}"')
def paso_ya_existe_proveedor(context, nit):
    _servicio(context).registrar(nit, "ACME S.A.S.", "contacto@acme.com")
    context.nit_existente = nit


@given('que existe un proveedor "{nombre}" con NIT "{nit}" sin órdenes de compra activas')
def paso_proveedor_sin_ordenes(context, nombre, nit):
    _servicio(context).registrar(nit, nombre, "contacto@acme.com")


@given('que existe un proveedor "{nombre}" con NIT "{nit}" con órdenes de compra activas')
def paso_proveedor_con_ordenes(context, nombre, nit):
    _servicio(context).registrar(nit, nombre, "contacto@acme.com")
    context.nits_con_ordenes.add(nit)


@when(
    'registro el proveedor con NIT "{nit}", nombre "{nombre}" '
    'y correo "{correo}"'
)
def paso_registro_proveedor(context, nit, nombre, correo):
    servicio = _servicio(context)
    context.ejecutar(
        context, lambda: servicio.registrar(nit, nombre, correo), MENSAJE_REGISTRADO
    )


@when("intento registrar otro proveedor con el mismo NIT")
def paso_registro_duplicado(context):
    servicio = _servicio(context)
    context.ejecutar(
        context,
        lambda: servicio.registrar(context.nit_existente, "Otra empresa", "otra@empresa.com"),
        MENSAJE_REGISTRADO,
    )


@when('registro un proveedor con correo "{correo}"')
def paso_registro_con_correo(context, correo):
    servicio = _servicio(context)
    context.ejecutar(
        context,
        lambda: servicio.registrar("900999999", "Proveedor de prueba", correo),
        MENSAJE_REGISTRADO,
    )


@when('solicito desactivar el proveedor con NIT "{nit}"')
def paso_desactivar_proveedor(context, nit):
    servicio = _servicio(context)
    context.ejecutar(context, lambda: servicio.desactivar(nit), MENSAJE_DESACTIVADO)


@then('el proveedor "{nit}" queda en estado "{estado}"')
def paso_estado_del_proveedor(context, nit, estado):
    assert context.repositorio_proveedores.obtener(nit).estado == estado


@then("solo existe {cantidad:d} proveedor registrado")
def paso_cantidad_de_proveedores(context, cantidad):
    assert context.repositorio_proveedores.cantidad() == cantidad
