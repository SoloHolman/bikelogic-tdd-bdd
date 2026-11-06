"""Repositorio en memoria de presupuestos y órdenes de compra."""
from bikelogic.ordenes_compra.modelo import OrdenCompra


class RepositorioOrdenesEnMemoria:
    """Guarda el presupuesto de cada proveedor y las órdenes por número."""

    def __init__(self):
        self._presupuestos: dict[str, int] = {}
        self._ordenes: dict[str, OrdenCompra] = {}

    def guardar_presupuesto(self, proveedor: str, presupuesto: int) -> None:
        self._presupuestos[proveedor] = presupuesto

    def obtener_presupuesto(self, proveedor: str) -> int | None:
        return self._presupuestos.get(proveedor)

    def guardar_orden(self, orden: OrdenCompra) -> None:
        self._ordenes[orden.numero] = orden

    def obtener_orden(self, numero: str) -> OrdenCompra | None:
        return self._ordenes.get(numero)

    def listar_ordenes(self) -> list[OrdenCompra]:
        return list(self._ordenes.values())
