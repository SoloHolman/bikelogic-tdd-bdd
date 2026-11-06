import pytest

from bikelogic.inventario.excepciones import ProductoInvalido, StockInsuficiente
from bikelogic.inventario.repositorio import RepositorioInventarioEnMemoria
from bikelogic.inventario.servicio import ServicioInventario


@pytest.fixture
def repositorio():
    return RepositorioInventarioEnMemoria()


@pytest.fixture
def servicio(repositorio):
    return ServicioInventario(repositorio)


def test_registrar_producto_valido_guarda_su_stock(servicio):
    producto = servicio.registrar("PROD-001", "Repuestos", 10)

    assert producto.codigo == "PROD-001"
    assert producto.categoria == "Repuestos"
    assert servicio.stock_de("PROD-001") == 10


def test_registrar_producto_con_stock_cero_es_valido(servicio):
    servicio.registrar("PROD-004", "Insumos", 0)

    assert servicio.stock_de("PROD-004") == 0


def test_registrar_codigo_duplicado_se_rechaza_y_conserva_el_original(servicio):
    servicio.registrar("PROD-001", "Repuestos", 10)

    with pytest.raises(ProductoInvalido, match="El código del producto ya existe"):
        servicio.registrar("PROD-001", "Lubricantes", 5)

    assert servicio.stock_de("PROD-001") == 10


@pytest.mark.parametrize("stock", [-1, -100])
def test_registrar_con_stock_inicial_negativo_se_rechaza(servicio, stock):
    with pytest.raises(ProductoInvalido, match="El stock inicial no puede ser negativo"):
        servicio.registrar("PROD-002", "Repuestos", stock)


@pytest.mark.parametrize("categoria", ["", "   "])
def test_registrar_sin_categoria_se_rechaza(servicio, categoria):
    with pytest.raises(ProductoInvalido, match="La categoría es obligatoria"):
        servicio.registrar("PROD-003", categoria, 5)


def test_registrar_sin_codigo_se_rechaza(servicio):
    with pytest.raises(ProductoInvalido, match="El código es obligatorio"):
        servicio.registrar("  ", "Repuestos", 5)


@pytest.mark.parametrize(
    ("stock_inicial", "retiro", "stock_final"),
    [(10, 3, 7), (5, 5, 0)],
)
def test_retirar_descuenta_el_stock(servicio, stock_inicial, retiro, stock_final):
    servicio.registrar("PROD-001", "Repuestos", stock_inicial)

    servicio.retirar("PROD-001", retiro)

    assert servicio.stock_de("PROD-001") == stock_final


def test_retirar_mas_del_stock_se_rechaza_y_no_lo_modifica(servicio):
    servicio.registrar("PROD-003", "Repuestos", 2)

    with pytest.raises(StockInsuficiente, match="Stock insuficiente"):
        servicio.retirar("PROD-003", 5)

    assert servicio.stock_de("PROD-003") == 2


def test_retirar_de_un_producto_inexistente_se_rechaza(servicio):
    with pytest.raises(ProductoInvalido, match="El producto no existe"):
        servicio.retirar("PROD-999", 1)
