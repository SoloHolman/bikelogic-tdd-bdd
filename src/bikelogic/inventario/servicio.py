"""Reglas de negocio del inventario."""
from bikelogic.inventario.excepciones import ProductoInvalido, StockInsuficiente
from bikelogic.inventario.modelo import Producto

MENSAJE_REGISTRADO = "Producto registrado exitosamente"
MENSAJE_RETIRO_EXITOSO = "Retiro exitoso"


class ServicioInventario:
    """Registra productos y controla sus retiros sin dejar el stock negativo."""

    def __init__(self, repositorio):
        self._repositorio = repositorio

    def registrar(self, codigo: str, categoria: str, stock: int) -> Producto:
        """Registra un producto con código único, categoría y stock inicial no negativo."""
        if not codigo.strip():
            raise ProductoInvalido("El código es obligatorio")
        if not categoria.strip():
            raise ProductoInvalido("La categoría es obligatoria")
        if stock < 0:
            raise ProductoInvalido("El stock inicial no puede ser negativo")
        if self._repositorio.existe(codigo):
            raise ProductoInvalido("El código del producto ya existe")
        producto = Producto(codigo, categoria, stock)
        self._repositorio.guardar(producto)
        return producto

    def retirar(self, codigo: str, cantidad: int) -> Producto:
        """Descuenta unidades del stock; si no alcanzan, rechaza el retiro sin modificarlo."""
        producto = self._producto_existente(codigo)
        if cantidad > producto.stock:
            raise StockInsuficiente()
        producto.stock -= cantidad
        self._repositorio.guardar(producto)
        return producto

    def stock_de(self, codigo: str) -> int:
        return self._producto_existente(codigo).stock

    def _producto_existente(self, codigo: str) -> Producto:
        producto = self._repositorio.obtener(codigo)
        if producto is None:
            raise ProductoInvalido("El producto no existe")
        return producto
