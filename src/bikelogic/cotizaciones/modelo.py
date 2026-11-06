"""Modelo del módulo de cotizaciones."""
from dataclasses import dataclass

ABIERTA = "ABIERTA"
CERRADA = "CERRADA"


@dataclass
class SolicitudCotizacion:
    codigo: str
    estado: str = ABIERTA


@dataclass(frozen=True)
class Cotizacion:
    codigo_solicitud: str
    nit_proveedor: str
    monto: int
