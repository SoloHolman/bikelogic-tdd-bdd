import pytest

from bikelogic.cotizaciones.excepciones import CotizacionInvalida
from bikelogic.cotizaciones.modelo import ABIERTA, CERRADA, SolicitudCotizacion
from bikelogic.cotizaciones.repositorio import RepositorioCotizacionesEnMemoria
from bikelogic.cotizaciones.servicio import ServicioCotizaciones

SOLICITUD = "SC-001"
PROVEEDOR = "900123456"


@pytest.fixture
def repositorio():
    repositorio = RepositorioCotizacionesEnMemoria()
    repositorio.agregar_solicitud(SolicitudCotizacion(SOLICITUD, ABIERTA))
    return repositorio


@pytest.fixture
def servicio(repositorio):
    return ServicioCotizaciones(repositorio)


def test_registrar_cotizacion_sobre_solicitud_abierta_la_guarda(servicio, repositorio):
    cotizacion = servicio.registrar(SOLICITUD, PROVEEDOR, 2500000)

    assert cotizacion.codigo_solicitud == SOLICITUD
    assert cotizacion.nit_proveedor == PROVEEDOR
    assert cotizacion.monto == 2500000
    assert repositorio.cantidad() == 1


@pytest.mark.parametrize("monto", [0, -1000])
def test_registrar_con_monto_invalido_se_rechaza_y_no_guarda(servicio, repositorio, monto):
    with pytest.raises(CotizacionInvalida, match="Monto inválido"):
        servicio.registrar(SOLICITUD, PROVEEDOR, monto)

    assert repositorio.cantidad() == 0


def test_registrar_sobre_solicitud_cerrada_se_rechaza(servicio, repositorio):
    repositorio.agregar_solicitud(SolicitudCotizacion("SC-002", CERRADA))

    with pytest.raises(CotizacionInvalida, match="La solicitud no está abierta"):
        servicio.registrar("SC-002", PROVEEDOR, 2500000)

    assert repositorio.cantidad() == 0


def test_registrar_sobre_solicitud_inexistente_se_rechaza(servicio):
    with pytest.raises(CotizacionInvalida, match="La solicitud no existe"):
        servicio.registrar("SC-999", PROVEEDOR, 2500000)


def test_registrar_cotizacion_duplicada_del_mismo_proveedor_se_rechaza(servicio, repositorio):
    servicio.registrar(SOLICITUD, PROVEEDOR, 2500000)

    with pytest.raises(CotizacionInvalida, match="El proveedor ya cotizó esta solicitud"):
        servicio.registrar(SOLICITUD, PROVEEDOR, 3000000)

    assert repositorio.cantidad() == 1


def test_proveedores_distintos_pueden_cotizar_la_misma_solicitud(servicio, repositorio):
    servicio.registrar(SOLICITUD, "900123456", 2500000)
    servicio.registrar(SOLICITUD, "800987654", 2700000)

    assert repositorio.cantidad() == 2
