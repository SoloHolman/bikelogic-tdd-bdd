"""Pasos de los escenarios de inventario."""
from behave import given, then, when

from bikelogic.inventario.repositorio import RepositorioInventarioEnMemoria
from bikelogic.inventario.servicio import (
    MENSAJE_REGISTRADO,
    MENSAJE_RETIRO_EXITOSO,
    ServicioInventario,
)


def _servicio(context):
    """Crea, una sola vez por escenario, el servicio de inventario y su repositorio."""
    if not hasattr(context, "servicio_inventario"):
        context.servicio_inventario = ServicioInventario(RepositorioInventarioEnMemoria())
    return context.servicio_inventario


@given(
    'que el producto "{codigo}" de la categoría "{categoria}" '
    "tiene stock {stock:d}"
)
def paso_producto_con_stock(context, codigo, categoria, stock):
    _servicio(context).registrar(codigo, categoria, stock)


@when('se retiran {cantidad:d} unidades del producto "{codigo}"')
def paso_retirar_unidades(context, cantidad, codigo):
    servicio = _servicio(context)
    context.ejecutar(
        context, lambda: servicio.retirar(codigo, cantidad), MENSAJE_RETIRO_EXITOSO
    )


@when(
    'registro el producto "{codigo}" de la categoría "{categoria}" '
    "con stock inicial {stock:d}"
)
def paso_registrar_producto(context, codigo, categoria, stock):
    servicio = _servicio(context)
    context.ejecutar(
        context, lambda: servicio.registrar(codigo, categoria, stock), MENSAJE_REGISTRADO
    )


@when('registro el producto "{codigo}" sin categoría con stock inicial {stock:d}')
def paso_registrar_producto_sin_categoria(context, codigo, stock):
    servicio = _servicio(context)
    context.ejecutar(
        context, lambda: servicio.registrar(codigo, "", stock), MENSAJE_REGISTRADO
    )


@then('el stock del producto "{codigo}" es {stock:d}')
def paso_stock_del_producto(context, codigo, stock):
    assert _servicio(context).stock_de(codigo) == stock
