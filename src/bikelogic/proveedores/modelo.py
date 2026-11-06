"""Modelo del módulo de proveedores."""
from dataclasses import dataclass

ACTIVO = "ACTIVO"
INACTIVO = "INACTIVO"


@dataclass
class Proveedor:
    nit: str
    nombre: str
    correo: str
    estado: str = ACTIVO
