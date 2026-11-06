"""Repositorio en memoria de solicitudes y cotizaciones."""
from bikelogic.cotizaciones.modelo import Cotizacion, SolicitudCotizacion


class RepositorioCotizacionesEnMemoria:
    """Guarda las solicitudes por código y las cotizaciones registradas."""

    def __init__(self):
        self._solicitudes: dict[str, SolicitudCotizacion] = {}
        self._cotizaciones: list[Cotizacion] = []

    def agregar_solicitud(self, solicitud: SolicitudCotizacion) -> None:
        self._solicitudes[solicitud.codigo] = solicitud

    def obtener_solicitud(self, codigo: str) -> SolicitudCotizacion | None:
        return self._solicitudes.get(codigo)

    def existe_cotizacion(self, codigo_solicitud: str, nit_proveedor: str) -> bool:
        return any(
            c.codigo_solicitud == codigo_solicitud and c.nit_proveedor == nit_proveedor
            for c in self._cotizaciones
        )

    def guardar(self, cotizacion: Cotizacion) -> None:
        self._cotizaciones.append(cotizacion)

    def cantidad(self) -> int:
        return len(self._cotizaciones)
