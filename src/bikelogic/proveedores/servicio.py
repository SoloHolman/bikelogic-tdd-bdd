"""Reglas de negocio de la gestión de proveedores."""
import re
from collections.abc import Callable

from bikelogic.proveedores.excepciones import ProveedorConOrdenesActivas, ProveedorInvalido
from bikelogic.proveedores.modelo import ACTIVO, INACTIVO, Proveedor

MENSAJE_REGISTRADO = "Proveedor registrado exitosamente"
MENSAJE_DESACTIVADO = "Proveedor desactivado"

PATRON_NIT = re.compile(r"\d{9,10}")
PATRON_CORREO = re.compile(r"[^@\s]+@[^@\s]+\.[^@\s]+")


def nit_es_valido(nit: str) -> bool:
    """Un NIT válido tiene solo dígitos y entre 9 y 10 caracteres."""
    return PATRON_NIT.fullmatch(nit) is not None


def correo_es_valido(correo: str) -> bool:
    """Un correo válido tiene la forma usuario@dominio.extensión."""
    return PATRON_CORREO.fullmatch(correo) is not None


class ServicioProveedores:
    """Registra y desactiva proveedores aplicando las reglas del negocio."""

    def __init__(
        self,
        repositorio,
        tiene_ordenes_activas: Callable[[str], bool] = lambda nit: False,
    ):
        self._repositorio = repositorio
        self._tiene_ordenes_activas = tiene_ordenes_activas

    def registrar(self, nit: str, nombre: str, correo: str) -> Proveedor:
        """Registra un proveedor nuevo en estado ACTIVO."""
        if not nombre.strip():
            raise ProveedorInvalido("El nombre es obligatorio")
        if not nit_es_valido(nit):
            raise ProveedorInvalido("NIT inválido")
        if not correo_es_valido(correo):
            raise ProveedorInvalido("Formato de correo inválido")
        if self._repositorio.existe(nit):
            raise ProveedorInvalido("El NIT ya está registrado")
        proveedor = Proveedor(nit, nombre, correo, ACTIVO)
        self._repositorio.guardar(proveedor)
        return proveedor

    def desactivar(self, nit: str) -> Proveedor:
        """Pasa un proveedor a INACTIVO si no tiene órdenes de compra activas."""
        proveedor = self._repositorio.obtener(nit)
        if proveedor is None:
            raise ProveedorInvalido("El proveedor no existe")
        if self._tiene_ordenes_activas(nit):
            raise ProveedorConOrdenesActivas()
        proveedor.estado = INACTIVO
        self._repositorio.guardar(proveedor)
        return proveedor
