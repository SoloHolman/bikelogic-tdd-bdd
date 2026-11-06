"""Reglas de negocio de las órdenes de compra."""
from bikelogic.ordenes_compra.excepciones import OrdenInvalida
from bikelogic.ordenes_compra.modelo import APROBADA, PENDIENTE, OrdenCompra

MENSAJE_APROBADA = "Orden aprobada"


class ServicioOrdenesCompra:
    """Gestiona el presupuesto de los proveedores y la aprobación de sus órdenes."""

    def __init__(self, repositorio):
        self._repositorio = repositorio

    def registrar_proveedor(self, proveedor: str, presupuesto: int) -> None:
        """Registra un proveedor con su presupuesto disponible."""
        if presupuesto < 0:
            raise OrdenInvalida("El presupuesto no puede ser negativo")
        self._repositorio.guardar_presupuesto(proveedor, presupuesto)

    def registrar_orden(self, numero: str, proveedor: str, monto: int) -> OrdenCompra:
        """Registra una orden PENDIENTE para un proveedor existente."""
        self.presupuesto_de(proveedor)
        if self._repositorio.obtener_orden(numero) is not None:
            raise OrdenInvalida("El número de orden ya está registrado")
        orden = OrdenCompra(numero, proveedor, monto)
        self._repositorio.guardar_orden(orden)
        return orden

    def aprobar(self, numero: str) -> OrdenCompra:
        """Aprueba una orden pendiente y descuenta su monto del presupuesto."""
        orden = self._orden_existente(numero)
        if orden.estado == APROBADA:
            raise OrdenInvalida("La orden ya está aprobada")
        presupuesto = self.presupuesto_de(orden.proveedor)
        if orden.monto > presupuesto:
            raise OrdenInvalida("Presupuesto insuficiente")
        orden.estado = APROBADA
        self._repositorio.guardar_presupuesto(orden.proveedor, presupuesto - orden.monto)
        return orden

    def presupuesto_de(self, proveedor: str) -> int:
        presupuesto = self._repositorio.obtener_presupuesto(proveedor)
        if presupuesto is None:
            raise OrdenInvalida("El proveedor no existe")
        return presupuesto

    def tiene_ordenes_activas(self, proveedor: str) -> bool:
        """Una orden está activa mientras esté pendiente de aprobación."""
        return any(
            orden.proveedor == proveedor and orden.estado == PENDIENTE
            for orden in self._repositorio.listar_ordenes()
        )

    def _orden_existente(self, numero: str) -> OrdenCompra:
        orden = self._repositorio.obtener_orden(numero)
        if orden is None:
            raise OrdenInvalida("La orden no existe")
        return orden
