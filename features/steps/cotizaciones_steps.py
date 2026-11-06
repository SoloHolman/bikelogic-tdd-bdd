"""Pasos de los escenarios de cotizaciones."""
from behave import given, then, when

from bikelogic.cotizaciones.modelo import SolicitudCotizacion
from bikelogic.cotizaciones.repositorio import RepositorioCotizacionesEnMemoria
from bikelogic.cotizaciones.servicio import MENSAJE_REGISTRADA, ServicioCotizaciones


def _servicio(context):
    """Crea, una sola vez por escenario, el servicio de cotizaciones y su repositorio."""
    if not hasattr(context, "servicio_cotizaciones"):
        context.repositorio_cotizaciones = RepositorioCotizacionesEnMemoria()
        context.servicio_cotizaciones = ServicioCotizaciones(context.repositorio_cotizaciones)
    return context.servicio_cotizaciones


@given('que existe una solicitud "{codigo}" en estado "{estado}"')
def paso_existe_solicitud(context, codigo, estado):
    _servicio(context)
    context.repositorio_cotizaciones.agregar_solicitud(SolicitudCotizacion(codigo, estado))


@given('el proveedor "{nit}" ya cotizó la solicitud "{codigo}"')
def paso_proveedor_ya_cotizo(context, nit, codigo):
    _servicio(context).registrar(codigo, nit, 1000000)


@when(
    'el proveedor "{nit}" registra una cotización por {monto:d} '
    'para la solicitud "{codigo}"'
)
def paso_registrar_cotizacion(context, nit, monto, codigo):
    servicio = _servicio(context)
    context.ejecutar(
        context, lambda: servicio.registrar(codigo, nit, monto), MENSAJE_REGISTRADA
    )


@then("hay {cantidad:d} cotizaciones registradas")
def paso_cantidad_de_cotizaciones(context, cantidad):
    assert context.repositorio_cotizaciones.cantidad() == cantidad
