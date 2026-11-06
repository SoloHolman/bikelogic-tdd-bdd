"""Errores del módulo de órdenes de compra."""
from bikelogic.errores import ErrorDeNegocio


class OrdenInvalida(ErrorDeNegocio):
    """La orden de compra no cumple las reglas de negocio."""
