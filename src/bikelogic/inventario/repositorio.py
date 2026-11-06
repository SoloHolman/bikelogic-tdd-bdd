"""Repositorio en memoria de productos."""
from bikelogic.inventario.modelo import Producto


class RepositorioInventarioEnMemoria:
    """Almacena los productos en un diccionario indexado por código."""

    def __init__(self):
        self._productos: dict[str, Producto] = {}

    def guardar(self, producto: Producto) -> None:
        self._productos[producto.codigo] = producto

    def obtener(self, codigo: str) -> Producto | None:
        return self._productos.get(codigo)

    def existe(self, codigo: str) -> bool:
        return codigo in self._productos
