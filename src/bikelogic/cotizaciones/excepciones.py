"""Errores del módulo de cotizaciones."""
from bikelogic.errores import ErrorDeNegocio


class CotizacionInvalida(ErrorDeNegocio):
    """La cotización no cumple las reglas de negocio."""
