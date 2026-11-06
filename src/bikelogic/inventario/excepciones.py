"""Errores del módulo de inventario."""
from bikelogic.errores import ErrorDeNegocio


class ProductoInvalido(ErrorDeNegocio):
    """El producto no cumple las reglas de negocio."""


class StockInsuficiente(ErrorDeNegocio):
    """Se intentó retirar más unidades de las disponibles."""

    def __init__(self):
        super().__init__("Stock insuficiente")
