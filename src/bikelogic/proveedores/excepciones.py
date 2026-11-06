"""Errores del módulo de proveedores."""
from bikelogic.errores import ErrorDeNegocio


class ProveedorInvalido(ErrorDeNegocio):
    """El proveedor no cumple las reglas de negocio."""


class ProveedorConOrdenesActivas(ErrorDeNegocio):
    """El proveedor no puede desactivarse porque tiene órdenes activas."""

    def __init__(self):
        super().__init__("No se puede eliminar un proveedor con órdenes activas")
