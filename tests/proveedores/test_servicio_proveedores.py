import pytest

from bikelogic.proveedores.excepciones import ProveedorConOrdenesActivas, ProveedorInvalido
from bikelogic.proveedores.repositorio import RepositorioProveedoresEnMemoria
from bikelogic.proveedores.servicio import ServicioProveedores

NIT = "900123456"


@pytest.fixture
def repositorio():
    return RepositorioProveedoresEnMemoria()


@pytest.fixture
def servicio(repositorio):
    return ServicioProveedores(repositorio)


def test_registrar_proveedor_valido_lo_deja_activo(servicio):
    proveedor = servicio.registrar(NIT, "ACME S.A.S.", "contacto@acme.com")

    assert proveedor.nit == NIT
    assert proveedor.nombre == "ACME S.A.S."
    assert proveedor.estado == "ACTIVO"


@pytest.mark.parametrize("nit", ["", "ABC123456", "12345", "12345678901"])
def test_registrar_con_nit_invalido_se_rechaza(servicio, nit):
    with pytest.raises(ProveedorInvalido, match="NIT inválido"):
        servicio.registrar(nit, "ACME S.A.S.", "contacto@acme.com")


@pytest.mark.parametrize("correo", ["", " ", "sin-arroba.com", "usuario@"])
def test_registrar_con_correo_invalido_se_rechaza(servicio, correo):
    with pytest.raises(ProveedorInvalido, match="Formato de correo inválido"):
        servicio.registrar(NIT, "ACME S.A.S.", correo)


def test_registrar_sin_nombre_se_rechaza(servicio):
    with pytest.raises(ProveedorInvalido, match="El nombre es obligatorio"):
        servicio.registrar(NIT, "   ", "contacto@acme.com")


def test_registrar_nit_duplicado_se_rechaza_y_conserva_el_original(servicio, repositorio):
    servicio.registrar(NIT, "ACME S.A.S.", "contacto@acme.com")

    with pytest.raises(ProveedorInvalido, match="El NIT ya está registrado"):
        servicio.registrar(NIT, "Otra empresa", "otra@empresa.com")

    assert repositorio.cantidad() == 1
    assert repositorio.obtener(NIT).nombre == "ACME S.A.S."


def test_desactivar_proveedor_sin_ordenes_activas_lo_deja_inactivo(servicio):
    servicio.registrar(NIT, "ACME S.A.S.", "contacto@acme.com")

    proveedor = servicio.desactivar(NIT)

    assert proveedor.estado == "INACTIVO"


def test_desactivar_proveedor_con_ordenes_activas_se_rechaza(repositorio):
    servicio = ServicioProveedores(repositorio, tiene_ordenes_activas=lambda nit: True)
    servicio.registrar(NIT, "ACME S.A.S.", "contacto@acme.com")

    with pytest.raises(ProveedorConOrdenesActivas, match="órdenes activas"):
        servicio.desactivar(NIT)

    assert repositorio.obtener(NIT).estado == "ACTIVO"


def test_desactivar_proveedor_inexistente_se_rechaza(servicio):
    with pytest.raises(ProveedorInvalido, match="El proveedor no existe"):
        servicio.desactivar(NIT)
