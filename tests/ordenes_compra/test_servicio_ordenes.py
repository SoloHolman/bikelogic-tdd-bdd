import pytest

from bikelogic.ordenes_compra.excepciones import OrdenInvalida
from bikelogic.ordenes_compra.repositorio import RepositorioOrdenesEnMemoria
from bikelogic.ordenes_compra.servicio import ServicioOrdenesCompra


@pytest.fixture
def repositorio():
    return RepositorioOrdenesEnMemoria()


@pytest.fixture
def servicio(repositorio):
    return ServicioOrdenesCompra(repositorio)


def test_aprobar_orden_dentro_del_presupuesto_la_aprueba_y_descuenta(servicio):
    servicio.registrar_proveedor("ACME", 10_000_000)
    servicio.registrar_orden("OC-100", "ACME", 5_000_000)

    orden = servicio.aprobar("OC-100")

    assert orden.estado == "APROBADA"
    assert servicio.presupuesto_de("ACME") == 5_000_000


def test_aprobar_orden_igual_al_presupuesto_la_aprueba_y_deja_presupuesto_en_cero(servicio):
    servicio.registrar_proveedor("ACME", 10_000_000)
    servicio.registrar_orden("OC-100", "ACME", 10_000_000)

    servicio.aprobar("OC-100")

    assert servicio.presupuesto_de("ACME") == 0


def test_aprobar_orden_que_excede_el_presupuesto_se_rechaza(servicio, repositorio):
    servicio.registrar_proveedor("ACME", 10_000_000)
    servicio.registrar_orden("OC-101", "ACME", 15_000_000)

    with pytest.raises(OrdenInvalida, match="Presupuesto insuficiente"):
        servicio.aprobar("OC-101")

    assert servicio.presupuesto_de("ACME") == 10_000_000
    assert repositorio.obtener_orden("OC-101").estado == "PENDIENTE"


def test_aprobar_orden_ya_aprobada_se_rechaza_y_no_descuenta_dos_veces(servicio):
    servicio.registrar_proveedor("ACME", 10_000_000)
    servicio.registrar_orden("OC-102", "ACME", 1_000_000)
    servicio.aprobar("OC-102")

    with pytest.raises(OrdenInvalida, match="La orden ya está aprobada"):
        servicio.aprobar("OC-102")

    assert servicio.presupuesto_de("ACME") == 9_000_000


def test_aprobar_orden_inexistente_se_rechaza(servicio):
    with pytest.raises(OrdenInvalida, match="La orden no existe"):
        servicio.aprobar("OC-999")


def test_registrar_orden_de_proveedor_inexistente_se_rechaza(servicio):
    with pytest.raises(OrdenInvalida, match="El proveedor no existe"):
        servicio.registrar_orden("OC-100", "FANTASMA", 1_000_000)


def test_registrar_orden_con_numero_repetido_se_rechaza(servicio):
    servicio.registrar_proveedor("ACME", 10_000_000)
    servicio.registrar_orden("OC-100", "ACME", 1_000_000)

    with pytest.raises(OrdenInvalida, match="El número de orden ya está registrado"):
        servicio.registrar_orden("OC-100", "ACME", 2_000_000)


def test_registrar_proveedor_con_presupuesto_negativo_se_rechaza(servicio):
    with pytest.raises(OrdenInvalida, match="El presupuesto no puede ser negativo"):
        servicio.registrar_proveedor("ACME", -1)
