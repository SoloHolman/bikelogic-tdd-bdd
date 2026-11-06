"""Modelo del módulo de órdenes de compra."""
from dataclasses import dataclass

PENDIENTE = "PENDIENTE"
APROBADA = "APROBADA"


@dataclass
class OrdenCompra:
    numero: str
    proveedor: str
    monto: int
    estado: str = PENDIENTE
