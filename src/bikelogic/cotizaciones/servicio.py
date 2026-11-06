"""Reglas de negocio de las cotizaciones."""
from bikelogic.cotizaciones.excepciones import CotizacionInvalida
from bikelogic.cotizaciones.modelo import ABIERTA, Cotizacion

MENSAJE_REGISTRADA = "Cotización registrada"


class ServicioCotizaciones:
    """Registra cotizaciones de proveedores sobre solicitudes abiertas."""

    def __init__(self, repositorio):
        self._repositorio = repositorio

    def registrar(self, codigo_solicitud: str, nit_proveedor: str, monto: int) -> Cotizacion:
        """Registra la cotización de un proveedor si cumple las reglas del negocio."""
        self._validar(codigo_solicitud, nit_proveedor, monto)
        cotizacion = Cotizacion(codigo_solicitud, nit_proveedor, monto)
        self._repositorio.guardar(cotizacion)
        return cotizacion

    def _validar(self, codigo_solicitud: str, nit_proveedor: str, monto: int) -> None:
        solicitud = self._repositorio.obtener_solicitud(codigo_solicitud)
        if solicitud is None:
            raise CotizacionInvalida("La solicitud no existe")
        if solicitud.estado != ABIERTA:
            raise CotizacionInvalida("La solicitud no está abierta")
        if monto <= 0:
            raise CotizacionInvalida("Monto inválido")
        if self._repositorio.existe_cotizacion(codigo_solicitud, nit_proveedor):
            raise CotizacionInvalida("El proveedor ya cotizó esta solicitud")
