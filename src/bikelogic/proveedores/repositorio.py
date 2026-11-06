"""Repositorio en memoria de proveedores."""
from bikelogic.proveedores.modelo import Proveedor


class RepositorioProveedoresEnMemoria:
    """Almacena los proveedores en un diccionario indexado por NIT."""

    def __init__(self):
        self._proveedores: dict[str, Proveedor] = {}

    def guardar(self, proveedor: Proveedor) -> None:
        self._proveedores[proveedor.nit] = proveedor

    def obtener(self, nit: str) -> Proveedor | None:
        return self._proveedores.get(nit)

    def existe(self, nit: str) -> bool:
        return nit in self._proveedores

    def cantidad(self) -> int:
        return len(self._proveedores)
