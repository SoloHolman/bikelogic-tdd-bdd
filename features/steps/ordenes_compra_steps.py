"""Pasos de los escenarios de órdenes de compra."""
from behave import given, then, when

from bikelogic.ordenes_compra.modelo import APROBADA
from bikelogic.ordenes_compra.repositorio import RepositorioOrdenesEnMemoria
from bikelogic.ordenes_compra.servicio import MENSAJE_APROBADA, ServicioOrdenesCompra


def _pesos(texto):
    """Convierte un monto con separadores de miles ("10.000.000") en un entero."""
    return int(texto.replace(".", ""))


def _servicio(context):
    """Crea, una sola vez por escenario, el servicio de órdenes y su repositorio."""
    if not hasattr(context, "servicio_ordenes"):
        context.repositorio_ordenes = RepositorioOrdenesEnMemoria()
        context.servicio_ordenes = ServicioOrdenesCompra(context.repositorio_ordenes)
    return context.servicio_ordenes


@given('que el proveedor "{proveedor}" tiene un presupuesto disponible de ${monto}')
def paso_proveedor_con_presupuesto(context, proveedor, monto):
    _servicio(context).registrar_proveedor(proveedor, _pesos(monto))
    context.proveedor = proveedor


@given('existe una orden "{numero}" por ${monto} en estado "{estado}"')
def paso_existe_orden(context, numero, monto, estado):
    servicio = _servicio(context)
    servicio.registrar_orden(numero, context.proveedor, _pesos(monto))
    if estado == APROBADA:
        servicio.aprobar(numero)


@when('el jefe de compras aprueba la orden "{numero}"')
def paso_aprobar_orden(context, numero):
    servicio = _servicio(context)
    context.ejecutar(context, lambda: servicio.aprobar(numero), MENSAJE_APROBADA)


@then('la orden "{numero}" queda en estado "{estado}"')
def paso_estado_de_la_orden(context, numero, estado):
    assert context.repositorio_ordenes.obtener_orden(numero).estado == estado


@then('el presupuesto del proveedor "{proveedor}" queda en ${monto}')
def paso_presupuesto_del_proveedor(context, proveedor, monto):
    assert _servicio(context).presupuesto_de(proveedor) == _pesos(monto)
